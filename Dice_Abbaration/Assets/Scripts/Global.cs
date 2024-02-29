using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{

    public static Vector3 Random3(Vector2 range)
    {
        return new Vector3(
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y)
        );
    }
}
