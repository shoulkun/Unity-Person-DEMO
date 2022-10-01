using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NormalRandom
{
    // u均值， d方差
    public static float Rand(int seed, float u, float d)
    {
        float u1, u2, z, x;
        if (d <= 0)
        {
            return u;
        }
        Random.InitState((int)(seed + u));
        u1 = Random.value;
        Random.InitState((int)(seed * u));
        u2 = Random.value;
        //(new Random(GetRandomSeed())).NextDouble();
        z = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        x = u + d * z;
        return x;
    }
}
