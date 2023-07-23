using System.Numerics;
using System.Security.AccessControl;
using NibbleKinematics;

public class Segment
{
    public Vector3 StartPoint { get; set; }
    public Vector3 EndPoint { get; private set; }
    public float Length { get; }

    public int Steps { get; private set; }
    
    public float StepDegrees { get; }
    internal Quaternion RotationQuaternion { get; set; } // Added rotation property

    public Segment(float length, Quaternion rotationQuaternion, float stepDegrees = 2.8f, int steps = 0) // Added rotation parameter
    {
        Length = length;
        RotationQuaternion = rotationQuaternion;
        StepDegrees = stepDegrees;
        Steps = steps;
    }
    
    public Vector3 RotationDegrees
    {
        get => Rotation.ToDegreesVector();
        set => Rotation = value.ToRadiansVector();
    }

    public Vector3 Rotation
    {
        get => RotationQuaternion.ToPitchYawhRoll();
        set => RotationQuaternion = Quaternion.CreateFromYawPitchRoll(value.Y,value.X,value.Z);
    }
    
    public static Vector3 QuaternionToYawPitchRoll(Quaternion q)
{
    Vector3 output;

    // roll (x-axis rotation)
    float sinr_cosp = +2.0f * (q.W * q.X + q.Y * q.Z);
    float cosr_cosp = +1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
    output.Z = (float)Math.Atan2(sinr_cosp, cosr_cosp);

    // pitch (y-axis rotation)
    float sinp = +2.0f * (q.W * q.Y - q.Z * q.X);
    if (Math.Abs(sinp) >= 1)
        output.X = (float)Math.CopySign(Math.PI / 2, sinp); // use 90 degrees if out of range
    else
        output.X = (float)Math.Asin(sinp);

    // yaw (z-axis rotation)
    float siny_cosp = +2.0f * (q.W * q.Z + q.X * q.Y);
    float cosy_cosp = +1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z);  
    output.Y = (float)Math.Atan2(siny_cosp, cosy_cosp);

    return output;
}
    
    public void Rotate(Vector3 axis, float radians)
    {
        RotationQuaternion = Quaternion.CreateFromAxisAngle(axis, radians);
    }
    
    public void RotateDegrees(Vector3 axis, float degrees)
    {
        RotationQuaternion = Quaternion.CreateFromAxisAngle(axis, MathExtensions.DegToRad(degrees));
    }

    public void RotateX(float radians) => Rotate(Vector3.UnitX, radians);
    public void RotateY(float radians) => Rotate(Vector3.UnitY, radians);
    public void RotateZ(float radians) => Rotate(Vector3.UnitZ, radians);
    
    public void RotateXDegrees(float degrees) => Rotate(Vector3.UnitX, MathExtensions.DegToRad(degrees));
    public void RotateYDegrees(float degrees) => Rotate(Vector3.UnitY, MathExtensions.DegToRad(degrees));
    public void RotateZDegrees(float degrees) => Rotate(Vector3.UnitZ, MathExtensions.DegToRad(degrees));
    
 

    
    
    public void CalculateEndpoint()
    {
        Vector3 direction = Vector3.Transform(new Vector3(0, -Length, 0), RotationQuaternion);
        EndPoint = StartPoint + Length * direction;
    }
    
   
}
