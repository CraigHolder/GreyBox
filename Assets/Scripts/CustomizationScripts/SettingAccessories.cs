﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingAccessories : MonoBehaviour
{
    public GameObject player;

    public GameObject[] bodyItems;
    private int bCurrentItem;
    public GameObject[] hatItems;
    private int hCurrentItem;
    public GameObject[] maskItems;
    private int mCurrentItem;

    private int i_BlueColour;
    private int i_RedColour;
    private int i_Skin;

    bool b_Accessoriezed = false;

    //TEMP VARIABLES:
    public int i_PlayerTeam = 0;

    // Start is called before the first frame update
    void Start()
    {
        //USE THIS TO SAVE CHARACTER PRESETS BETWEEN GAMES
        bCurrentItem = PlayerPrefs.GetInt("Body");
        hCurrentItem = PlayerPrefs.GetInt("Hat");
        mCurrentItem = PlayerPrefs.GetInt("Mask");

        i_BlueColour = PlayerPrefs.GetInt("BlueColour");
        i_RedColour = PlayerPrefs.GetInt("RedColour");
        i_Skin = PlayerPrefs.GetInt("Skin");
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_Accessoriezed)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        Activate(0, bCurrentItem, bodyItems);
                        break;
                    case 1:
                        Activate(1, hCurrentItem, hatItems);
                        break;
                    case 2:
                        Activate(2, mCurrentItem, maskItems);
                        break;
                }
            }
            b_Accessoriezed = true;
        }
    }



    public void Activate(int part, int currentItem, GameObject[] items)
    {
        ApplyColour();
        for (int c = 0; c < items.Length; c++)
        {
            if (c == currentItem)
            {
                //If accessories are unactive when this is used, doesnt work for some reason??
                items[c].SetActive(true);
                //items[c].transform.rotation = player.transform.rotation;
            }
            else
            {
                items[c].SetActive(false);
            }
        }
    }

    public void ApplyColour()
    {
        switch (i_Skin)
        {
            case 0:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/FerretBaseColour");
                break;
            case 1:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/FerretAlbino");
                break;
            case 2:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/FerretDarkBrown");
                break;
        }

        string s_TeamColour = "";

        if (i_PlayerTeam == 0)
        {
            switch (i_RedColour)
            {
                case 0:
                    s_TeamColour = "Red";
                    break;
                case 1:
                    s_TeamColour = "Orange";
                    break;
                case 2:
                    s_TeamColour = "Yellow";
                    break;
            }
        }
        else
        {
            switch (i_BlueColour)
            {
                case 0:
                    s_TeamColour = "Blue";
                    break;
                case 1:
                    s_TeamColour = "Green";
                    break;
                case 2:
                    s_TeamColour = "Purple";
                    break;
            }
        }

        /*****************************/
        /*       VERY IMPORTANT      */
        /*****************************/

       // //bodyItems;
       bodyItems[0].GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Cape" + s_TeamColour);
       // bodyItems[1].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Dragon_Wings" + s_TeamColour);
       // //maskItems;
       // maskItems[0].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mask" + s_TeamColour);
       // maskItems[1].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Goggles" + s_TeamColour);
       // //hatItems;
       // hatItems[0].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Tophat" + s_TeamColour);
       hatItems[1].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ArcherHat" + s_TeamColour);

        //TopHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Tophat" + s_TeamColour);
        //DragonWings.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Dragon_Wings" + s_TeamColour);
        //Cape.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Cape" + s_TeamColour);
        //Mask.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mask" + s_TeamColour);
        //ArcherHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ArcherHat" + s_TeamColour);
    }
}
