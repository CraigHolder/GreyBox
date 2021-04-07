using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageColour : MonoBehaviour
{
    public GameObject player;

    public GameObject TopHat;
    public GameObject DragonWings;
    public GameObject Cape;
    public GameObject Mask;
    public GameObject ArcherHat;
    public GameObject Goggles;

    public RawImage[] displayImages;

    public RawImage ri_RedTeam;
    public RawImage ri_BlueTeam;
    public RawImage ri_Skin;

    private int currentMaterial = 1;
    private int i_Skin = 0;
    private int i_Colour = 0;
    public string s_TeamColour = "Red";

    private bool b_TeamChanger = false;

    private void Start()
    {
        int temp = 0;

        temp = PlayerPrefs.GetInt("Skin");
        PlayerPrefs.SetInt("Skin", temp);
        i_Skin = (temp - 1);
        SwitchSkin();

        temp = PlayerPrefs.GetInt("BlueColour");
        PlayerPrefs.SetInt("BlueColour", temp);
        i_Colour = temp;
        ApplyColour();
        SaveColour();

        currentMaterial = 0;
        temp = PlayerPrefs.GetInt("RedColour");
        PlayerPrefs.SetInt("RedColour", temp);
        i_Colour = temp;
        ApplyColour();
        SaveColour();
    }

    public void SaveColour()
    {
        if (currentMaterial == 0)
        {
            switch (i_Colour)
            {
                case 0:
                    ri_RedTeam.texture = Resources.Load<Texture2D>("UI/redBG");
                    break;
                case 1:
                    ri_RedTeam.texture = Resources.Load<Texture2D>("UI/orangeBG");
                    break;
                case 2:
                    ri_RedTeam.texture = Resources.Load<Texture2D>("UI/yellowBG");
                    break;
            }
        }
        else
        {
            switch (i_Colour)
            {
                case 0:
                    ri_BlueTeam.texture = Resources.Load<Texture2D>("UI/blueBG");
                    break;
                case 1:
                    ri_BlueTeam.texture = Resources.Load<Texture2D>("UI/greenBG");
                    break;
                case 2:
                    ri_BlueTeam.texture = Resources.Load<Texture2D>("UI/purpleBG");
                    break;
            }
        }

        if (currentMaterial == 0)
        {
            PlayerPrefs.SetInt("RedColour", i_Colour);
            PlayerPrefs.SetInt("PlayerTeam", 0);
        }
        else
        {

            PlayerPrefs.SetInt("BlueColour", i_Colour);
            PlayerPrefs.SetInt("PlayerTeam", 1);
        }
    }

    public void FinalizeColours()
    {
        PlayerPrefs.SetInt("Skin", i_Skin);
        PlayerPrefs.SetInt("BlueColour", i_Colour);
        PlayerPrefs.SetInt("RedColour", i_Colour);
    }

    public void SwitchSkin()
    {
        if (i_Skin == 2)
        {
            i_Skin = 0;
        }
        else
        {
            i_Skin++;
        }

        switch (i_Skin)
        {
            case 0:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretBaseColour");
                ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/FerretBaseColour");
                break;
            case 1:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretAlbino");
                ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/FerretAlbino");
                break;
            case 2:
                player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretDarkBrown");
                ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/FerretDarkBrown");
                break;
        }
        PlayerPrefs.SetInt("Skin", i_Skin);
    }

    //public void SetSkimImage()
    //{
    //    switch (i_Skin)
    //    {
    //        case 0:
    //            ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/FerretBasic");
    //            break;
    //        case 1:
    //            ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/FerretAlbino");
    //            break;
    //        case 2:
    //            ri_Skin.texture = Resources.Load<Texture>("Materials/Ferret/Ferret");
    //            break;
    //    }
    //}

    public void SwitchColour()
    {
        if (i_Colour == 2)
        {
            i_Colour = 0;
        }
        else
        {
            i_Colour++;
        }

        ApplyColour();
        SaveColour();
    }

    public void SwitchTeams()
    {
        if (currentMaterial == 1)
        {
            i_Colour = PlayerPrefs.GetInt("RedColour");

            currentMaterial = 0;
        }
        else
        {
            i_Colour = PlayerPrefs.GetInt("BlueColour");

            currentMaterial++;
        }

        ApplyColour();
    }

    public void ColourButtons()
    {
        for (int b = 0; b < displayImages.Length; b++)
        {
            switch (s_TeamColour)
            {
                case "Red":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/RedMat").color;
                    break;
                case "Orange":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/OrangeMat").color;
                    break;
                case "Yellow":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/YellowMat").color;
                    break;
                case "Blue":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/BlueMat").color;
                    break;
                case "Green":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/GreenMat").color;
                    break;
                case "Purple":
                    displayImages[b].color = Resources.Load<Material>("UI/UIMats/PurpleMat").color;
                    break;
            }
        }
    }

    public void ApplyColour()
    {
        if (currentMaterial == 0)
        {
            switch (i_Colour)
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
            switch (i_Colour)
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

        ColourButtons();

        //Backparts
        DragonWings.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Dragon_Wings/Dragon_Wings" + s_TeamColour);
        Cape.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Cape/Cape" + s_TeamColour);
        //Masks
        Goggles.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Goggles/Goggles" + s_TeamColour);
        Mask.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mask/Mask" + s_TeamColour);
        //Hats
        ArcherHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ArcherHat/ArcherHat" + s_TeamColour);
        TopHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Tophat/Tophat" + s_TeamColour);
    }
}