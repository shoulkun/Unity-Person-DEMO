using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceondOrderDynamics : MonoBehaviour
{
    private Vector3 xp;
    private Vector3 y, yd;
    private float _w, _z, _d, k1, k2, k3;

    public void init(float f, float z, float r, Vector3 x0)
    {
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }
    public void changePropties(float f, float z, float r)
    {
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);
    }
    // Update is called once per frame
    public Vector3 ComputeSOD(float T, Vector3 x, Vector3 xd)
    {
        if(xd == Vector3.zero)
        {
            xd = (x - xp) / T;
            xp = x;
        }
        float k2_stable = Mathf.Max(k2, 1.1f * (T*T/4 + T*k1/2));
        y = y + T*yd;
        yd = yd + T * (x + k3*xd - y - k1*yd) /k2_stable;
        return y;
    }
}
