using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int score = 0;
    public static int winScore = 250;
    public static int stage = 0;
    public static int sparePoints = 0;

    public static Vector3 Random3(Vector2 range)
    {
        return new Vector3(
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y)
        );
    }

    const float HALF_PI = Mathf.PI * 0.5f;

    public static float ElasticEaseInOut(float value)
    {
        float result = 0f;

        if (value < 0.5f)
        {
            result = 0.5f * Mathf.Sin(13f * HALF_PI * 2f * value) * Mathf.Pow(2f, 10f * (2f * value - 1f));
        }
        else
        {
            result = 0.5f * (Mathf.Sin(-13f * HALF_PI * (2f * value - 1f + 1f)) * Mathf.Pow(2f, -10f * (2f * value - 1f)) + 2f);
        }

        return result;
    }

    public static float SmoothStep(float edge0, float edge1, float x)
    {
        float t;
        t = Mathf.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return t * t * (3.0f - 2.0f * t);
    }

    public static int Wrap(int x, int x_min, int x_max)
    {
        return (((x - x_min) % (x_max - x_min)) + (x_max - x_min)) % (x_max - x_min) + x_min;
    }

    //from: https://discussions.unity.com/t/enum-choose-random/18261
    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }


}
