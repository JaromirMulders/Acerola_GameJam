using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreNumber : MonoBehaviour
{
    public GameObject scoreNumber;
    public TextMeshPro textMeshPro;

    private Vector3 pos = Vector3.zero;

    public float animSpeed = 10.0f;

    private float alpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = scoreNumber.GetComponent<TextMeshPro>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.z += Time.deltaTime * animSpeed;
        alpha -= Time.deltaTime * animSpeed * 0.2f;

        transform.position = pos;
        textMeshPro.color = new Color(1.0f, 1.0f, 1.0f, alpha);



        if(alpha < 0.0f)
        {
            Destroy(gameObject);
        }

    }
}
