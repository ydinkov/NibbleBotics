using System.Numerics;

namespace NibbleKinematics;

public class ArmBuilder
{
    public Vector3 Origin;
    public IList<Segment> Segments = new List<Segment>();
    public static ArmBuilder CreateNew(Vector3? origin = null)
    {
        var builder = new ArmBuilder();
        builder.Origin = origin ?? Vector3.Zero;
        return new ArmBuilder();
    }
}

public static class ArmBuilderExtensions
{
    public static ArmBuilder AddSegment(this ArmBuilder builder, float length)
    {
        builder.Segments.Add(new Segment(length, Quaternion.Identity));
        return builder;
    }
    public static Arm Build(this ArmBuilder builder)
    {
        builder.Segments.First().StartPoint = builder.Origin;
        return new Arm(builder.Segments.ToArray());
    }
}