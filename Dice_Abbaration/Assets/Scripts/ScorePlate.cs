using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePlate : MonoBehaviour
{
    private Vector3 basePosition;
    private Vector3 baseScale;
    private Vector3 growScale;

    private bool mouseHover = false;

    public float growAmount = 1.1f;

    public SpriteRenderer background;
    public SpriteRenderer boon;
    public TextMeshPro textMeshPro;

    private int scoreToReach = 0;

    public bool canScore = true;

    public GameObject gameManager;
    public GameManager manager;
    public DiceEditor diceEditor;

    public DiceProps.Side reward;

    void Start()
    {
        basePosition = transform.position;
        baseScale = transform.localScale;
        growScale = Vector3.Scale(transform.localScale,new Vector3(growAmount, growAmount, growAmount));

        gameManager = GameObject.Find("GameManager");
        manager = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        float t = 10.0f * Time.deltaTime;

        if(Global.score >= scoreToReach && canScore)
        {
            background.color = Color.white;
        }
        else
        {
            background.color = Color.grey;
            return;

        }

        if (mouseHover)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, growScale, t);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, t);
        }

    }
    

    public void SetScorePlates(int amount)
    {
        DiceProps.Side reward = Global.GetRandomEnum<DiceProps.Side>();

        while(reward == DiceProps.Side.None)
        {
            reward = Global.GetRandomEnum<DiceProps.Side>();
        }

        Sprite sprite = Resources.Load<Sprite>(reward.ToString());
        boon.sprite = sprite;

        diceEditor.editMode = reward;
        
        scoreToReach = amount;
        textMeshPro.text = amount.ToString();
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && Global.score >= scoreToReach && canScore)
        {
            manager.SetStageState(GameManager.StageState.EditDice);
        }
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
    }

    private void OnMouseExit()
    {
        mouseHover = false; 
    }
}
