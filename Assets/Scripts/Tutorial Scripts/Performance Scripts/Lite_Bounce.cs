using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lite_Bounce : MonoBehaviour
{
    public Bounce bounce_weight;

    void OnTriggerStay(Collider collision)
    {
        bounce_weight.OnTriggerStay(collision);
    }

    void OnTriggerExit(Collider collision)
    {
        bounce_weight.OnTriggerExit(collision);
    }
}
