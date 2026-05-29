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
/// Visual shape of each moving unit in the flow animation.
/// </summary>
public enum FlowShape
{
    /// <summary>Rounded pill / dash (default). <see cref="FlowLinkModel.FlowSize"/> = 0 produces dots/circles.</summary>
    Dash,
    /// <summary>Square-ended rectangle dash — same as Dash but with flat caps.</summary>
    Rectangle,
    /// <summary>Solid filled triangle pointing in the flow direction.</summary>
    Arrow,
    /// <summary>Open chevron "&gt;" pointing in the flow direction.</summary>
    Chevron,
    /// <summary>Two open chevrons "&gt;&gt;" pointing in the flow direction.</summary>
    DblChevron,
    /// <summary>Three open chevrons "&gt;&gt;&gt;" pointing in the flow direction.</summary>
    TripleChevron,
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
    private double _flowSize = 10;
    private double _flowGapSize = 10;
    private FlowShape _flowShape = FlowShape.Dash;
    private LinkMarker? _flowMarker;

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
        set { _flowDirection = value; UpdateFlowMarkers(); Refresh(); }
    }

    /// <summary>
    /// Width of the animated flow overlay stroke. Defaults to half of <see cref="LinkModel.Width"/> when null.
    /// </summary>
    public double? FlowWidth
    {
        get => _flowWidth;
        set { _flowWidth = value; Refresh(); }
    }

    /// <summary>
    /// Size of each moving unit in the flow animation.
    /// For <see cref="FlowShape.Dash"/>/<see cref="FlowShape.Rectangle"/> this is the dash length in px (0 = dot/circle).
    /// For shape modes this is the bounding-box width of each shape in px.
    /// Default is 10 px.
    /// </summary>
    public double FlowSize
    {
        get => _flowSize;
        set { _flowSize = Math.Max(0, value); Refresh(); }
    }

    /// <summary>
    /// Gap between each moving unit.
    /// Applies to <see cref="FlowShape.Dash"/> and <see cref="FlowShape.Rectangle"/> only.
    /// Default is 10 px.
    /// </summary>
    public double FlowGapSize
    {
        get => _flowGapSize;
        set { _flowGapSize = Math.Max(1, value); Refresh(); }
    }

    /// <summary>
    /// Visual shape of each moving unit. Default is <see cref="FlowShape.Dash"/>.
    /// </summary>
    public FlowShape FlowShape
    {
        get => _flowShape;
        set { _flowShape = value; Refresh(); }
    }

    /// <summary>
    /// A marker placed at the flow-direction end of the link.
    /// When set, <see cref="BaseLinkModel.SourceMarker"/> and <see cref="BaseLinkModel.TargetMarker"/>
    /// are managed automatically: the marker appears at whichever end data is currently flowing toward.
    /// Setting this to <c>null</c> clears both markers.
    /// </summary>
    public LinkMarker? FlowMarker
    {
        get => _flowMarker;
        set
        {
            if (value == null && _flowMarker != null)
            {
                // Clearing: remove markers we may have set
                SourceMarker = null;
                TargetMarker = null;
            }
            _flowMarker = value;
            UpdateFlowMarkers();
            Refresh();
        }
    }

    /// <summary>
    /// Resolved flow overlay stroke width — half of <see cref="LinkModel.Width"/>, or <see cref="FlowWidth"/> if set.
    /// When <see cref="LinkModel.Width"/> is 0 and <see cref="FlowWidth"/> is not set, returns 0 (invisible).
    /// Set <see cref="FlowWidth"/> explicitly to show a flow overlay with no base line.
    /// </summary>
    public double ResolvedFlowWidth => FlowWidth ?? Math.Max(0, Width / 2.0);

    /// <summary>
    /// Resolved flow color — <see cref="FlowColor"/> if set, else <see cref="LinkModel.Color"/>.
    /// </summary>
    public string? ResolvedFlowColor => FlowColor ?? Color;

    /// <summary>
    /// SVG stroke-dashoffset 'to' value for the animate element (Dash/Rectangle shapes).
    /// Forward = negative offset (source→target), Reverse = positive, None/Paused = "0".
    /// </summary>
    public string FlowAnimateTo => FlowDirection switch
    {
        FlowDirection.Forward => (-(FlowSize + FlowGapSize)).ToString("G", System.Globalization.CultureInfo.InvariantCulture),
        FlowDirection.Reverse => (FlowSize + FlowGapSize).ToString("G", System.Globalization.CultureInfo.InvariantCulture),
        _ => "0"
    };

    /// <summary>
    /// Updates <see cref="BaseLinkModel.SourceMarker"/> and <see cref="BaseLinkModel.TargetMarker"/>
    /// based on <see cref="FlowMarker"/> and the current <see cref="FlowDirection"/>.
    /// Has no effect when <see cref="FlowMarker"/> is null.
    /// </summary>
    private void UpdateFlowMarkers()
    {
        if (_flowMarker == null) return;
        SourceMarker = _flowDirection == FlowDirection.Reverse ? _flowMarker : null;
        TargetMarker = _flowDirection == FlowDirection.Forward ? _flowMarker : null;
    }
}
