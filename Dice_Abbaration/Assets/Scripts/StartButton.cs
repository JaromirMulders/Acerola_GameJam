using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshPro textMeshPro;
    private GameManager gameManager;

    public enum ButtonType { 
        Start,
        Settings,
        Exit,
    }

    public ButtonType buttonType;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver()
    {

        if(Input.GetMouseButtonDown(0) && gameManager.stageState == GameManager.StageState.StartScreen)
        {
            if (buttonType == ButtonType.Start)
            {
                gameManager.SetStageState(GameManager.StageState.Game);
                Debug.Log("test");
            }
        }

    }




}
