using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public SpriteRenderer graphic;

    private bool mouseState = false;

    Vector3 baseScale = new Vector3(1.5f, 1.5f, 1.5f);
    Vector3 growScale = new Vector3(1.8f, 1.8f, 1.8f);

    public float growSpeed = 25.0f;

    public GameObject graphicObj;

    public int cost = 100;

    private DiceEditor diceEditor;

    public DiceEditor.Modifier modifier;

    public 

    // Start is called before the first frame update
    void Start()
    {
        diceEditor = GameObject.Find("DiceEditor").GetComponent<DiceEditor>();
    }

    void Update()
    {

        if (mouseState && Global.sparePoints >= cost)
        {
            graphicObj.transform.localScale = Vector3.Lerp(graphicObj.transform.localScale, growScale, Time.deltaTime * growSpeed);
            graphic.color = Color.white;

            if (Input.GetMouseButtonDown(0))
            {
                Global.sparePoints -= cost;
                GameObject.Find("CurrencyPoints").GetComponent<TextMeshPro>().text = Global.sparePoints.ToString();

                if(modifier == DiceEditor.Modifier.Randomize)
                {
                    diceEditor.Randomize();
                }
                else
                {
                    diceEditor.modifier = modifier;
                }
            }
        }
        else
        {
            graphicObj.transform.localScale = Vector3.Lerp(graphicObj.transform.localScale, baseScale, Time.deltaTime * growSpeed);
            graphic.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
        }

    }

    private void OnMouseEnter()
    {
        mouseState = true;
    }

    private void OnMouseExit()
    {
        mouseState = false;
    }
}
