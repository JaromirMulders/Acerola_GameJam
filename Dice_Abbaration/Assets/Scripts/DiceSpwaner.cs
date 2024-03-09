using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpwaner : MonoBehaviour
{
    public GameObject dice;

    public float spwanInterval = 1.0f;
    public float lifeSpan = 5.0f;
    private float timer = 0.0f;
    public GameManager gameManager;

    void Start()
    {
    }

    void Update()
    {
        if (gameManager.stageState != GameManager.StageState.StartScreen) return;

        timer += Time.deltaTime;

        if (timer >= spwanInterval)
        {
            GameObject spawnDice = Instantiate(dice, transform.position, Quaternion.identity);

            spawnDice.transform.parent = transform;

            Rigidbody rb = spawnDice.GetComponent<Rigidbody>();

            rb.AddForce(Global.Random3(new Vector2(-10.0f, 10.0f)), ForceMode.Impulse);
            rb.AddTorque(Global.Random3(new Vector2(-10.0f, 10.0f)), ForceMode.Impulse);

            Destroy(spawnDice, lifeSpan);  

            timer = 0f;
        }
    }
}

