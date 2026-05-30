using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Bunit;
using Xunit;

namespace Blazor.Diagrams.Tests.Components
{
    public class DiagramCanvasTests
    {
        [Fact]
        public void Behavior_WhenDisposing_ShouldUnsubscribeToResizes()
        {
            // Arrange
            JSRuntimeInvocationHandler call;
            using (var ctx = new BunitContext())
            {
                ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);
                call = ctx.JSInterop.SetupVoid("ZBlazorDiagrams.unobserve", _ => true).SetVoidResult();

                // Act
                var cut = ctx.Render<DiagramCanvas>(p => p.Add(n => n.BlazorDiagram, new BlazorDiagram()));
            }

            // Assert
            call.VerifyInvoke("ZBlazorDiagrams.unobserve", calledTimes: 1);
        }
    }
}
