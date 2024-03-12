using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject textObject;
    private TextMeshPro textMeshPro;

    public List<ScorePlate> scorePlates = new List<ScorePlate>();

    void Start()
    {
        textMeshPro = textObject.GetComponent<TextMeshPro>();
        textMeshPro.text = Global.score.ToString();

        SetScoringGoals(6);
    }

    public void NewStage()
    {
        Global.score = 0;
        textMeshPro.text = Global.score.ToString();

        Global.stage += 1;
        SetScoringGoals(6);
    }

    public void AddScore(int amount)
    {
        Global.score += amount;
        textMeshPro.text = Global.score.ToString();
    }

    public void SetCanScore(bool state)
    {
        for (int i = 0; i < scorePlates.Count; i++)
        {
            scorePlates[i].canScore = state;
        }
    }

    public void SetScoringGoals(int baseScore)
    {
        for(int i = 0; i < scorePlates.Count; i++)
        {
            scorePlates[i].SetScorePlates(baseScore * Global.stage + Random.Range(-3,3 + i));
        }
    }
}
