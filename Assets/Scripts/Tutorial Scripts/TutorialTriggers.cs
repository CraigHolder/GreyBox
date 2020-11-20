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

    public AudioSource instructions;

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

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            instructions.Play();

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
            }
        }
            
            
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.Stop();

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
            }
        }
    }
}
