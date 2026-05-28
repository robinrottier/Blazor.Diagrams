using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components;

public partial class FlowLinkWidget
{
    private bool _hovered;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;

    /// <summary>The link, typed as <see cref="LinkModel"/> to match the name expected by <c>LinkRenderer</c>.</summary>
    [Parameter] public LinkModel Link { get; set; } = null!;

    private FlowLinkModel FlowLink => (FlowLinkModel)Link;

    /// <summary>
    /// Builds the SVG path string for a single animateMotion shape, centered at the origin.
    /// Forward shapes point in the +x direction; <paramref name="reverse"/> shapes point in -x.
    /// (<c>rotate="auto"</c> then orients along the path tangent.)
    /// </summary>
    public static string BuildShapePath(FlowShape shape, double size, double width, bool reverse = false)
    {
        var s = Math.Max(1.0, size);
        // d is the x-direction sign: +1 for forward (tip at +x), -1 for reverse (tip at -x)
        var d = reverse ? -1.0 : 1.0;
        var strokeW = Math.Max(1.0, width / 4.0);
        // Arrow (filled): half-height spans the full flow overlay width
        var hArrow = Math.Max(0.5, width / 2.0);
        // Chevrons (stroked): reduce h so the outer stroke edge stays within the flow overlay width
        var hChev = Math.Max(0.5, (width - strokeW) / 2.0);

        static string F(double v) => v.ToInvariantString();

        return shape switch
        {
            FlowShape.Arrow =>
                $"M {F(-d * s / 2)},{F(-hArrow)} L {F(d * s / 2)},0 L {F(-d * s / 2)},{F(hArrow)} Z",

            FlowShape.Chevron =>
                $"M {F(-d * s / 2)},{F(-hChev)} L {F(d * s / 2)},0 L {F(-d * s / 2)},{F(hChev)}",

            FlowShape.DblChevron =>
                $"M 0,{F(-hChev)} L {F(d * s / 2)},0 L 0,{F(hChev)} " +
                $"M {F(d * -s / 2)},{F(-hChev)} L 0,0 L {F(d * -s / 2)},{F(hChev)}",

            FlowShape.TripleChevron =>
                $"M {F(d * s / 6)},{F(-hChev)} L {F(d * s / 2)},0 L {F(d * s / 6)},{F(hChev)} " +
                $"M {F(d * -s / 6)},{F(-hChev)} L {F(d * s / 6)},0 L {F(d * -s / 6)},{F(hChev)} " +
                $"M {F(d * -s / 2)},{F(-hChev)} L {F(d * -s / 6)},0 L {F(d * -s / 2)},{F(hChev)}",

            _ => ""
        };
    }

    private RenderFragment GetSelectionHelperPath(string color, string d, int index)
    {
        return builder =>
        {
            builder.OpenElement(0, "path");
            builder.AddAttribute(1, "class", "selection-helper");
            builder.AddAttribute(2, "stroke", color);
            builder.AddAttribute(3, "stroke-width", 12);
            builder.AddAttribute(4, "d", d);
            builder.AddAttribute(5, "stroke-linecap", "butt");
            builder.AddAttribute(6, "stroke-opacity", _hovered ? "0.05" : "0");
            builder.AddAttribute(7, "fill", "none");
            builder.AddAttribute(8, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseEnter));
            builder.AddAttribute(9, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseLeave));
            builder.AddAttribute(10, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, e => OnPointerDown(e, index)));
            builder.AddEventStopPropagationAttribute(11, "onpointerdown", FlowLink.Segmentable);
            builder.CloseElement();
        };
    }

    private void OnPointerDown(PointerEventArgs e, int index)
    {
        if (!FlowLink.Segmentable)
            return;

        var rPt = BlazorDiagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
        var vertex = new LinkVertexModel(FlowLink, rPt);
        FlowLink.Vertices.Insert(index, vertex);
        FlowLink.Refresh();
        BlazorDiagram.TriggerPointerDown(vertex, e.ToCore());
    }

    private void OnMouseEnter(MouseEventArgs e) => _hovered = true;

    private void OnMouseLeave(MouseEventArgs e) => _hovered = false;
}
