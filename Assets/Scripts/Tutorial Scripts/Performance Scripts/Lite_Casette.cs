using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lite_Casette : MonoBehaviour
{
    public int id;
    public TutorialTriggers tut_trigger;


    void OnTriggerEnter(Collider other)
    {
        tut_trigger.ReceiveId(id);

        tut_trigger.OnTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        tut_trigger.ReceiveId(id);

        tut_trigger.OnTriggerExit(other);
    }
}