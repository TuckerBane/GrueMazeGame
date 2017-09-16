using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GameMath
{

    public static float DistanceBetween(SuperPos pos, SuperPos pos2)
    {
        return (pos.value - pos2.value).magnitude;
    }

    public static float DistanceBetweenSquared(SuperPos pos, SuperPos pos2)
    {
        return (pos.value - pos2.value).sqrMagnitude;
    }

    public static Vector3 UnitVectorBetween(SuperPos pos, SuperPos pos2)
    {
        return (pos2.value - pos.value).normalized;
    }

    public static void PointObjAt2D(GameObject obj, SuperPos pos)
    {
        DistanceBetween(obj, obj.transform);

        Vector3 dir = UnitVectorBetween(obj, pos);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion goalDirection = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = goalDirection;
    }

    public static void Turn2D(GameObject obj, float angle)
    {

    }

}
