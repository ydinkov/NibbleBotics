using System.Numerics;

namespace NibbleKinematics;

public class Arm
{
    public IList<Segment> Segments { get; set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="signal">Each signal index corresponds to a segment. Positive numbers mean a step up, negative numbers mean a step down, 0 means no movements</param>
    public void SendStepSignal(params short[]signal)
    {
        for (int i = 0; i < signal.Length; i++)
        {
            Segments[i].Steps += (signal[i]);
        }

        SolveForward();
    }
    public void SendDegreesSignal(params float[]signal)
    {
        for (int i = 0; i < signal.Length; i++)
        {
            Segments[i].RotationDegrees = signal[i];
        }

        SolveForward();
    }

    

    public void NotifyFault(EFaults fault)
    {
        //TODO
    }

    public Vector3 EndPoint => Segments.Last().EndPoint;
    
    public void SolveForward()
    {
        for (int i = 0; i < Segments.Count; i++)
        {
            if (i > 0)
            {
                Segments[i].StartPoint = Segments[i - 1].EndPoint;
                Segments[i].RotationQuaternion = Segments[i - 1].RotationQuaternion * Segments[i].RotationQuaternion;
            }
        }
    }
}