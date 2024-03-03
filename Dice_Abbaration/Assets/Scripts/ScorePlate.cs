using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlate : MonoBehaviour
{
    private Vector3 baseScale;
    private Vector3 growScale;

    private bool mouseHover = false;

    public float growAmount = 1.1f;

    void Start()
    {
        baseScale = transform.localScale;
        growScale = Vector3.Scale(transform.localScale,new Vector3(growAmount, growAmount, growAmount));
    }

    void Update()
    {
        float t = 10.0f * Time.deltaTime;

        if(mouseHover)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, growScale, t);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, t);
        }

    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {

        }
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
    }

    private void OnMouseExit()
    {
        mouseHover = false; 
    }
}
