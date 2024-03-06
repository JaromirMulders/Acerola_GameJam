using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject cameraObj;
    public DiceEditor diceEditor;
    public DiceManager diceManager;
    public Scoring scoring;

    public enum StageState
    {
        Game,
        Pause,
        EditDice
    }

    public StageState stageState;

    void Start()
    {
        stageState = StageState.Game;
    }


    public void SetStageState(StageState state)
    {
        stageState = state;

        if(stageState == StageState.Game)
        {
            Action reset = () => ResetEditDice();
            StartCoroutine(RotateTo(cameraObj, cameraObj.transform.eulerAngles, new Vector3(90.0f,0.0f,0.0f), 2.0f, reset));
        }
        else if(stageState == StageState.Pause)
        {

        }
        else if(stageState == StageState.EditDice)
        {
            StartCoroutine(RotateTo(cameraObj,cameraObj.transform.eulerAngles,Vector3.zero, 2.0f));
        }
    }

    public void ResetEditDice()
    {
        diceEditor.Reset();
        diceManager.NewStage();
        scoring.NewStage();
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
