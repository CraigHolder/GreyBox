using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public float f_Master = 0.5f;
    public float f_Music = 0.5f;
    public float f_Effects = 0.5f;

    public Slider s_Master;
    public Slider s_Music;
    public Slider s_Effects;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterV"))
        f_Master = PlayerPrefs.GetFloat("MasterV");

        if (PlayerPrefs.HasKey("MusicV"))
            f_Music = PlayerPrefs.GetFloat("MusicV");

        if (PlayerPrefs.HasKey("EffectsV"))
            f_Effects = PlayerPrefs.GetFloat("EffectsV");

        s_Master.value = f_Master;
        s_Music.value = f_Music;
        s_Effects.value = f_Effects;

    }

    public void SaveSounds()
    {
        PlayerPrefs.SetFloat("MasterV", s_Master.value);
        PlayerPrefs.SetFloat("MusicV", s_Music.value);
        PlayerPrefs.SetFloat("EffectsV", s_Effects.value);
    }

    // Update is called once per frame
    void Update()
    {
        f_Master = s_Master.value;
        f_Music = s_Music.value;
        f_Effects = s_Effects.value;
    }
}
