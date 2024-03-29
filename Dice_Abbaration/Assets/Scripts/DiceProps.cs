using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice/Dice")]
public class DiceProps : ScriptableObject
{
    public enum Side
    {
        None,
        AddOne,
        Area,
        AddDice,
        Pull,
        Touch,
        Lucky,
        Multiply
    }

    public List<Side> sides = new List<Side> { Side.None , Side.None , Side.None  , Side.None  , Side.None  , Side.None };

}