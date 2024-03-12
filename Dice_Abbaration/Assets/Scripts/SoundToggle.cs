using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggle : MonoBehaviour
{
    public bool soundOn = true;

    public Sprite on;
    public Sprite off;

    private SpriteRenderer spriteRenderer;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            soundOn = !soundOn;
            if (soundOn)
            {
                spriteRenderer.sprite = on;
                audioSource.mute = false;
            }
            else
            {
                spriteRenderer.sprite = off;
                audioSource.mute = true;
            }

        }
    }
}
