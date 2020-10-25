using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PluginController : MonoBehaviour
{
    public static PluginController s_TheObj;
    const string DLL_NAME = "PluginV2";

    [DllImport(DLL_NAME)]
    private static extern void ResetLogger();
    [DllImport(DLL_NAME)]
    private static extern float SaveCheckpointTime(float RTBC);
    [DllImport(DLL_NAME)]
    private static extern float GetTotalTime();
    [DllImport(DLL_NAME)]
    private static extern float GetCheckpointTime(int index);
    [DllImport(DLL_NAME)]
    private static extern int GetNumCheckpoints();

    public void SaveTime(float checkpointtime)
    {
        SaveCheckpointTime(checkpointtime);
    }

    public float LoadTime(int index)
    {
        if (index >= GetNumCheckpoints())
        {
            return -1.0f;
        }
        else
        {
            return GetCheckpointTime(index);
        }
    }

    public float LoadTotalTime()
    {
        return GetTotalTime();
    }

    public void ResetLog()
    {
        ResetLogger();
    }

    private void OnDestroy()
    {
        ResetLog();
    }

    void Start()
    {
        lastTime = Time.time;
        if(!s_TheObj)
        {
            s_TheObj = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float lastTime = 0.0f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            float currentTime = Time.time;
            float checkpointTime = currentTime - lastTime;
            lastTime = currentTime;

            SaveTime(checkpointTime);
        }

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                Debug.Log(LoadTime(i));
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(LoadTotalTime());
        }
    }

}
