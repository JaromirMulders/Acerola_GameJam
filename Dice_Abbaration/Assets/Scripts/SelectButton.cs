using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject graphic;
    public TextMeshPro textMeshPro;
    public DiceEditor diceEditor;

    public bool editFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = "Select!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !editFlag)
        {
            diceEditor.EditDice();

            editFlag = true;
        }
    }
}
