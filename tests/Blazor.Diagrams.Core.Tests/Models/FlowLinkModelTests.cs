using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models;

public class FlowLinkModelTests
{
    private static FlowLinkModel NewLink() =>
        new(new PortModel(new NodeModel()), new PortModel(new NodeModel()));

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        var link = NewLink();

        link.FlowDirection.Should().Be(FlowDirection.None);
        link.FlowSpeed.Should().BeApproximately(1.0, 0.001);
        link.FlowDashSize.Should().BeApproximately(10.0, 0.001);
        link.FlowGapSize.Should().BeApproximately(10.0, 0.001);
        link.FlowColor.Should().BeNull();
        link.FlowWidth.Should().BeNull();
    }

    [Fact]
    public void FlowDirection_CanBeChanged()
    {
        var link = NewLink();

        link.FlowDirection = FlowDirection.Forward;
        link.FlowDirection.Should().Be(FlowDirection.Forward);

        link.FlowDirection = FlowDirection.Reverse;
        link.FlowDirection.Should().Be(FlowDirection.Reverse);

        link.FlowDirection = FlowDirection.Paused;
        link.FlowDirection.Should().Be(FlowDirection.Paused);
    }

    [Fact]
    public void FlowDirection_Change_TriggersRefresh()
    {
        var link = NewLink();
        var refreshCount = 0;
        link.Changed += _ => refreshCount++;

        link.FlowDirection = FlowDirection.Forward;
        refreshCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void FlowSpeed_IsClampedToMinimum()
    {
        var link = NewLink();
        link.FlowSpeed = -5.0;
        link.FlowSpeed.Should().BeGreaterThan(0);
    }

    [Fact]
    public void FlowDashSize_IsClampedToMinimum()
    {
        var link = NewLink();
        link.FlowDashSize = 0;
        link.FlowDashSize.Should().BeGreaterOrEqualTo(1);
    }

    [Theory]
    [InlineData(FlowDirection.Forward)]
    [InlineData(FlowDirection.Reverse)]
    public void FlowAnimateTo_IsNonZeroForAnimatedDirections(FlowDirection dir)
    {
        var link = NewLink();
        link.FlowDashSize = 10;
        link.FlowDirection = dir;

        link.FlowAnimateTo.Should().NotBe("0");
    }

    [Theory]
    [InlineData(FlowDirection.None)]
    [InlineData(FlowDirection.Paused)]
    public void FlowAnimateTo_IsZeroForNonAnimated(FlowDirection dir)
    {
        var link = NewLink();
        link.FlowDirection = dir;

        link.FlowAnimateTo.Should().Be("0");
    }

    [Fact]
    public void FlowAnimateTo_ForwardIsNegative()
    {
        var link = NewLink();
        link.FlowDashSize = 10;
        link.FlowDirection = FlowDirection.Forward;

        link.FlowAnimateTo.Should().StartWith("-");
    }

    [Fact]
    public void FlowAnimateTo_ReverseIsPositive()
    {
        var link = NewLink();
        link.FlowDashSize = 10;
        link.FlowDirection = FlowDirection.Reverse;

        double.Parse(link.FlowAnimateTo, System.Globalization.CultureInfo.InvariantCulture)
            .Should().BeGreaterThan(0);
    }

    [Fact]
    public void ResolvedFlowWidth_DefaultsToHalfBaseWidth()
    {
        var link = NewLink();
        link.Width = 6;

        link.ResolvedFlowWidth.Should().Be(3);
    }

    [Fact]
    public void ResolvedFlowWidth_UsesExplicitFlowWidth()
    {
        var link = NewLink();
        link.Width = 6;
        link.FlowWidth = 2;

        link.ResolvedFlowWidth.Should().Be(2);
    }

    [Fact]
    public void ResolvedFlowColor_FallsBackToLinkColor()
    {
        var link = NewLink();
        link.Color = "#ff0000";

        link.ResolvedFlowColor.Should().Be("#ff0000");
    }

    [Fact]
    public void ResolvedFlowColor_UsesExplicitFlowColor()
    {
        var link = NewLink();
        link.Color = "#ff0000";
        link.FlowColor = "#00ff00";

        link.ResolvedFlowColor.Should().Be("#00ff00");
    }

    [Fact]
    public void FlowGapSize_CanBeChanged()
    {
        var link = NewLink();
        link.FlowGapSize = 20;
        link.FlowGapSize.Should().BeApproximately(20.0, 0.001);
    }

    [Fact]
    public void FlowGapSize_ClampedToMinimumOfOne()
    {
        var link = NewLink();
        link.FlowGapSize = 0;
        link.FlowGapSize.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void FlowGapSize_Change_TriggersRefresh()
    {
        var link = NewLink();
        var refreshCount = 0;
        link.Changed += _ => refreshCount++;

        link.FlowGapSize = 15;
        refreshCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void FlowAnimateTo_Forward_UsesDashPlusGap()
    {
        var link = NewLink();
        link.FlowDashSize = 10;
        link.FlowGapSize = 5;
        link.FlowDirection = FlowDirection.Forward;

        link.FlowAnimateTo.Should().Be("-15");
    }

    [Fact]
    public void FlowAnimateTo_Reverse_UsesDashPlusGap()
    {
        var link = NewLink();
        link.FlowDashSize = 10;
        link.FlowGapSize = 5;
        link.FlowDirection = FlowDirection.Reverse;

        link.FlowAnimateTo.Should().Be("15");
    }

    [Fact]
    public void Constructor_WithPorts_Succeeds()
    {
        var srcPort = new PortModel(new NodeModel());
        var tgtPort = new PortModel(new NodeModel());
        var link = new FlowLinkModel(srcPort, tgtPort);
        link.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithIdAndPorts_Succeeds()
    {
        var srcPort = new PortModel(new NodeModel());
        var tgtPort = new PortModel(new NodeModel());
        var link = new FlowLinkModel("my-id", srcPort, tgtPort);
        link.Id.Should().Be("my-id");
    }
}
