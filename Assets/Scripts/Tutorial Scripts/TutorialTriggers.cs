using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public int trigger_id;

    public GameObject w_img;
    public GameObject a_img;
    public GameObject s_img;
    public GameObject d_img;

    public GameObject e_img;

    public GameObject space_img;
    public GameObject Lshift_img;

    public GameObject mouse_img;

    public AudioSource Start_Audio; //1
    public AudioSource Jump_Audio; //2
    public AudioSource Sprint_Audio; //3
    public AudioSource Bounce_Audio; //4
    public AudioSource Speakers_Audio; //5
    public AudioSource Slip_Audio; //6
    public AudioSource Grab_Audio; //7

    public AudioSource Currentinstructions;

    void Start()
    {
        w_img.SetActive(false);
        a_img.SetActive(false);
        s_img.SetActive(false);
        d_img.SetActive(false);

        e_img.SetActive(false);

        space_img.SetActive(false);
        Lshift_img.SetActive(false);

        mouse_img.SetActive(false);
    }

    public void ReceiveId(int id)
    {
        trigger_id = id;

        switch (id)
        {
            case 1:
                Currentinstructions = Start_Audio;
                break;
            case 2:
                Currentinstructions = Jump_Audio;
                break;
            case 3:
                Currentinstructions = Sprint_Audio;
                break;
            case 4:
                Currentinstructions = Bounce_Audio;
                break;
            case 5:
                Currentinstructions = Speakers_Audio;
                break;
            case 6:
                Currentinstructions = Slip_Audio;
                break;
            case 7:
                Currentinstructions = Grab_Audio;
                break;
            case 8:
                Currentinstructions = Grab_Audio;
                break;
        }
    }

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            Currentinstructions.Play();

            switch (trigger_id)
            {
                case 1:
                    w_img.SetActive(true);
                    a_img.SetActive(true);
                    s_img.SetActive(true);
                    d_img.SetActive(true);
                    mouse_img.SetActive(true);

                   
                    break;

                case 2:
                    space_img.SetActive(true);
                   
                    break;

                case 3:
                    Lshift_img.SetActive(true);
                  
                    break;

                case 7:
                    e_img.SetActive(true);
                    break;
                case 8:
                    e_img.SetActive(true);
                    Currentinstructions.Stop();
                    break;
            }
        }
            
            
    }

    // Update is called once per frame
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Currentinstructions.Stop();

            switch (trigger_id)
            {
               

                case 1:
                    w_img.SetActive(false);
                    a_img.SetActive(false);
                    s_img.SetActive(false);
                    d_img.SetActive(false);
                    mouse_img.SetActive(false);
                    break;

                case 2:
                    space_img.SetActive(false);
                    //instructions.Play();
                    break;

                case 3:
                    Lshift_img.SetActive(false);
                    //instructions.Play();
                    break;

                case 7:
                    e_img.SetActive(false);
                    break;
                case 8:
                    e_img.SetActive(false);
                    break;
            }
        }
    }

   

}
