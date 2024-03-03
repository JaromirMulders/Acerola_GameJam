using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject textObject;
    private TextMeshPro textMeshPro;

    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = textObject.GetComponent<TextMeshPro>();
        textMeshPro.text = score.ToString();
    }


    public void AddScore(int amount)
    {
        score += amount;
        textMeshPro.text = score.ToString();
    }
}
