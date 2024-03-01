using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool isMoving = true;
    public int currentSide = 0;

    public Texture[] sideTextures;

    public List<GameObject> sideObjects = new List<GameObject>();
    private List<Renderer> sideRenderer = new List<Renderer>();

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

        for(int i = 0; i < sideObjects.Count; i++)
        {
            sideRenderer.Add(sideObjects[i].GetComponent<Renderer>());
            sideRenderer[i].material.mainTexture = sideTextures[i];
        }
    }

    void Update()
    {
        CheckThrow();
    }

    void CheckThrow()
    {
        if (!isMoving) return;

        if (rigidBody.velocity == Vector3.zero && rigidBody.angularVelocity == Vector3.zero)
        {
            isMoving = false;
        }
        
    }



    //from: https://forum.unity.com/threads/dice-which-face-is-up.10443/
    public void CheckSide()
    {
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
    }



}
