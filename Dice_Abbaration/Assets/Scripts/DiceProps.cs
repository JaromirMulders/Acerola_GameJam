using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice/Dice")]
public class DiceProps : ScriptableObject
{
    public enum Side
    {
        None,
        Add,
        Subtract
    }

    public Side side_1;
    public Side side_2;
    public Side side_3;
    public Side side_4;
    public Side side_5;
    public Side side_6;


}