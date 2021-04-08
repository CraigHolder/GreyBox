using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public float f_Master = 0.5f;
    public float f_Music = 0.5f;
    public float f_Effects = 0.5f;
    AudioSource[] as_Music;
    AudioSource[] as_Effects;

    public Slider s_Master;
    public Slider s_Music;
    public Slider s_Effects;

    public bool b_AudioUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterV"))
            f_Master = PlayerPrefs.GetFloat("MasterV");

        if (PlayerPrefs.HasKey("MusicV"))
            f_Music = PlayerPrefs.GetFloat("MusicV");

        if (PlayerPrefs.HasKey("EffectsV"))
            f_Effects = PlayerPrefs.GetFloat("EffectsV");

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Music");

        as_Music = new AudioSource[temp.Length];
        for (int m = 0; m < temp.Length; m++) {
            as_Music[m] = temp[m].GetComponent<AudioSource>();
        }

        temp = new GameObject[GameObject.FindGameObjectsWithTag("SFX").Length];
        temp = GameObject.FindGameObjectsWithTag("SFX");

        as_Effects = new AudioSource[temp.Length];
        for (int e = 0; e < temp.Length; e++)
        {
            as_Effects[e] = temp[e].GetComponent<AudioSource>();
        }

        if (s_Master != null)
            s_Master.value = f_Master;
        if (s_Music != null)
            s_Music.value = f_Effects;
        if (s_Effects != null)
            s_Effects.value = f_Effects;
    }

    public void AudioUpdate()
    {
        b_AudioUpdate = true;
    }

    public void ResumeAudio()
    {
        PlayerPrefs.SetFloat("MasterV", f_Master);
    }

    // Update is called once per frame
    void Update()
    {
        if (b_AudioUpdate)
        {
            if (s_Master != null)
            f_Master = s_Master.value;
            if (s_Music != null)
                f_Music = s_Music.value;
            if (s_Effects != null)
                f_Effects = s_Effects.value;

            for (int m = 0; m < as_Music.Length; m++)
            {
                as_Music[m].volume = (1 * f_Music) * f_Master;
            }
            for (int e = 0; e < as_Effects.Length; e++)
            {
                as_Effects[e].volume = (1 * f_Effects) * f_Master;
            }
        }
        b_AudioUpdate = false;
    }
}
