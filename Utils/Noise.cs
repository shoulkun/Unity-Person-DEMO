using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GeneratetNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] nosieMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float PerlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                nosieMap[x,y] = PerlinValue;
            }
        }
        return nosieMap;
    }
}
