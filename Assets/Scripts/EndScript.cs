using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public Text t_texttotal;
    public Text t_textcheckpoint1;
    public Text t_textcheckpoint2;
    public Text t_textcheckpoint3;
    public Text t_textcheckpoint4;
    public Text t_textcheckpoint5;
    GameObject obj_plugin;
    PluginController s_plugin;
    // Start is called before the first frame update
    void Start()
    {
        obj_plugin = GameObject.FindWithTag("PluginController");
        s_plugin = obj_plugin.GetComponent<PluginController>();
        t_texttotal.text = s_plugin.LoadTotalTime().ToString();
        t_textcheckpoint1.text = s_plugin.LoadTime(1).ToString();
        t_textcheckpoint2.text = s_plugin.LoadTime(2).ToString();
        t_textcheckpoint3.text = s_plugin.LoadTime(3).ToString();
        t_textcheckpoint4.text = s_plugin.LoadTime(4).ToString();
        t_textcheckpoint5.text = s_plugin.LoadTime(5).ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
