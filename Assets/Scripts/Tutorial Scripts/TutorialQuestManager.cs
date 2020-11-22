using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialQuestManager : MonoBehaviour
{
    public Plugin_Manager p_handler;

    public player_controller_behavior player;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    public GameObject l2Walk;
    public GameObject l2Jump;
    public GameObject l2Sprint;
    public GameObject l2Bounce;
    public GameObject l2Hazard;
    public GameObject l2Nest;

    public GameObject FinishedText;

    private TextMeshProUGUI l2Walk_text;
    private TextMeshProUGUI l2Jump_text;
    private TextMeshProUGUI l2Sprint_text;
    private TextMeshProUGUI l2Bounce_text;
    private TextMeshProUGUI l2Hazard_text;
    private TextMeshProUGUI l2Nest_text;

    private bool walkDone = false;
    private bool jumpDone = false; 
    private bool sprintDone = false;
    private bool bounceDone = false;
    private bool hazardDone = false;
    private bool nestDone = false;

    private bool TutorialCompleted = false;



    // Start is called before the first frame update
    void Start()
    {
        S_Notifier.AddObserver(achievmentobserver);

        l2Walk_text = (TextMeshProUGUI)l2Walk.GetComponent<TextMeshProUGUI>();
        l2Jump_text = (TextMeshProUGUI)l2Jump.GetComponent<TextMeshProUGUI>();
        l2Sprint_text = (TextMeshProUGUI)l2Sprint.GetComponent<TextMeshProUGUI>();
        l2Bounce_text = (TextMeshProUGUI)l2Bounce.GetComponent<TextMeshProUGUI>();
        l2Hazard_text = (TextMeshProUGUI)l2Hazard.GetComponent<TextMeshProUGUI>();
        l2Nest_text = (TextMeshProUGUI)l2Nest.GetComponent<TextMeshProUGUI>();

        FinishedText.SetActive(false);
        //l2Nest_text.fontStyle = FontStyles.Strikethrough;
    }

    // Update is called once per frame
    void Update()
    {

        float input_x = Input.GetAxis("Horizontal");
        float input_y = Input.GetAxis("Vertical");

        if(!walkDone && !Mathf.Approximately(input_x, 0.0f) || !Mathf.Approximately(input_y, 0.0f))
        {
            Recieve_Event("Walk");
        }

        if (!jumpDone && Input.GetButton("Jump"))
        {
            Recieve_Event("Jump");
        }

        if (!sprintDone && Input.GetButton("Sprint"))
        {
            Recieve_Event("Sprint");
        }

        if(walkDone && jumpDone && sprintDone && bounceDone && hazardDone && nestDone && !TutorialCompleted)
        {

            FinishedText.SetActive(true);
            S_Notifier.Notify(GameObject.FindGameObjectWithTag("Player"), Observer.EventType.Tutorial);

            if (p_handler)
            {
                p_handler.createFile();
            }

            TutorialCompleted = true;
        }
    }

    public void Recieve_Event(string str)
    {
        switch (str)
        {
            case "Walk":
                l2Walk_text.fontStyle = FontStyles.Strikethrough;
                walkDone = true;
                break;
            case "Jump":
                l2Jump_text.fontStyle = FontStyles.Strikethrough;
                jumpDone = true;
                break;
            case "Sprint":
                l2Sprint_text.fontStyle = FontStyles.Strikethrough;
                sprintDone = true;
                break;
            case "Bounce":
                l2Bounce_text.fontStyle = FontStyles.Strikethrough;
                bounceDone = true;
                break;
            case "Hazard":
                l2Hazard_text.fontStyle = FontStyles.Strikethrough;
                hazardDone = true;
                break;
            case "Nested":
                l2Nest_text.fontStyle = FontStyles.Strikethrough;
                nestDone = true;
                break;
        }
    }
}
