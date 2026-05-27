using Blazor.Diagrams.Core.Anchors;

namespace Blazor.Diagrams.Core.Models;

/// <summary>
/// Direction of animated flow along a link.
/// </summary>
public enum FlowDirection
{
    /// <summary>No animation — link is drawn but static.</summary>
    None,
    /// <summary>Flow moves from source to target.</summary>
    Forward,
    /// <summary>Flow moves from target to source.</summary>
    Reverse,
    /// <summary>Animation is frozen at current position (e.g. zero flow).</summary>
    Paused
}

/// <summary>
/// A <see cref="LinkModel"/> that adds an animated "marching ants" flow overlay.
/// The flow direction and speed can be driven at runtime (e.g. by an MQTT data value).
/// Register a <c>FlowLinkWidget</c> for this type to render the layered SVG.
/// </summary>
public class FlowLinkModel : LinkModel
{
    private FlowDirection _flowDirection = FlowDirection.None;
    private double _flowSpeed = 1.0;
    private string? _flowColor;
    private double? _flowWidth;
    private double _flowDashSize = 10;

    public FlowLinkModel(Anchor source, Anchor target) : base(source, target) { }

    public FlowLinkModel(string id, Anchor source, Anchor target) : base(id, source, target) { }

    public FlowLinkModel(PortModel? sourcePort, PortModel? targetPort = null)
        : base(sourcePort, targetPort) { }

    public FlowLinkModel(NodeModel sourceNode, NodeModel targetNode)
        : base(sourceNode, targetNode) { }

    public FlowLinkModel(string id, PortModel sourcePort, PortModel targetPort)
        : base(id, sourcePort, targetPort) { }

    /// <summary>
    /// Color of the animated flow path. Defaults to <see cref="LinkModel.Color"/> when null.
    /// </summary>
    public string? FlowColor
    {
        get => _flowColor;
        set { _flowColor = value; Refresh(); }
    }

    /// <summary>
    /// Duration of one animation cycle in seconds. Default is 1.0.
    /// </summary>
    public double FlowSpeed
    {
        get => _flowSpeed;
        set { _flowSpeed = Math.Max(0.05, value); Refresh(); }
    }

    /// <summary>
    /// Direction of the flow animation. Changing this calls <see cref="LinkModel.Refresh()"/>
    /// so the SVG animate element updates immediately.
    /// </summary>
    public FlowDirection FlowDirection
    {
        get => _flowDirection;
        set { _flowDirection = value; Refresh(); }
    }

    /// <summary>
    /// Width of the animated inner path. Defaults to half of <see cref="LinkModel.Width"/> when null.
    /// </summary>
    public double? FlowWidth
    {
        get => _flowWidth;
        set { _flowWidth = value; Refresh(); }
    }

    /// <summary>
    /// Size of each dash and gap in the marching-ants pattern. Default is 10 px.
    /// </summary>
    public double FlowDashSize
    {
        get => _flowDashSize;
        set { _flowDashSize = Math.Max(1, value); Refresh(); }
    }

    /// <summary>
    /// Resolved flow path stroke width — half of base <see cref="LinkModel.Width"/>, or <see cref="FlowWidth"/> if set.
    /// </summary>
    public double ResolvedFlowWidth => FlowWidth ?? Math.Max(1, Width / 2.0);

    /// <summary>
    /// Resolved flow color — <see cref="FlowColor"/> if set, else <see cref="LinkModel.Color"/>.
    /// </summary>
    public string? ResolvedFlowColor => FlowColor ?? Color;

    /// <summary>
    /// SVG stroke-dashoffset 'to' value for the animate element based on the current <see cref="FlowDirection"/>.
    /// Forward = negative offset (moves source→target), Reverse = positive, None/Paused = "0".
    /// </summary>
    public string FlowAnimateTo => FlowDirection switch
    {
        FlowDirection.Forward => (-FlowDashSize * 2).ToString("G", System.Globalization.CultureInfo.InvariantCulture),
        FlowDirection.Reverse => (FlowDashSize * 2).ToString("G", System.Globalization.CultureInfo.InvariantCulture),
        _ => "0"
    };
}
