using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayZoneChecker : MonoBehaviour {
    public float min;
    public float gray;
    public bool inTheZone = false;

    public bool InZone(float f)
    {
        if(inTheZone)
        {
            if(f <= min + gray)
            {
                return true;
            }
            else
            {
                inTheZone = false;
                return false;
            }
        }
        else
        {
            if(f <= min)
            {
                inTheZone = true;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
