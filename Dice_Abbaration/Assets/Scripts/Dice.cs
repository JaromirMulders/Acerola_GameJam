using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool isThrown = true;
    public int currentSide = 0;

    public Material material = null;
    public Texture[] sideTextures = null;

    private Vector3[] sides =
    {
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    private Rigidbody rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckThrow();
        CheckSide();

    }

    void CheckThrow()
    {
        if (!isThrown) return;

        if (rigidBody.velocity == Vector3.zero && rigidBody.angularVelocity == Vector3.zero)
        {
            isThrown = false;
        }
        
    }

    //from: https://forum.unity.com/threads/dice-which-face-is-up.10443/
    void CheckSide()
    {
        if(isThrown) return;

        float maxY = float.NegativeInfinity;
        int result = -1;

        for (int i = 0; i < 3; i++)
        {
            // Transform the vector to world-space:
            Vector3 worldSpace = transform.TransformDirection(sides[i]);
            if (worldSpace.y > maxY)
            {
                result = i + 1; // index 0 is 1
                maxY = worldSpace.y;
            }
            if (-worldSpace.y > maxY)
            { // also check opposite side
                result = 6 - i; // sum of opposite sides = 7
                maxY = -worldSpace.y;
            }
        }

        currentSide = result;
        Debug.Log(currentSide);
    }



}
