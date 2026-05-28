using Blazor.Diagrams;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos.Links;

public partial class FlowLinkDemo
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();
    private readonly List<FlowLinkModel> _links = new();
    private FlowDirection _direction = FlowDirection.Forward;
    private double _speed = 1.0;
    private double _flowSize = 20;
    private double _gapSize = 15;
    private string? _color = "#0088ff";      // flow color: blue
    private string? _lineColor = "#ff8800";  // line color: orange
    private double _flowWidth = 10;
    private double _lineWidth = 20;
    private FlowShape _flowShape = FlowShape.Arrow;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Flow Links";
        LayoutData.Info = "FlowLinkModel renders a marching-ants animated overlay on top of the base link path. " +
            "Use the panel to control direction, speed, dash size, and color in real time.";
        LayoutData.DataChanged();

        _blazorDiagram.RegisterComponent<FlowLinkModel, FlowLinkWidget>();
        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        // Compact 2×2 grid — short lines so big shapes look impactful at startup
        var node1 = NewNode(80,  180);   // top-left
        var node2 = NewNode(280, 180);   // top-right
        var node3 = NewNode(80,  360);   // bottom-left
        var node4 = NewNode(280, 360);   // bottom-right

        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3, node4 });

        // Two short horizontal flow links
        _links.Add(AddFlow(node1.GetPort(PortAlignment.Right)!, node2.GetPort(PortAlignment.Left)!));
        _links.Add(AddFlow(node3.GetPort(PortAlignment.Right)!, node4.GetPort(PortAlignment.Left)!));

        // Short vertical flow link on the right side
        _links.Add(AddFlow(node2.GetPort(PortAlignment.Right)!, node4.GetPort(PortAlignment.Right)!));

        _blazorDiagram.Links.Add(_links.ToArray());
    }

    private FlowLinkModel AddFlow(PortModel source, PortModel target)
    {
        var link = new FlowLinkModel(source, target)
        {
            Color = _lineColor,
            FlowWidth = _flowWidth,
            FlowDirection = _direction,
            FlowSpeed = _speed,
            FlowSize = _flowSize,
            FlowGapSize = _gapSize,
            FlowShape = _flowShape,
            LineWidth = _lineWidth,
        };
        if (!string.IsNullOrEmpty(_color))
            link.FlowColor = _color;
        return link;
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        return node;
    }

    private void OnDirectionChanged(ChangeEventArgs e)
    {
        if (Enum.TryParse<FlowDirection>(e.Value?.ToString(), out var dir))
        {
            _direction = dir;
            foreach (var link in _links) link.FlowDirection = dir;
        }
    }

    private void OnSpeedChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _speed = val;
            foreach (var link in _links) link.FlowSpeed = val;
        }
    }

    private void OnFlowSizeChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _flowSize = val;
            foreach (var link in _links) link.FlowSize = val;
        }
    }

    private void OnGapSizeChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _gapSize = val;
            foreach (var link in _links) link.FlowGapSize = val;
        }
    }

    private void OnFlowShapeChanged(ChangeEventArgs e)
    {
        if (Enum.TryParse<FlowShape>(e.Value?.ToString(), out var shape))
        {
            _flowShape = shape;
            foreach (var link in _links) link.FlowShape = shape;
        }
    }

    private void OnLineColorChanged(ChangeEventArgs e)
    {
        _lineColor = string.IsNullOrEmpty(e.Value?.ToString()) ? null : e.Value.ToString();
        foreach (var link in _links)
        {
            link.Color = _lineColor;
            link.Refresh();
        }
    }

    private void OnColorChanged(ChangeEventArgs e)
    {
        _color = e.Value?.ToString();
        foreach (var link in _links)
            link.FlowColor = string.IsNullOrEmpty(_color) ? null : _color;
    }

    private void OnFlowWidthChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _flowWidth = val;
            foreach (var link in _links) link.FlowWidth = val;
        }
    }

    private void OnLineWidthChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _lineWidth = val;
            foreach (var link in _links) link.LineWidth = val;
        }
    }
}
