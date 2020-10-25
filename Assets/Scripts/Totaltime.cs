using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Totaltime : MonoBehaviour
{
    Text t_text;
    public PluginController s_plugin;
    float f_time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        t_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        f_time += Time.deltaTime;
        t_text.text = f_time.ToString();
    }
}
