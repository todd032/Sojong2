using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    public static float GetAngle(Vector2 _from, Vector2 _to)
    {
        float angle = Vector2.Angle(_from, _to);
        if(Vector3.Cross(_to, _from).z > 0f)
        {
            angle = 360f - angle;
        }
        return angle;
    }


}


