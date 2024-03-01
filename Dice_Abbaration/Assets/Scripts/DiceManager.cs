using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DiceManager : MonoBehaviour
{
    public GameObject dice;
    public float throwForce = 10.0f;
    public float rotForce = 5.0f;
    public int throwAmount = 3;
    public float throwSpeed = 0.5f;
    public float moveSpeed = 1.0f;

    public List<GameObject> allDice = new List<GameObject>();
    public List<Vector3> dicePositions = new List<Vector3>();
    public List<Dice> allDiceScripts = new List<Dice>();

    private bool SetCollectFlag = false;

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
        Collect
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

        for (int i = 0; i < allDice.Count; i++)
        {
            allDice[i].GetComponent<Rigidbody>().isKinematic = true;

            allDiceScripts[i].CheckSide();

            dicePositions.Add(new Vector3(((float)(i % 8) / 7.0f * 2.0f - 1.0f) * 8.0f, 0.0f, 0.0f));
        }

        gameState = GameState.Collect;

    }

    private void CollectDice()
    {
        float t = Time.deltaTime * moveSpeed;

        //check if there are still dice rolling
        for (int i = 0; i < allDice.Count; i++)
        {
            allDice[i].transform.position = Vector3.Lerp(allDice[i].transform.position, dicePositions[i], t);
            allDice[i].transform.rotation = Quaternion.Slerp(allDice[i].transform.rotation, Quaternion.Euler(diceAngles[allDiceScripts[i].currentSide -1]),t * 5.0f);
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
