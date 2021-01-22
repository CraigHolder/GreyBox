﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBodyItems : MonoBehaviour
{
    public GameObject player;

    public GameObject[] bodyItems;
    private int bCurrentItem;
    public GameObject[] hatItems;
    private int hCurrentItem;
    public GameObject[] maskItems;
    private int mCurrentItem;

    void Update()
    {
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
                items[c].transform.rotation = player.transform.rotation;
            }
            else
            {
                items[c].SetActive(false);
            }
        }
    }

    public void SwitchBodyItems()
    {
        if (bCurrentItem == bodyItems.Length - 1)
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
        if (hCurrentItem == hatItems.Length - 1)
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
        if (mCurrentItem == maskItems.Length - 1)
        {
            mCurrentItem = 0;
        }
        else
        {
            mCurrentItem++;
        }
    }
}