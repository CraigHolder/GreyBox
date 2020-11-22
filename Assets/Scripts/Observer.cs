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
        Steal
    }
    virtual public void OnNotify(GameObject entity, EventType e_event) { }
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


}

public class Achievments : Observer
{
    AchievementHandler h_handler;
    override public void OnNotify(GameObject entity, EventType e_event) 
    { 
        switch (e_event)
        {
            case EventType.PickupObject:
                if(entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.PickupObject);
                }
                break;
            case EventType.Bounce:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Bounce);
                }
                break;
            case EventType.Steal:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Steal);
                }
                break;
            case EventType.Return:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Return);
                }
                break;
            case EventType.Remote:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Remote);
                }
                break;
            case EventType.Tutorial:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Tutorial);
                }
                break;
            case EventType.Push:
                if (entity.tag == "Player" && entity.GetComponent<player_controller_behavior>().b_disableachieve == false)
                {
                    Unlock(EventType.Push);
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