using UnityEngine;

public static class VectorUtility
{
    public static Vector3 ShearTo2D(this Vector3 v)
    {
        v.y = 0;
        return v;
    }
}
