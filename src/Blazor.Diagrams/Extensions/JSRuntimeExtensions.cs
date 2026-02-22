using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Extensions;

public static class JSRuntimeExtensions
{
    public static async Task<Rectangle?> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
    {
        try
        {
            return await jsRuntime.InvokeAsync<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", element);
        }
        catch (Exception ex) when (
                   ex is ObjectDisposedException
                || ex is InvalidOperationException
                || ex is JSDisconnectedException
                || ex is TaskCanceledException
            )
        {
            return null;
        }
    }

    public static async Task ObserveResizes<T>(this IJSRuntime jsRuntime, ElementReference element,
        DotNetObjectReference<T> reference) where T : class
    {
        await SafeJSRuntimeInvokeVoidAsync(jsRuntime, "ZBlazorDiagrams.observe", element, reference, element.Id);
    }

    public static async Task UnobserveResizes(this IJSRuntime jsRuntime, ElementReference element)
    {
        await SafeJSRuntimeInvokeVoidAsync(jsRuntime, "ZBlazorDiagrams.unobserve", element, element.Id);
    }

    private static async Task SafeJSRuntimeInvokeVoidAsync(IJSRuntime jsRuntime, string identifier, params object?[]? args)
    {
        //
        // protect js runtime calls against runtime being uninitialized, disconnected or disposed or otherwise failing
        // which can happen when the user navigates away from the page or when the component is disposed
        // .net10 seems to introduce InvalidOperationException when simply reloading a server side render page. The exception says:
        //      JavaScript interop calls cannot be issued at this time. This is because the component is being statically rendered.
        //      When prerendering is enabled, JavaScript interop calls can only be performed during the OnAfterRenderAsync lifecycle method.
        //
        // perhaps a catch-all should be added -- do we care if a void task in remote runtime fails in any way?
        //
        // (in .net10) jsRuntime is a Microsoft.AspNetCore.Components.Server.Circuits.RemoteJSRuntime 
        // - it has ".IsInitialized" which seems to be false when this fails so that could be tested before call to avois exceptin needing to be caught
        // - but dont know how to reference it and reflection may be too much here
        //
        try
        {
            await jsRuntime.InvokeVoidAsync(identifier, args);
        }
        catch (Exception ex) when (
                   ex is ObjectDisposedException 
                || ex is InvalidOperationException 
                || ex is JSDisconnectedException
                || ex is TaskCanceledException
            )
        {
            // This can happens alot in some "normal" operations,
            // System.Diagnostics.Debug.WriteLine($"Ignore {ex.GetType().FullName} from JSRuntime.{identifier}");
        }
    }
}