using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowButton : MonoBehaviour
{
    public DiceManager diceManager;
    public GameObject graphic;
    public TextMeshPro textMeshPro;
    public GameObject upgradeScreen;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = "THROW!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && !upgradeScreen.activeSelf)
        {
            diceManager.ThrowDice();
        }
    }
}
