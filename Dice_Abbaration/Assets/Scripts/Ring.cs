using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90.0f,0.0f,0.0f);
    }


}
