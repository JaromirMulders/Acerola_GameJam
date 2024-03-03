using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject slotUsed;

    public bool isSlotUsed = false;

    void Start()
    {
        
    }

    public void setSlotState(bool state)
    {
        isSlotUsed = state;

        slotUsed.SetActive(state);
    }


}
