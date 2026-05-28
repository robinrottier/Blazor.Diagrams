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
    private double _dashSize = 10;
    private double _gapSize = 10;
    private string? _color;
    private double _width = 3;

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
        // 2x2 grid — positioned below the info box
        var node1 = NewNode(50,  270);   // top-left
        var node2 = NewNode(310, 270);   // top-right
        var node3 = NewNode(50,  450);   // bottom-left
        var node4 = NewNode(310, 450);   // bottom-right

        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3, node4 });

        // Two horizontal flow links
        _links.Add(AddFlow(node1.GetPort(PortAlignment.Right)!, node2.GetPort(PortAlignment.Left)!, "#00aa44"));
        _links.Add(AddFlow(node3.GetPort(PortAlignment.Right)!, node4.GetPort(PortAlignment.Left)!, "#ff8800"));

        // One plain curved link looping down the right side (no flow animation)
        var plainLink = new LinkModel(node2.GetPort(PortAlignment.Right)!, node4.GetPort(PortAlignment.Right)!);
        _blazorDiagram.Links.Add(plainLink);
        _blazorDiagram.Links.Add(_links.ToArray());
    }

    private FlowLinkModel AddFlow(PortModel source, PortModel target, string baseColor)
    {
        var link = new FlowLinkModel(source, target)
        {
            Color = baseColor,
            Width = _width,
            FlowDirection = _direction,
            FlowSpeed = _speed,
            FlowDashSize = _dashSize,
            FlowGapSize = _gapSize,
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

    private void OnDashSizeChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _dashSize = val;
            foreach (var link in _links) link.FlowDashSize = val;
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

    private void OnLineColorChanged(ChangeEventArgs e)
    {
        var val = e.Value?.ToString();
        foreach (var link in _links)
        {
            link.Color = string.IsNullOrEmpty(val) ? null : val;
            link.Refresh();
        }
    }

    private void OnColorChanged(ChangeEventArgs e)
    {
        _color = e.Value?.ToString();
        foreach (var link in _links)
            link.FlowColor = string.IsNullOrEmpty(_color) ? null : _color;
    }

    private void OnWidthChanged(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            _width = val;
            foreach (var link in _links)
            {
                link.Width = val;
                link.Refresh();
            }
        }
    }
}
