using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject dice;
    public float throwForce = 10.0f;
    public float rotForce = 5.0f;
    public int throwAmount = 3;

    public List<GameObject> allDice;


    void Start()
    {

    }

    void Update()
    {

    }

    public void ThrowDice()
    {
        for (int i = 0; i < throwAmount; i++)
        {
            Quaternion startRotation = Quaternion.Euler(Global.Random3(new Vector2(0.0f, 360.0f)));

            GameObject newDice = Instantiate(dice, new Vector3(-7.0f, 5.0f, 0.0f), startRotation);
            newDice.transform.parent = transform;

            allDice.Add(newDice);

            Rigidbody diceBody = newDice.GetComponent<Rigidbody>();

            Vector3 throwDir = new Vector3(Random.Range(1.0f, 2.0f), -0.5f, Random.Range(-1.0f, 1.0f));
            diceBody.AddForce(throwDir * throwForce, ForceMode.Impulse);
            Vector3 rotDir = Global.Random3(new Vector2(10.0f, 360f));
            diceBody.AddTorque(rotDir * rotForce, ForceMode.Impulse);
        }

    }



}
