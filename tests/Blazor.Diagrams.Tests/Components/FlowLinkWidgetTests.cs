using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class FlowLinkWidgetTests
{
    [Theory]
    [InlineData(FlowShape.Arrow)]
    [InlineData(FlowShape.Chevron)]
    [InlineData(FlowShape.DblChevron)]
    [InlineData(FlowShape.TripleChevron)]
    public void BuildShapePath_ReturnsNonEmptyForMotionShapes(FlowShape shape)
    {
        FlowLinkWidget.BuildShapePath(shape, 10, 10).Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(FlowShape.Dash)]
    [InlineData(FlowShape.Rectangle)]
    public void BuildShapePath_ReturnsEmptyForDashShapes(FlowShape shape)
    {
        FlowLinkWidget.BuildShapePath(shape, 10, 10).Should().BeEmpty();
    }

    [Fact]
    public void BuildShapePath_Arrow_ContainsZ()
    {
        FlowLinkWidget.BuildShapePath(FlowShape.Arrow, 10, 10).Should().Contain("Z");
    }

    [Fact]
    public void BuildShapePath_Chevron_DoesNotContainZ()
    {
        FlowLinkWidget.BuildShapePath(FlowShape.Chevron, 10, 10).Should().NotContain("Z");
    }

    [Fact]
    public void BuildShapePath_DegenerateSize_UsesMinimum()
    {
        var path = FlowLinkWidget.BuildShapePath(FlowShape.Arrow, 0, 10);
        path.Should().NotContainAny("NaN", "Infinity");
    }

    [Fact]
    public void BuildShapePath_Arrow_Forward_TipAtPositiveX()
    {
        // Forward arrow: tip is at +s/2 = +5
        var path = FlowLinkWidget.BuildShapePath(FlowShape.Arrow, 10, 10, reverse: false);
        path.Should().Contain("L 5,");
    }

    [Fact]
    public void BuildShapePath_Arrow_Reverse_TipAtNegativeX()
    {
        // Reverse arrow: tip is at -s/2 = -5
        var path = FlowLinkWidget.BuildShapePath(FlowShape.Arrow, 10, 10, reverse: true);
        path.Should().Contain("L -5,");
    }

    [Fact]
    public void BuildShapePath_Chevron_Forward_AndReverse_Differ()
    {
        var fwd = FlowLinkWidget.BuildShapePath(FlowShape.Chevron, 10, 10, reverse: false);
        var rev = FlowLinkWidget.BuildShapePath(FlowShape.Chevron, 10, 10, reverse: true);
        fwd.Should().NotBe(rev);
    }

    [Fact]
    public void BuildShapePath_Chevron_HeightConstrainedByWidth()
    {
        // With width=10, strokeW=max(1, 10/4)=2.5, h = (10 - 2.5)/2 = 3.75
        // Y-coordinates in the path should be ±3.75 (not ±5)
        var path = FlowLinkWidget.BuildShapePath(FlowShape.Chevron, 10, 10, reverse: false);
        path.Should().Contain(",3.75")
            .And.NotContain(",-5")
            .And.NotContain(",5");
    }

    [Fact]
    public void BuildShapePath_Arrow_HeightEqualsHalfWidth()
    {
        // Arrow with width=10: h = 5
        var path = FlowLinkWidget.BuildShapePath(FlowShape.Arrow, 10, 10, reverse: false);
        path.Should().Contain("-5");
    }

    [Theory]
    [InlineData(FlowShape.Arrow)]
    [InlineData(FlowShape.Chevron)]
    [InlineData(FlowShape.DblChevron)]
    [InlineData(FlowShape.TripleChevron)]
    public void BuildShapePath_Reverse_ReturnsNonEmpty(FlowShape shape)
    {
        FlowLinkWidget.BuildShapePath(shape, 10, 10, reverse: true).Should().NotBeNullOrEmpty();
    }
}

