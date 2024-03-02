using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

public class DiceManager : MonoBehaviour
{
    public GameObject dice;
    public float throwForce = 10.0f;
    public float rotForce = 5.0f;
    public int throwAmount = 3;
    public float throwSpeed = 0.5f;
    public float moveSpeed = 1.0f;

    public List<GameObject> allDice = new List<GameObject>();
    private List<Vector3> dicePositions = new List<Vector3>();
    private List<Dice> allDiceScripts = new List<Dice>();

    private bool SetCollectFlag = false;

    private Vector3 baseScale = new Vector3(50.0f, 50.0f, 50.0f);

    private Vector3[] diceAngles =
    {
        new Vector3(0.0f, 0.0f, 0.0f), //1
        new Vector3(270.0f, 0.0f, 0.0f),//2
        new Vector3(0.0f, 0.0f, 90.0f), //3
        new Vector3(0.0f, 0.0f, 270.0f), //4
        new Vector3(90.0f, 0.0f, 0.0f), //5
        new Vector3(180.0f, 180.0f, 0.0f) //6
    };

    public enum GameState
    {
        Throw,
        Wait,
        Collect,
        Select
    }

    public GameState gameState = GameState.Throw;

    void Start()
    {

    }

    void Update()
    {


        if(gameState == GameState.Throw)
        {

        }
        else if(gameState == GameState.Wait)
        {
            WaitForMovement();
        }
        else if (gameState == GameState.Collect)
        {
            CollectDice();
        
        }else if (gameState == GameState.Select)
        {
            SelectDice();
        }
    }


    public void ThrowDice()
    {
        StartCoroutine(SpawnDice(onThrowComplete));
    }

    private void WaitForMovement()
    {
        //check if there are still dice rolling
        for(int i = 0; i < allDiceScripts.Count; i++)
        {
            if (allDiceScripts[i].isMoving) return;
        }

        if(!SetCollectFlag) StartCoroutine(SetCollect());
    }

    private IEnumerator SetCollect()
    {
        SetCollectFlag = true;

        yield return new WaitForSeconds(1.0f);

        List<int> diceValues = new List<int>();

        for (int i = 0; i < allDice.Count; i++)
        {
            allDice[i].GetComponent<Rigidbody>().isKinematic = true;

            allDiceScripts[i].CheckSide();
            diceValues.Add(allDiceScripts[i].currentSide);

            dicePositions.Add(new Vector3(((float)(i % 8) / 7.0f * 2.0f - 1.0f) * 8.0f, 0.0f, 0.0f));
        }

        // Sort allDice and allDiceScripts based on diceValues
        var sortedDice = allDice.OrderBy(dice => allDiceScripts[allDice.IndexOf(dice)].currentSide).ToList();
        var sortedDiceScripts = sortedDice.Select(dice => dice.GetComponent<Dice>()).ToList();

        allDice = sortedDice;
        allDiceScripts = sortedDiceScripts;

        gameState = GameState.Collect;

    }

    private void CollectDice()
    {
        float t = Time.deltaTime * moveSpeed;

        bool diceMoveing = false;


        //check if there are still dice rolling
        for (int i = 0; i < allDice.Count; i++)
        {
            allDice[i].transform.position = Vector3.Lerp(allDice[i].transform.position, dicePositions[i], t);
            allDice[i].transform.rotation = Quaternion.Slerp(allDice[i].transform.rotation, Quaternion.Euler(diceAngles[allDiceScripts[i].currentSide -1]),t * 2.0f);

            if(Vector3.Distance(allDice[i].transform.position, dicePositions[i]) > 0.01){
                diceMoveing = true;
            }
        }

        if (!diceMoveing) gameState = GameState.Select; 

    }

    private void SelectDice()
    {
        float t = Time.deltaTime * 25.0f;

        for (int i = 0; i < allDice.Count; i++)
        {
            Vector3 growScale = baseScale;
            if (allDiceScripts[i].mouseState) growScale = Vector3.Scale(growScale, new Vector3(1.2f, 1.2f, 1.2f));
            
            allDice[i].transform.localScale = Vector3.Lerp(allDice[i].transform.localScale, growScale, t);
            
        }
    }

    IEnumerator SpawnDice(Action onComplete)
    {
        for (int i = 0; i < throwAmount; i++)
        {
            Quaternion startRotation = Quaternion.Euler(Global.Random3(new Vector2(0.0f, 360.0f)));

            GameObject newDice = Instantiate(dice, new Vector3(-7.0f, 5.0f, 0.0f), startRotation);

            allDice.Add(newDice);
            allDiceScripts.Add(newDice.GetComponent<Dice>());

            newDice.transform.parent = transform;

            Rigidbody diceBody = newDice.GetComponent<Rigidbody>();

            Vector3 throwDir = new Vector3(UnityEngine.Random.Range(1.0f, 2.0f), -0.5f, UnityEngine.Random.Range(-1.0f, 1.0f));
            diceBody.AddForce(throwDir * throwForce, ForceMode.Impulse);
            Vector3 rotDir = Global.Random3(new Vector2(10.0f, 360f));
            diceBody.AddTorque(rotDir * rotForce, ForceMode.Impulse);


            yield return new WaitForSeconds(throwSpeed);
        }

        onComplete?.Invoke();

    }

    private void onThrowComplete()
    {
        gameState = GameState.Wait;
    }




}
