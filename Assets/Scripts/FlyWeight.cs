using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Flyweight", order = 1)]
public class FlyWeight : ScriptableObject
{
    public Speakers.SpeakerState e_speakerstate;
    public Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    public BounceObjCommand c_objbounce;

    //public void Awake()
    //{
    //    S_Notifier.AddObserver(achievmentobserver);
    //    e_speakerstate = Speakers.SpeakerState.Off;
    //}
    public void OnEnable()
    {
        c_objbounce = new BounceObjCommand();
        S_Notifier.AddObserver(achievmentobserver);
        e_speakerstate = Speakers.SpeakerState.Off;
    }
}
