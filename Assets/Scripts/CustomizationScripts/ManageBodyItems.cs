using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//USE PLAYER PREFS: RESEARCH ON DOCUMENTATION

public class ManageBodyItems : MonoBehaviour
{
    public GameObject player;

    public GameObject[] bodyItems;
    public GameObject[] hatItems;
    public GameObject[] maskItems;

    private int bCurrentItem = 0;
    private int hCurrentItem = 0;
    private int mCurrentItem = 0;
    private bool b_Left = false;
    private bool b_Right = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("Body")) {
            PlayerPrefs.SetInt("Body", bCurrentItem);
        }
        if (PlayerPrefs.HasKey("Hat"))
        {
            PlayerPrefs.SetInt("Hat", hCurrentItem);
        }
        if (PlayerPrefs.HasKey("Mask"))
        {
            PlayerPrefs.SetInt("Mask", mCurrentItem);
        }
    }

    void Update()
    {
        if (b_Left)
        {
            player.transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
        if (b_Right)
        {
            player.transform.Rotate(0, -100 * Time.deltaTime, 0);
        }



        for (int i = 0; i < 3; i++) {
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
    }

    public void Activate(int part, int currentItem, GameObject[] items)
    {
        for (int c = 0; c < items.Length; c++)
        {
            if (c == currentItem)
            {
                items[c].SetActive(true);
                //items[c].transform.rotation = player.transform.rotation;
            }
            else
            {
                items[c].SetActive(false);
            }
        }
    }

    public void FinalizeChar()
    {
        PlayerPrefs.SetInt("Body", bCurrentItem);
        PlayerPrefs.SetInt("Hat", hCurrentItem);
        PlayerPrefs.SetInt("Mask", mCurrentItem);
        PlayerPrefs.Save();
    }

    public void SwitchBodyItems()
    {
        if (bCurrentItem == bodyItems.Length)
        {
            bCurrentItem = 0;
        }
        else
        {
            bCurrentItem++;
        }
    }
    public void SwitchHatItems()
    {
        if (hCurrentItem == hatItems.Length)
        {
            hCurrentItem = 0;
        }
        else
        {
            hCurrentItem++;
        }
    }
    public void SwitchMaskItems()
    {
        if (mCurrentItem == maskItems.Length)
        {
            mCurrentItem = 0;
        }
        else
        {
            mCurrentItem++;
        }
    }

    public void RotateLeft()
    {
        b_Left = true;
    }
    public void StopLeft()
    {
        b_Left = false;
    }
    public void RotateRight()
    {
        b_Right = true;
    }
    public void StopRight()
    {
        b_Right = false;
    }
}