using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WInButton : MonoBehaviour
{
    public SpriteRenderer button;

    public bool winState = false;
    public TextMeshPro winAmount;
    public DiceManager diceManager;
    public GameObject winButton;

    private Vector3 baseScale = Vector3.one;

    private bool mouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        winAmount.text = Global.winScore.ToString() + "!";
    }

    void Update()
    {
        if(Global.score < Global.winScore)
        {
            button.color = Color.gray;
            winState = false;
        }
        else
        {
            if (!mouseOver)
            {
                button.color = Color.white;

            }
            else
            {
                button.color = new Color(1.0f,1.0f,1.0f,0.7f);
            }
            winState = true;

            float t = Mathf.Sin(Time.time * 2.0f) * 0.05f;
            winButton.transform.localScale = baseScale + new Vector3(t, t, t);
        }
    }

    private void OnMouseOver()
    {
        if (!winState) return;

        if (Input.GetMouseButtonDown(0))
        {
            diceManager.WinState();
        }   
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;

    }
}
