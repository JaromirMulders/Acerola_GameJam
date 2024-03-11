using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddDice : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public SpriteRenderer addDiceButton;
    public Deck deck;
    public DiceManager diceManager;

    public int requiredDice = 0;
    public int requiredAmount = 2;

    public bool addDiceAvailible = false;

    public bool isUsed = false;

    public Slot slot;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRequirments();
    }

    void Update()
    {
        if (addDiceAvailible && isUsed == false)
        {
            addDiceButton.color = Color.white;
        }
        else
        {
            addDiceButton.color = Color.grey;
        }
    }

    public void UpdateRequirments()
    {
        isUsed = false;
        slot.setSlotState(false);

        requiredDice = Random.Range(0, 6);
        textMeshPro.text = requiredAmount.ToString() + "X" + (requiredDice + 1).ToString();
    }

    public void SetAvailible(bool state)
    {
        addDiceAvailible = state;

    }

    private void OnMouseOver()
    {
        if (addDiceAvailible == false || isUsed == true) return;

        if (Input.GetMouseButton(0))
        {
            deck.AddDice();
            isUsed = true;
            diceManager.AddDiceRoutine();
            requiredAmount++;
        }
    }

}
