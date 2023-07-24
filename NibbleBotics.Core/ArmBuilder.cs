using System.Numerics;

namespace NibbleKinematics;

public class ArmBuilder
{
    public Vector3 Origin;
    public IList<Segment> Segments = new List<Segment>();
    internal Arm arm_ = new();
    public static ArmBuilder CreateNew(Vector3? origin = null) => new()
        {
            Origin = origin ?? Vector3.Zero
        };
}

public static class ArmBuilderExtensions
{
    public static ArmBuilder AddSegment(this ArmBuilder builder, float length, Segment.EVectorAxis defaultAxis = Segment.EVectorAxis.Z)
    {
        builder.Segments.Add(new Segment(builder.arm_, length,defaultAxis, Quaternion.Identity));
        return builder;
    }
    public static Arm Build(this ArmBuilder builder)
    {
        builder.Segments.First().StartPoint = builder.Origin;
        builder.arm_.Segments = builder.Segments;
        builder.arm_.SolveForward();
        return builder.arm_;
    }
}