using UnityEngine;

public static class CameraUtils
{
    public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
    {
        var delta = eulerAngle;

        if (delta.x > 180) delta.x -= 360;
        else if (delta.x < -180) delta.x += 360;

        if (delta.y > 180) delta.y -= 360;
        else if (delta.y < -180) delta.y += 360;

        if (delta.z > 180) delta.z -= 360;
        else if (delta.z < -180) delta.z += 360;

        return new Vector3(delta.x, delta.y, delta.z);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }
}