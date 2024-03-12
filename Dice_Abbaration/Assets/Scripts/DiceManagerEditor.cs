using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiceManager))]
public class DiceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DiceManager diceManager = (DiceManager)target;

        if (GUILayout.Button("Throw"))
        {
            if (Application.isPlaying) diceManager.ThrowDice();
        }
    }
}