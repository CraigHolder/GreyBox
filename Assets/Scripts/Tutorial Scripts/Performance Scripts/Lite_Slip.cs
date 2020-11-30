using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lite_Slip : MonoBehaviour
{
    public HazardSpill SpillWeight;

    void OnTriggerEnter(Collider collision)
    {
        SpillWeight.OnTriggerEnter(collision);
    }

    void OnTriggerExit(Collider collision)
    {
        SpillWeight.OnTriggerExit(collision);
    }
}
