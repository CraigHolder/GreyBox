using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    public Text t_scoretext;
    public int i_teamscore = 0;

    public FlyWeight fly_shareddata;

    // Start is called before the first frame update
    void Start()
    {
        
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
            fly_shareddata.S_Notifier.Notify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Return);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Score>() != null)
        {
            
            i_teamscore -= collision.gameObject.GetComponent<Score>().i_score;
            t_scoretext.text = "Score: " + i_teamscore.ToString();
            fly_shareddata.S_Notifier.Notify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Steal);
        }
    }
}
