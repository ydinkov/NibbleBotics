using System.Numerics;

namespace NibbleKinematics;

public class Arm
{
    public Segment[] Segments { get;}

    public Arm(Segment[] segments)
    {
        Segments = segments;
    }

    public Vector3 EndPoint => Segments.Last().EndPoint;
    
    public void SolveForward()
    {
        for (int i = 0; i < Segments.Length; i++)
        {
            if (i > 0)
            {
                Segments[i].StartPoint = Segments[i - 1].EndPoint;
                Segments[i].RotationQuaternion = Segments[i - 1].RotationQuaternion * Segments[i].RotationQuaternion;
            }

            Segments[i].CalculateEndpoint();
        }
    }
}