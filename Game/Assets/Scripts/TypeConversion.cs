using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPos
{
    public Vector3 value;

    public static implicit operator Vector3(SuperPos me)
    {
        return me.value;
    }

    public SuperPos(Vector3 vec)
    {
        value = vec;
    }

    public SuperPos(Vector2 vec)
    {
        value = vec;
    }


    public SuperPos(Transform trans)
    {
        value = trans.position;
    }

    public SuperPos(GameObject obj)
    {
        value = obj.transform.position;
    }

    public SuperPos(MonoBehaviour behav)
    {
        value = behav.gameObject.transform.position;
    }

    public static implicit operator SuperPos(Vector3 me)
    {
        return new SuperPos(me);
    }

    public static implicit operator SuperPos(Vector2 me)
    {
        return new SuperPos(me);
    }

    public static implicit operator SuperPos(Transform me)
    {
        return new SuperPos(me);
    }

    public static implicit operator SuperPos(GameObject me)
    {
        return new SuperPos(me);
    }

    public static implicit operator SuperPos(MonoBehaviour me)
    {
        return new SuperPos(me);
    }
}

public class SuperPos2D
{
    public Vector2 value;

    public static implicit operator Vector2(SuperPos2D me)
    {
        return me.value;
    }

    public SuperPos2D(Vector3 vec)
    {
        value = vec;
    }

    public SuperPos2D(Vector2 vec)
    {
        value = vec;
    }


    public SuperPos2D(Transform trans)
    {
        value = trans.position;
    }

    public SuperPos2D(GameObject obj)
    {
        value = obj.transform.position;
    }

    public SuperPos2D(MonoBehaviour behav)
    {
        value = behav.gameObject.transform.position;
    }

    public static implicit operator SuperPos2D(Vector3 me)
    {
        return new SuperPos2D(me);
    }

    public static implicit operator SuperPos2D(Vector2 me)
    {
        return new SuperPos2D(me);
    }

    public static implicit operator SuperPos2D(Transform me)
    {
        return new SuperPos2D(me);
    }

    public static implicit operator SuperPos2D(GameObject me)
    {
        return new SuperPos2D(me);
    }

    public static implicit operator SuperPos2D(MonoBehaviour me)
    {
        return new SuperPos2D(me);
    }
}
