using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathG
{
    public static float AngleClampNegative(float angle, float min, float max)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);

        return angle;
    }
    public static float AngleCorrectionNegative(float angle)
    {
        if (angle > 180)
            return angle - 360;
        else
            return angle;
    }
}
