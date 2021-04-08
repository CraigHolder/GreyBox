using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMP_InputField tmp_Name;

    private int currentMaterial = 1;
    private int i_Skin = 0;
    private int i_Colour = 0;
    private int i_ColourR = 0;
    private int i_ColourB = 0;

    public string s_TeamColour = "Red";

    private bool b_TeamChanger = false;
    private bool b_init = false;

    private void Start()
    {
        InitCustom();
    }

    public void SaveColour()
    {
        //Depending on the team, using the saved i_colour to determine backgrounds.
        switch (i_ColourR)
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
        //Then saves the player prefs.
        PlayerPrefs.SetInt("RedColour", i_ColourR);
        PlayerPrefs.SetInt("PlayerTeam", 0);

        //Depending on the team, using the saved i_colour to determine backgrounds.
        switch (i_ColourB)
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
        //Then saves the player prefs.
        PlayerPrefs.SetInt("BlueColour", i_ColourB);
        PlayerPrefs.SetInt("PlayerTeam", 1);
    }

    public void FinalizeColours()
    {
        if (tmp_Name.text.Length > 0)
            PlayerPrefs.SetString("PlayerName", tmp_Name.text);

        PlayerPrefs.SetInt("Skin", i_Skin);

        PlayerPrefs.SetInt("BlueColour", i_ColourB);

        PlayerPrefs.SetInt("RedColour", i_ColourR);
    }

    public void SwitchSkin()
    {
        //Takes skin value and updates it based on this information.
        if (i_Skin == 2)
        {
            i_Skin = 0;
        }
        else
        {
            i_Skin++;
        }

        //Updates the skin based on this information.
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
        //Changes the temp i_Colour variable
        if (i_Colour == 2)
        {
            i_Colour = 0;
        }
        else
        {
            i_Colour++;
        }

        //Applies this information
        ApplyColour();
    }

    public void SwitchTeams()
    {
        //Checks current team
        if (currentMaterial == 1)
        {
            //Sets team colour to temp team colour
            //Then fetches the RedPref and sets that up to finalize.
            i_ColourB = i_Colour;
            i_ColourR = PlayerPrefs.GetInt("RedColour");
            i_Colour = i_ColourR;
            currentMaterial = 0;
        }
        else
        {
            //Sets team colour to temp team colour
            //Then fetches the BluePref and sets that up to finalize.
            i_ColourR = i_Colour;
            i_ColourB = PlayerPrefs.GetInt("BlueColour");
            i_Colour = i_ColourB;
            currentMaterial++;
        }

        ApplyColour();
    }

    public void ColourButtons()
    {
        //Gathers # of displayed images and colours them according to the TeamColour
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
        //Determines current team
        if (currentMaterial == 0)
        {
            //Then uses the temp variable i_colour to determine what it sets the colour to
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
            //After this it sets the Team i_Colour to it's value to the temp value
            i_ColourR = i_Colour;
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
            i_ColourB = i_Colour;
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

        SaveColour();
    }

    public void InitCustom()
    {
        i_ColourB = PlayerPrefs.GetInt("BlueColour");
        i_ColourR = PlayerPrefs.GetInt("RedColour");

        int temp = 0;

        temp = PlayerPrefs.GetInt("Skin");
        PlayerPrefs.SetInt("Skin", temp);
        i_Skin = (temp - 1);
        SwitchSkin();

        currentMaterial = 1;
        i_Colour = PlayerPrefs.GetInt("BlueColour");
        PlayerPrefs.SetInt("BlueColour", i_Colour);
        ApplyColour();

        currentMaterial = 0;
        i_Colour = PlayerPrefs.GetInt("RedColour");
        PlayerPrefs.SetInt("RedColour", i_Colour);
        ApplyColour();

        tmp_Name.text = PlayerPrefs.GetString("PlayerName");
        b_init = true;
    }
}