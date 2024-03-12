using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Dice : MonoBehaviour
{
    public bool isMoving = true;
    public int currentSide = 0;

    public Texture[] sideTextures;

    public List<GameObject> sideObjects = new List<GameObject>();
    public int[] diceValues = { 1, 2, 3, 4, 5, 6 };

    public bool skipFX = false;

    public bool mouseState = false;

    public int diceCollisionAmount = 0;

    private DiceProps myDiceProps;

    private Vector3[] sides =
    {
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    private Rigidbody rigidBody;


    private DiceManager diceManager;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckThrow();
    }

    public void AddProps(DiceProps diceProps)
    {
        diceManager = GameObject.Find("DiceManager").GetComponent<DiceManager>();

        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValues[i] = i + diceManager.baseVal;
            SetText(i, diceValues[i].ToString());
        }

        myDiceProps = diceProps;

        for(int i = 0; i < diceProps.sides.Count; i++)
        {
            sideObjects[i].GetComponent<TextMeshPro>().text = "";
            Transform graphicsTransform = sideObjects[i].transform.Find("graphic");
            GameObject graphicsObject = graphicsTransform.gameObject;
            SpriteRenderer spriteRenderer = graphicsObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

            Sprite sprite = Resources.Load<Sprite>(diceProps.sides[i].ToString());

            if (diceProps.sides[i] == DiceProps.Side.None)
            {
                sideObjects[i].GetComponent<TextMeshPro>().text = diceValues[i].ToString();
            }
            else if (diceProps.sides[i] == DiceProps.Side.AddOne)
            {
                sideObjects[i].GetComponent<TextMeshPro>().text = diceValues[i].ToString() + "+1";
                diceValues[i] += 1;
            }
            else if (diceProps.sides[i] == DiceProps.Side.Area)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }
            else if (diceProps.sides[i] == DiceProps.Side.AddDice)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }
            else if (diceProps.sides[i] == DiceProps.Side.Pull)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }
            else if (diceProps.sides[i] == DiceProps.Side.Touch)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }
            else if (diceProps.sides[i] == DiceProps.Side.Lucky)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }
            else if (diceProps.sides[i] == DiceProps.Side.Multiply)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = sprite;
            }

        }
    }

    public void SetText(int id, string text)
    {
        sideObjects[id].GetComponent<TextMeshPro>().text = "";
        Transform graphicsTransform = sideObjects[id].transform.Find("graphic");
        GameObject graphicsObject = graphicsTransform.gameObject;
        SpriteRenderer spriteRenderer = graphicsObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1.0f,1.0f,1.0f,0.7f);
        
        //spriteRenderer.enabled = false;

        sideObjects[id].GetComponent<TextMeshPro>().text = text;

    }

    void CheckThrow()
    {
        if (!isMoving) return;

        //add slight delay so dice dont stop when they are still tilted
        if (rigidBody.velocity == Vector3.zero && rigidBody.angularVelocity == Vector3.zero)
        {
            StartCoroutine(CheckStillMoving());
        }
    }

    IEnumerator CheckStillMoving()
    {
        yield return new WaitForSeconds(0.1f); 

        if (rigidBody.velocity == Vector3.zero && rigidBody.angularVelocity == Vector3.zero) isMoving = false;

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

    private void OnMouseOver()
    {
        mouseState = true;
    }

    private void OnMouseExit()
    {
        mouseState = false; 
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObj = collision.gameObject;

        if (collisionObj.GetComponent<Dice>() != null){

            if (myDiceProps == null) return;

            diceCollisionAmount++;

            for (int i = 0; i < myDiceProps.sides.Count; i++)
            {
                if (myDiceProps.sides[i] == DiceProps.Side.Touch)
                {
                    diceValues[i] += diceCollisionAmount;
                    SetText(i,"+" + diceCollisionAmount.ToString());
                }
            }
        }
    }


}
