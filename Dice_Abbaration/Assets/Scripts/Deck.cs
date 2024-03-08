using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public int startSize = 5;

    public List<DiceProps> diceDeck = new List<DiceProps>();

    void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            DiceProps diceProps = ScriptableObject.CreateInstance<DiceProps>();
            diceProps.name = "Dice_" + i.ToString();
            diceDeck.Add(diceProps);
        }
    }

    void Update()
    {
        
    }
}
