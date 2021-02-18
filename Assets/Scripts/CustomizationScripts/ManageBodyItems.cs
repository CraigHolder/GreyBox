using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//USE PLAYER PREFS: RESEARCH ON DOCUMENTATION

public class ManageBodyItems : MonoBehaviour
{
    public GameObject player;

    public GameObject[] bodyItems;
    public GameObject[] hatItems;
    public GameObject[] maskItems;

    public RawImage ri_Mask;
    public RawImage ri_Hat;
    public RawImage ri_Torso;

    private int bCurrentItem = 0;
    private int hCurrentItem = 0;
    private int mCurrentItem = 0;

    private bool b_Change = true;
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


        if (b_Change)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        Activate(0, bCurrentItem, bodyItems);
                        SetTorsoImage();
                        break;
                    case 1:
                        Activate(1, hCurrentItem, hatItems);
                        SetHatImage();
                        break;
                    case 2:
                        Activate(2, mCurrentItem, maskItems);
                        SetMaskImage();
                        b_Change = false;
                        break;
                }
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

    public void FinalizeBody()
    {
        PlayerPrefs.SetInt("Body", bCurrentItem);
        PlayerPrefs.SetInt("Hat", hCurrentItem);
        PlayerPrefs.SetInt("Mask", mCurrentItem);
        PlayerPrefs.Save();
    }

    public void SwitchBodyItems()
    {
        b_Change = true;
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
        b_Change = true;
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
        b_Change = true;
        if (mCurrentItem == maskItems.Length)
        {
            mCurrentItem = 0;
        }
        else
        {
            mCurrentItem++;
        }
    }

    public void SetMaskImage()
    {
        switch(mCurrentItem)
        {
            case 0:
                ri_Mask.texture = Resources.Load<Texture>("UI/UITextures/_ask");
                break;
            case 1:
                ri_Mask.texture = Resources.Load<Texture>("UI/UITextures/_oggles");
                break;
            case 2:
                ri_Mask.texture = Resources.Load<Texture>("UI/UITextures/_parrow1");
                break;
        }
    }
    public void SetHatImage()
    {
        switch (hCurrentItem)
        {
            case 0:
                ri_Hat.texture = Resources.Load<Texture>("UI/UITextures/loadingTEXT2");
                break;
            case 1:
                ri_Hat.texture = Resources.Load<Texture>("UI/UITextures/_at");
                break;
            case 2:
                ri_Hat.texture = Resources.Load<Texture>("UI/UITextures/_parrow1");
                break;
        }
    }
    public void SetTorsoImage()
    {
        switch (bCurrentItem)
        {
            case 0:
                ri_Torso.texture = Resources.Load<Texture>("UI/UITextures/_ape");
                break;
            case 1:
                ri_Torso.texture = Resources.Load<Texture>("UI/UITextures/_atwings");
                break;
            case 2:
                ri_Torso.texture = Resources.Load<Texture>("UI/UITextures/_parrow1");
                break;
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