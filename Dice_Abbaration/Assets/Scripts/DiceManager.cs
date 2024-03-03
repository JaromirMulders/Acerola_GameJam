using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using Unity.VisualScripting;
using TMPro;

public class DiceManager : MonoBehaviour
{
    public GameObject dice;
    public float throwForce = 10.0f;
    public float rotForce = 5.0f;
    public int throwAmount = 3;
    public float throwSpeed = 0.5f;
    public float moveSpeed = 1.0f;
    public float diceDist = 1.0f;
    private int selectedSide = 0;

    public GameObject scoreNumber;

    public Scoring scoring;

    public List<GameObject> allDice = new List<GameObject>();
    private List<Vector3> dicePositions = new List<Vector3>();
    private List<Dice> allDiceScripts = new List<Dice>();

    public List<GameObject> allSlots = new List<GameObject>();
    private List<Slot> allSlotScripts = new List<Slot>();

    private bool throwFlag = false;
    private bool setCollectFlag = false;
    private bool setScoreFlag = false;
    private bool setClearFlag = false;

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
        Select,
        Score,
        Clear
    }

    public GameState gameState = GameState.Throw;

    void Start()
    {
        for(int i = 0;i < allSlots.Count;i++)
        {
            allSlotScripts.Add(allSlots[i].GetComponent<Slot>());
        }
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
        else if (gameState == GameState.Select)
        {
            SelectDice();
        }
        else if (gameState == GameState.Score)
        {
            Score();
        }
        else if(gameState == GameState.Clear)
        {
            Clear();
        }

    }


    public void ThrowDice()
    {
        if (gameState != GameState.Throw || throwFlag == true) return;

        StartCoroutine(SpawnDice(onThrowComplete));

        throwFlag = true;
    }

    private void WaitForMovement()
    {
        //check if there are still dice rolling
        for(int i = 0; i < allDiceScripts.Count; i++)
        {
            if (allDiceScripts[i].isMoving) return;
        }

        if(!setCollectFlag) StartCoroutine(SetCollect());
    }

    private IEnumerator SetCollect()
    {
        setCollectFlag = true;

        yield return new WaitForSeconds(1.0f);

        List<int> diceValues = new List<int>();

        for (int i = 0; i < allDice.Count; i++)
        {
            allDice[i].GetComponent<Rigidbody>().isKinematic = true;

            allDiceScripts[i].CheckSide();
            diceValues.Add(allDiceScripts[i].currentSide);

        }

        // Sort allDice and allDiceScripts based on diceValues
        var sortedDice = allDice.OrderBy(dice => allDiceScripts[allDice.IndexOf(dice)].currentSide).ToList();
        var sortedDiceScripts = sortedDice.Select(dice => dice.GetComponent<Dice>()).ToList();

        allDice = sortedDice;
        allDiceScripts = sortedDiceScripts;

        float baseRowPos = allSlots[0].transform.position.x;
        float row = baseRowPos;

        for (int i = 0; i < allDice.Count; i++)
        {
            if (allDiceScripts[i].currentSide - allDiceScripts[Math.Max(i - 1, 0)].currentSide != 0)
            {
                row = baseRowPos;
            }
            row += diceDist;


            dicePositions.Add(new Vector3(row, 0.0f, -allDiceScripts[i].currentSide * diceDist + 7.0f));
        }

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

        selectedSide = 0;

        Vector3 growScale = Vector3.Scale(baseScale, new Vector3(1.2f, 1.2f, 1.2f));
        Vector3 diceScale = Vector3.one;

        for (int i = 0; i < allDice.Count; i++)
        {
            if (allDiceScripts[i].mouseState) selectedSide = allDiceScripts[i].currentSide;
        }



        for (int i = 0; i < allDice.Count; i++)
        {
            if (allDiceScripts[i].currentSide == selectedSide && allSlotScripts[allDiceScripts[i].currentSide - 1].isSlotUsed == false )
            {
                diceScale = growScale;

                if (Input.GetMouseButtonDown(0))
                {
                    gameState = GameState.Score;
                }
            }
            else
            {
                diceScale = baseScale;
            }

            allDice[i].transform.localScale = Vector3.Lerp(allDice[i].transform.localScale, diceScale, t);
        }
    }

    private void Score()
    {
        if(!setScoreFlag) StartCoroutine(SlotDice());

        for (int i = 0; i < allDice.Count; i++)
        {
            if (allDiceScripts[i].currentSide == selectedSide)
            {
                if (Vector3.Distance(allDice[i].transform.localScale, Vector3.zero) > 0.01f) return;
            }
        }
        gameState = GameState.Clear;

    }

    private void Clear()
    {
        Vector3 targetPos = new Vector3(8.0f, 0.0f, 8.0f);

        bool destroyFlag = true;

        for (int i = 0; i < allDice.Count; i++)
        {
            if (Vector3.Distance(allDice[i].transform.position, targetPos) > 0.01) destroyFlag = false;
        }

        if (destroyFlag){
            Reset();
            return;
        }

        if (setClearFlag) return;

        allSlotScripts[selectedSide - 1].setSlotState(true);

        for(int i = 0; i < allDice.Count; i++)
        {
            StartCoroutine(MoveToPosition(allDice[i], targetPos, moveSpeed));
        }

        setClearFlag = true;
    }

    private void Reset()
    {
        StopAllCoroutines();

        allDiceScripts.Clear();
        dicePositions.Clear();

        for (int i = 0; i < allDice.Count; i++)
        {
            GameObject dice = allDice[i];
            allDice.Remove(dice);
            Destroy(dice);
        }

        allDice.Clear();

        setCollectFlag = false;
        setScoreFlag = false;
        setClearFlag = false;
        throwFlag = false; 

        selectedSide = 0;

        gameState = GameState.Throw;
    }

    private void AddScore()
    {
        Vector3 slotPos = allSlots[selectedSide - 1].transform.position;
        slotPos.y = 5.0f;
        slotPos.x += -1.5f;

        GameObject number = Instantiate(scoreNumber, slotPos, Quaternion.identity);
        ScoreNumber numberScript = number.GetComponent<ScoreNumber>();
        numberScript.textMeshPro.text = "+" + selectedSide.ToString();

        scoring.AddScore(selectedSide);
    }

    IEnumerator SlotDice()
    {
        setScoreFlag = true;

        for (int i = 0; i < allDice.Count; i++)
        {
            if (allDiceScripts[i].currentSide == selectedSide)
            {
                StartCoroutine(MoveToPosition(allDice[i], allSlots[selectedSide-1].transform.position, moveSpeed));
                
                yield return new WaitForSeconds(throwSpeed);
            }
        }
    }

    IEnumerator MoveToPosition(GameObject gameObject, Vector3 targetPosition, float moveSpeed)
    {
        float randomDir = UnityEngine.Random.Range(0.5f, 1.0f) * moveSpeed*5.0f * Time.deltaTime;
        randomDir *= (float)(UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1);

        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.01f)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y + randomDir, gameObject.transform.eulerAngles.z);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }
        
        if(gameState == GameState.Score)
        {
            AddScore();
        }

        if(gameObject != null) yield return StartCoroutine(ScaleToTarget(gameObject, Vector3.zero, moveSpeed));

    }

    IEnumerator ScaleToTarget(GameObject gameObject, Vector3 targetScale, float scaleSpeed)
    {
        while (Vector3.Distance(gameObject.transform.localScale, targetScale) > 0.01f)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
            yield return null;
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
