using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Observer
{

    public enum EventType
    {
        Tutorial,
        PickupObject,
        Bounce,
        Push,
        Remote,
        Return,
        Steal,

        Sprint, //done
        Jump, //done
        Move, //done
        Tut_Return, //done
        Tut_Hazard, //done
        Tut_Bounce //done
    }
    virtual public void OnNotify(GameObject entity, EventType e_event) { }
    virtual public void OnTutorialNotify(GameObject entity, EventType e_event, TutorialQuestManager tut_Manager) { }
}

public class Subject
{
    public List<Observer> L_attachedobservers = new List<Observer>();

    public void AddObserver(Observer observer) 
    {
        L_attachedobservers.Add(observer);
    }
    public void SubObserver(Observer observer)
    {
        foreach (Observer observers in L_attachedobservers)
        {
            if (observers == observer)
            {
                L_attachedobservers.Remove(observer);
            }
        }
    }

    public void Notify(GameObject entity, Observer.EventType e_event)
    {
        for (int i = 0; i <= L_attachedobservers.Count - 1; i++)
        {
            L_attachedobservers[i].OnNotify(entity, e_event);
        }
    }

    public void TutorialNotify(GameObject entity, Observer.EventType e_event, TutorialQuestManager tut_Manager)
    {
        for (int i = 0; i <= L_attachedobservers.Count - 1; i++)
        {
            L_attachedobservers[i].OnTutorialNotify(entity, e_event, tut_Manager);
        }
    }


}

public class Achievments : Observer
{
    //Used for the tutorial;

    private bool hasMoved = false;
    private bool hasJumped = false;
    private bool hasSprinted = false;
    private bool hasBounced = false;
    private bool hasAvoided = false;
    private bool hasNested = false;

    AchievementHandler h_handler;
    override public void OnNotify(GameObject entity, EventType e_event) 
    { 
        switch (e_event)
        {
            case EventType.PickupObject:
                if(entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.PickupObject);
                }
                break;
            case EventType.Bounce:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.Bounce);
                }
                break;
            case EventType.Steal:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.Steal);
                }
                break;
            case EventType.Return:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.Return);
                }
                break;
            case EventType.Remote:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.Remote);
                }
                break;
            case EventType.Tutorial:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true)
                {
                    Unlock(EventType.Tutorial);
                }
                break;
            case EventType.Push:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == false)
                {
                    Unlock(EventType.Push);
                }
                break;
        }//End if switch

       

    }

    override public void OnTutorialNotify(GameObject entity, EventType e_event, TutorialQuestManager tut_Manager)
    {
        switch (e_event)
        {
            case EventType.Move:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasMoved == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Walk");

                    hasMoved = true;
                }
                break;
            case EventType.Jump:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasJumped == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Jump");

                    hasJumped = true;
                }
                break;

            case EventType.Sprint:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasSprinted == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Sprint");

                    hasSprinted = true;
                }
                break;

            case EventType.Tut_Bounce:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasBounced == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Bounce");

                    hasBounced = true;
                }
                break;

            case EventType.Tut_Hazard:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasAvoided == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Hazard");

                    hasAvoided = true;
                }
                break;

            case EventType.Tut_Return:
                if (entity.tag == "Player" && entity.GetComponent<PlayerMovement>().b_disableachieve == true && hasNested == false)
                {
                    //Debug.Log("Oh hey you made it");
                    tut_Manager.Recieve_Event("Nested");

                    hasNested = true;
                }
                break;

        }

    }


    void Unlock(EventType e_event)
    {
        Debug.Log(e_event);
        Debug.Log(PlayerPrefs.GetInt(e_event.ToString(), 0));
        PlayerPrefs.SetInt(e_event.ToString(), 1);
        Debug.Log(PlayerPrefs.GetInt(e_event.ToString(), 0));
    }

}
    


   