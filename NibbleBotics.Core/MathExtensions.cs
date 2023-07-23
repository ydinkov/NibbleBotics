using System.Numerics;

namespace NibbleKinematics;

public static class MathExtensions
{
    public static Quaternion QuaternionFromDegrees(float x, float y, float z) => 
        Quaternion.CreateFromYawPitchRoll(DegToRad(x), DegToRad(y), DegToRad(z));

    public static float RadToDeg(float radians) => (180f / Convert.ToSingle(Math.PI) ) * radians;
    public static float DegToRad(float degrees) => degrees * (Convert.ToSingle(Math.PI) / 180f);
    
    public static Vector3 ToPitchYawhRoll(this Quaternion q)
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

    public static Vector3 ToDegreesVector(this Vector3 radians) => 
        new(RadToDeg(radians.X), RadToDeg(radians.Y), RadToDeg(radians.Z));
    public static Vector3 ToRadiansVector(this Vector3 degrees) => 
        new(DegToRad(degrees.X), DegToRad(degrees.Y), DegToRad(degrees.Z));
}