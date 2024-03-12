using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceEditor : MonoBehaviour
{
    public GameManager manager;
    public DiceManager diceManager;
    private BoxCollider boxCollider;
    public GameObject dice;
    public Deck deck;
    public GameObject editSlot;
    public SpriteRenderer editIcon;
    public SelectButton selectButton;

    public List<GameObject> allDice = new List<GameObject>();
    private List<Vector3> dicePositions = new List<Vector3>();
    public List<Dice> allDiceScripts = new List<Dice>();

    private bool diceThrown = false;

    public Arrow arrowLeft;
    public Arrow arrowRight;

    private Vector3 baseScale = new Vector3(50.0f, 50.0f, 50.0f);
    private List<Vector3> randomRot = new List<Vector3>();

    public int selectedDice = 0;
    private int selectedSide = 0;

    public DiceProps.Side editMode;

    public Material diceDistort;

    public enum Modifier
    {
        None,
        AllSides,
        Randomize
    }

    public Modifier modifier = Modifier.None;

    private Vector3[] diceAngles =
{
        new Vector3(270.0f, 0.0f, 0.0f), //1
        new Vector3(0.0f, 180.0f, 180.0f),//2
        new Vector3(270.0f, 0.0f, 90.0f), //3
        new Vector3(270.0f, 270.0f, 0.0f), //4
        new Vector3(0.0f, 0.0f, 0.0f), //5
        new Vector3(270.0f, 180.0f, 0.0f) //6
    };

    void Start()
    {

        diceDistort.SetFloat("_Distort", 0.0f);

        editMode = DiceProps.Side.None;

        boxCollider = GetComponent<BoxCollider>();
    }


    void Update()
    {
        if(manager.stageState != GameManager.StageState.EditDice)
        {
            boxCollider.enabled = false;
            diceThrown = false;
            return;
        }
            
        editIcon.sprite = Resources.Load<Sprite>(editMode.ToString());

        boxCollider.enabled = true;

        SetDiceSide();

        float t  = Time.deltaTime * 10.0f;

        Vector3 growScale = Vector3.Scale(baseScale, new Vector3(1.2f, 1.2f, 1.2f));
        Vector3 diceScale = baseScale;

        if (diceThrown)
        {
            for(int i  = 0; i < allDice.Count; i++)
            {
                allDice[i].transform.position = Vector3.Lerp(allDice[i].transform.position, dicePositions[i], t);

                if (selectedDice != 0 && i == (selectedDice - 1))
                {
                    Vector3 rotTo = diceAngles[selectedSide];
                    allDice[i].transform.rotation = Quaternion.Lerp(allDice[i].transform.rotation, Quaternion.Euler(rotTo), t);
                }
                else
                {
                    if(!allDiceScripts[i].mouseState) allDice[i].transform.eulerAngles += randomRot[i] * 50.0f * Time.deltaTime;

                    allDice[i].layer = LayerMask.NameToLayer("Default");
                    SetLayerRecursively(allDice[i].transform, LayerMask.NameToLayer("Default"));
                }

                if (allDiceScripts[i].mouseState)
                {

                    allDice[i].transform.localScale = Vector3.Lerp(allDice[i].transform.localScale, growScale, t);
                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedDice = i + 1;
                        RecalculatePositions();
                        dicePositions[i] = editSlot.transform.position;

                        allDice[i].layer = LayerMask.NameToLayer("FX");
                        SetLayerRecursively(allDice[i].transform, LayerMask.NameToLayer("FX"));
                    }
                }
                else
                {
                    allDice[i].transform.localScale = Vector3.Lerp(allDice[i].transform.localScale, diceScale, t);
                }


            }
        }

    }

    public void Randomize()
    {
        DiceProps.Side prevMode = editMode;

        editMode = Global.GetRandomEnum<DiceProps.Side>();

        if (editMode == DiceProps.Side.None || editMode == prevMode) Randomize();
    }

    void SetLayerRecursively(Transform obj, int layer)
    {
        obj.gameObject.layer = layer;

        foreach (Transform child in obj)
        {
            SetLayerRecursively(child, layer);
        }
    }

    public void EditDice()
    {
        StartCoroutine(EditDiceFx());

    }

    IEnumerator EditDiceFx()
    {
        float distort = 0.0f;
        float duration = 0.5f;
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            distort = Mathf.Lerp(0.0f, 0.25f, Global.SmoothStep(0.0f,1.0f,t * t));

            diceDistort.SetFloat("_Distort", distort);

            yield return null;
        }

        diceDistort.SetFloat("_Distort", 1.0f);

        Deck deckScript = deck.GetComponent<Deck>();

        if(modifier == Modifier.None)
        {
            deckScript.diceDeck[selectedDice - 1].sides[selectedSide] = editMode;
        }
        else if(modifier == Modifier.AllSides)
        {
            for (int i = 0; i < 6; i++)
            {
                deckScript.diceDeck[selectedDice - 1].sides[i] = editMode;
            }
        }


        allDice[selectedDice - 1].GetComponent<Dice>().AddProps(deck.diceDeck[selectedDice - 1]);

        startTime = Time.time;
        endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            distort = Mathf.Lerp(0.25f, 0.0f, Global.SmoothStep(0.0f,1.0f,t  * t));

            diceDistort.SetFloat("_Distort", distort);

            yield return null;
        }

        diceDistort.SetFloat("_Distort", 0.0f);

        yield return new WaitForSeconds(0.5f);

        manager.SetStageState(GameManager.StageState.Game);

        yield return null;
    }



    public void SetDiceSide()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (arrowLeft.mouseHover)
        {
            selectedSide -= 1;
        }
        else if (arrowRight.mouseHover)
        {
            selectedSide += 1;
        }

        selectedSide = Global.Wrap(selectedSide, 0, 6);
    }

    public void Reset()
    {
        modifier = Modifier.None;

        for(int i = 0; i < allDice.Count; i++)
        {
            Destroy(allDice[i]);
        }

        selectButton.editFlag = false;

        allDice.Clear();
        allDiceScripts.Clear();
        dicePositions.Clear();
        randomRot.Clear();
        

        selectedDice = 0;
        selectedSide = 0;

        diceThrown = false;
    }

    public void ThrowDice()
    {
        diceThrown = false;

        for (int i = 0; i < deck.diceDeck.Count; i++)
        {
            float del = (float)i/(deck.diceDeck.Count-1)  * 2.0f - 1.0f;
            del *= 3.0f;
            GameObject newDice = Instantiate(dice, new Vector3(del, 17.0f, 17.5f), Quaternion.identity);
            Dice diceScript = newDice.GetComponent<Dice>();

            newDice.transform.parent = transform;

            allDice.Add(newDice);
            
            allDiceScripts.Add(diceScript);
            diceScript.AddProps(deck.diceDeck[i]);

            Rigidbody rb = newDice.GetComponent<Rigidbody>();
            rb.AddTorque(Global.Random3(new Vector2(10.0f, 360f)), ForceMode.Impulse);
            rb.AddForce(new Vector3(Random.Range(-5.0f,5.0f),0.0f, Random.Range(-5.0f, 5.0f)), ForceMode.Impulse);
        }

        StartCoroutine(GoToSelectState());
    }

    private Vector3 GetPosition(int i)
    {

        float phase = (float)(i + 1) / (allDice.Count) * 2.0f - 1.0f;

        phase -= 1.0f / allDice.Count;

        return new Vector3(phase * 10.0f, 8.0f, 17.5f);
    }

    private void RecalculatePositions()
    {
        for (int i = 0; i < dicePositions.Count; i++)
        {
            dicePositions[i] = GetPosition(i);
        }
    }


    IEnumerator GoToSelectState()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < allDice.Count; i++)
        {

            dicePositions.Add(GetPosition(i));
            allDice[i].GetComponent<Rigidbody>().isKinematic = true;
            float randomFloat = (Random.value < 0.5f) ? 1.0f : -1.0f;
            randomRot.Add(Vector3.Scale(Global.Random3(new Vector2(0.5f, 1.0f)), new Vector3(randomFloat, randomFloat, randomFloat) ));
        }

        diceThrown = true;

    }
}





