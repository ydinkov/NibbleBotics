using System.Numerics;
using NibbleKinematics;

public class Segment
{
    private readonly EVectorAxis _defaultAxis;

    public enum EVectorAxis
    {
        X,Y,Z
    }

    public Vector3 StartPoint { get; set; }

    public Vector3 EndPoint =>
        StartPoint + (Vector3.Transform(new Vector3(0, -Length, 0), RotationQuaternion));
    public Arm Parent { get; }
    public float Length { get; }
    
    public Vector3 DefaultAxis { get; }

    private int steps_;
    public int Steps
    {
        get => steps_;
        set
        {
            if (0 <= value && value <= MaxSteps)
            {
                steps_ = value;
                RotationDegrees = (steps_*StepDegrees);
            }
            else
            {
                if (value <0)
                    Parent.NotifyFault(EFaults.SEGMENT_MIN_STEP_LIMIT);
                else if(value > MaxSteps)
                    Parent.NotifyFault(EFaults.SEGMENT_MAX_STEP_LIMIT);
            }
        }
    }

    public int MaxSteps { get; }

    public float StepDegrees { get; }
    
    internal Quaternion RotationQuaternion { get; set; } // Added rotation property

    public Segment(
        Arm parent,
        float length,
        EVectorAxis defaultAxis, 
        Quaternion rotationQuaternion,
        float stepDegrees = 2.8f,
        int steps = 0,
        int maxSteps = 180)
    {
        _defaultAxis = defaultAxis;
        Parent = parent;
        Length = length;
        RotationQuaternion = rotationQuaternion;
        StepDegrees = stepDegrees;
        Steps = steps;
        MaxSteps = maxSteps;
        DefaultAxis = defaultAxis switch
        {
            EVectorAxis.X => Vector3.UnitX,
            EVectorAxis.Y => Vector3.UnitY,
            EVectorAxis.Z => Vector3.UnitZ
        };
    }


    public float RotationDegrees
    {
        get => MathExtensions.RadToDeg(Rotation);
        set => Rotation = MathExtensions.DegToRad(value);
    }

    private float Rotation
    {
        get => (RotationQuaternion.ToPitchYawhRoll() * DefaultAxis).Max();
        set => RotationQuaternion = Quaternion.CreateFromYawPitchRoll(DefaultAxis.Y*value, DefaultAxis.X*value,DefaultAxis.Z*value);
    }
    
    
}
