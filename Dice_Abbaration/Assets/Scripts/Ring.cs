using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{

    public Vector3 baseScale;

    public float radSize = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
    }



    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90.0f,0.0f,0.0f);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, radSize);
    }

}
