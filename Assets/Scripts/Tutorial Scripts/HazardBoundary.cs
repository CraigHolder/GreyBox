using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBoundary : MonoBehaviour
{
    public TutorialQuestManager t_manager;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    // Start is called before the first frame update
    void Start()
    {
        S_Notifier.AddObserver(achievmentobserver);
    }

   void OnTriggerEnter(Collider collision)
   {
        if(collision.gameObject.tag == "Player")
        {
            if (t_manager)
            {
                S_Notifier.TutorialNotify(collision.gameObject, Observer.EventType.Tut_Hazard, t_manager);
            }
        }
   }
}
