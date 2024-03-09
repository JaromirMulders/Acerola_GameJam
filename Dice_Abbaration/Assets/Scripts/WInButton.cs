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
            button.color = Color.white;
            winState = true;
        }
    }

    private void OnMouseOver()
    {
        if (!winState) return;

        if(Input.GetMouseButtonDown(0))
        {
            diceManager.WinState();
        }
        
    }
}
