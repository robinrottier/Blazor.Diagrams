using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models;

public class LinkModel : BaseLinkModel
{
    public LinkModel(Anchor source, Anchor target) : base(source, target) { }

    public LinkModel(string id, Anchor source, Anchor target) : base(id, source, target) { }

    public LinkModel(PortModel? sourcePort, PortModel? targetPort)
        : base(sourcePort == null ? null : new SinglePortAnchor(sourcePort),
               targetPort == null ? null : new SinglePortAnchor(targetPort)) { }

    public LinkModel(NodeModel sourceNode, NodeModel targetNode)
        : base(new ShapeIntersectionAnchor(sourceNode), new ShapeIntersectionAnchor(targetNode)) { }

    public LinkModel(string id, PortModel sourcePort, PortModel targetPort)
        : base(id, new SinglePortAnchor(sourcePort), new SinglePortAnchor(targetPort)) { }

    public LinkModel(string id, NodeModel sourceNode, NodeModel? targetNode)
        : base(id, new ShapeIntersectionAnchor(sourceNode), 
            targetNode == null ? null : new ShapeIntersectionAnchor(targetNode)) { }

    public string? Color { get; set; }
    public string? SelectedColor { get; set; }

    /// <summary>
    /// Set a value equal to values accepted by SVG's 'stroke-dasharray' attribute.
    /// For example '5,5' will result in patter where the line is drawn for 5px and not drawn for another 5px.
    /// </summary>
    public string? DashPattern { get; set; }
    public double Width { get; set; } = 2;

    /// <summary>
    /// Collection of Animation elements to add to this path.
    /// Each element will be converted to an SVG <animate> element and added as a child of the path element.
    /// </summary>
    public List<AnimateModel>? Animations { get; private set; }

    public void AddAnimation(AnimateModel animation)
    {
        Animations ??= new List<AnimateModel>();
        Animations.Add(animation);
    }

    public void RemoveAnimation(AnimateModel animation)
    {
        Animations?.Remove(animation);
    }

}
