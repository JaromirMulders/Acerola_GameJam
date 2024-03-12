using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject cameraObj;
    public DiceEditor diceEditor;
    public DiceManager diceManager;
    public Scoring scoring;
    public AddDice addDice;
    public TextMeshPro stageCounter;
    public GameObject upgradeScreen;

    public enum StageState
    {
        StartScreen,
        Game,
        Pause,
        EditDice
    }

    public StageState stageState;

    void Start()
    {
        stageState = StageState.StartScreen;
        if (stageState == StageState.StartScreen) Physics.gravity = new Vector3(0, 9.81f, 0); // Reverse gravity

    }


    public void SetStageState(StageState state)
    {
        stageState = state;

        if (stageState == StageState.StartScreen)
        {
            Global.stage = 0;
            GameObject.Find("GameOver").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Win").transform.GetChild(0).gameObject.SetActive(false);

            StartCoroutine(RotateTo(cameraObj, cameraObj.transform.eulerAngles, new Vector3(-180.0f, 0.0f, 0.0f), 2.0f));
            Physics.gravity = new Vector3(0, 9.81f, 0); // Reverse gravity
        }
        if (stageState == StageState.Game)
        {
            ResetBefore();
            Action reset = () => ResetEditDice();
            StartCoroutine(RotateTo(cameraObj, cameraObj.transform.eulerAngles, new Vector3(90.0f,0.0f,0.0f), 2.0f, reset));
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        else if(stageState == StageState.Pause)
        {

        }
        else if(stageState == StageState.EditDice)
        {
            StartCoroutine(RotateTo(cameraObj,cameraObj.transform.eulerAngles,Vector3.zero, 2.0f));
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }

    public void ResetBefore()
    {
        scoring.NewStage();
        addDice.UpdateRequirments();
        diceManager.NewStage();

        stageCounter.text = "Stage: " + Global.stage.ToString();

        int stageMod = Global.stage % 3;
        if(stageMod == 0)
        {
            upgradeScreen.SetActive(true);
        }

    }

    public void ResetEditDice()
    {
        diceEditor.Reset();
    }

    public void GameOver()
    {
        Global.score = 0;
        SetStageState(StageState.StartScreen);
    }

    IEnumerator RotateTo(GameObject gameObject, Vector3 basePos, Vector3 target, float duration, Action action = null)
    {
        float t = 0f;
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(target);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            gameObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Global.SmoothStep(0.0f,1.0f,t));
            yield return null;
        }

        if(stageState == StageState.EditDice)
        {
            diceEditor.ThrowDice();
        }

        // Ensure exact rotation at the end
        gameObject.transform.rotation = targetRotation;

        action?.Invoke();
    }
}
