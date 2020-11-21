using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    public Text t_scoretext;
    public int i_teamscore = 0;

    public TutorialQuestManager t_manager;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    // Start is called before the first frame update
    void Start()
    {
        S_Notifier.AddObserver(achievmentobserver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Score>() != null)
        {
            
            i_teamscore += collision.gameObject.GetComponent<Score>().i_score;
            t_scoretext.text = "Score: " + i_teamscore.ToString();
            S_Notifier.Notify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Return);

            if (t_manager)
            {
                print("Nested!!");
                S_Notifier.TutorialNotify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Tut_Return, t_manager);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Score>() != null)
        {
            
            i_teamscore -= collision.gameObject.GetComponent<Score>().i_score;
            t_scoretext.text = "Score: " + i_teamscore.ToString();
            S_Notifier.Notify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Steal);
        }
    }
}
