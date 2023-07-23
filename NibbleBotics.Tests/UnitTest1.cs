using FluentAssertions;
using System.Numerics;
using NibbleKinematics;

public class ArmTests
{
    private const float tolerance = 0.0001f;

    [Fact]
    public void TestSegmentCreation()
    {
        Segment segment = new Segment(1, Quaternion.Identity);
        segment.Length.Should().Be(1);
        segment.RotationDegrees.Should().Be(Vector3.Zero);
    }

    [Fact]
    public void TestCalculateEndpoint()
    {
        Segment segment = new Segment(1, Quaternion.Identity);
        segment.StartPoint = new Vector3(0, 0, 0);
        segment.CalculateEndpoint();

        segment.EndPoint.Should().Be(new Vector3(0, -1, 0));
    }

    [Fact]
    public void TestArmCreation()
    {
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        
        arm.Segments.Length.Should().Be(3);
        foreach (Segment segment in arm.Segments)
        {
            segment.Length.Should().Be(1);
            segment.RotationDegrees.Should().Be(Vector3.Zero);
        }
    }

    [Fact]
    public void TestSolveForward()
    {
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        
        arm.Segments[0].StartPoint = new Vector3(0, 0, 0);
        arm.SolveForward();

        Vector3.Distance(new Vector3(0, -3, 0), arm.Segments[2].EndPoint).Should().BeLessThan(tolerance);
    }

    [Fact]
    public void TestSolveForwardWithRotation()
    {
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        
        arm.Segments[0].StartPoint = new Vector3(0, 0, 0);
        arm.Segments[0].RotateZDegrees(90);
        arm.Segments[1].RotateZDegrees(0);
        arm.Segments[2].RotateZDegrees(0);
        arm.SolveForward();

        Vector3.Distance(new Vector3(3, 0, 0), arm.EndPoint).Should().BeLessThan(tolerance);
    }
    
    [Fact]
    public void TestSolveForwardWithRotation2()
    {
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        
        arm.Segments[0].StartPoint = new Vector3(0, 0, 0);
        arm.Segments[0].RotateZDegrees(90);
        arm.Segments[1].RotateZDegrees(0);
        arm.Segments[2].RotateZDegrees(90);
        arm.SolveForward();

        Vector3.Distance(new Vector3(2, 1, 0), arm.EndPoint).Should().BeLessThan(tolerance);
    }
}