using FluentAssertions;
using System.Numerics;
using NibbleKinematics;

public class ArmTests
{
    private const float tolerance = 0.25f;

    [Fact]
    public void TestSegmentCreation()
    {
        var arm = new Arm();
        Segment segment = new Segment(arm,1,Segment.EVectorAxis.Z, Quaternion.Identity);
        segment.Length.Should().Be(1);
        segment.RotationDegrees.Should().Be(0);
    }

    [Fact]
    public void TestCalculateEndpoint()
    {
        var arm = new Arm();
        Segment segment = new Segment(arm,1,Segment.EVectorAxis.Z, Quaternion.Identity);
        segment.StartPoint = new Vector3(0, 0, 0);
        //segment.CalculateEndpoint();

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
        
        arm.Segments.Count.Should().Be(3);
        foreach (Segment segment in arm.Segments)
        {
            segment.Length.Should().Be(1);
            segment.RotationDegrees.Should().Be(0);
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
        
        arm.SendDegreesSignal(90,0,0);

        Vector3.Distance(new Vector3(3, 0, 0), arm.EndPoint)
            .Should()
            .BeLessThan(tolerance,because:"Found vector {0}",arm.EndPoint);
    }
    
    [Fact]
    public void TestSolveForwardWithRotation2()
    {
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        
        arm.SendDegreesSignal(90,0,90);
        Vector3.Distance(new Vector3(2, 1, 0), arm.EndPoint).Should().BeLessThan(tolerance);
    }
    
    [Fact]
    public void TestSolveForwardWithRotation3()
    {
        //Assemble
        var arm = ArmBuilder.CreateNew()
            .AddSegment(1f)
            .AddSegment(1f)
            .AddSegment(1f)
            .Build();
        //var output = new List<Vector3>();
        
        for (int i = 0; i < 32; i++)
        {
            arm.SendStepSignal(1,0,0);    
        }
        
        //ASSERT
        var distance = Vector3.Distance(new Vector3(3, 0, 0), arm.EndPoint);
        //handlePlots(output);
        distance.Should().BeLessThan(tolerance,because:"Arm Endpoint was {0}",arm.EndPoint);
    }

    private void handlePlots(IList<Vector3> output)
    {
        ScottPlot.Plot myPlot = new();
        double[] dataX = output.Select(x=>Convert.ToDouble(x.X)).ToArray();
        double[] dataY = output.Select(x=>Convert.ToDouble(x.Y)).ToArray();
        double[] dataZ = output.Select(x=>Convert.ToDouble(x.Z)).ToArray();
        var x =myPlot.Add.Signal(dataX);
        var y = myPlot.Add.Signal(dataY);
        var z = myPlot.Add.Signal(dataZ);
        x.Label = "X";
        y.Label = "Y";
        z.Label = "Z";
        myPlot.SavePng("scatter-plot.png",600,400);

    }
}