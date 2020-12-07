using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ThePluginManager : MonoBehaviour
{
    const string DLL_NAME = "EnginesDLLTut";

    //METHODS
    [DllImport(DLL_NAME)]
    private static extern void ResetLogger();

    //SETTERS
    [DllImport(DLL_NAME)]
    private static extern float SaveCheckpointTime(float RTBC);

    //GETTERS
    [DllImport(DLL_NAME)]
    private static extern float GetTotalTime();
    [DllImport(DLL_NAME)]
    private static extern float GetCheckpointTime(int index);
    [DllImport(DLL_NAME)]
    private static extern int GetNumCheckpoints();

    float lastTime = 0.0f;
    //EVERYTHING with TEST in the name is a TESTing function
    public void SaveTimer(float checkpointTime)
    {
        SaveCheckpointTime(checkpointTime);
    }

    public float LoadTimer(int index)
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

    public void ResetLoggerTest()
    {
        ResetLogger();
    }

    // Start is called before the first frame update
    void Start()
    {
        //lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Y))
        //{
        //    float currentTime = Time.time;
        //    float checkpointTime = currentTime - lastTime;
        //    lastTime = currentTime;
        //
        //    SaveTimer(checkpointTime);
        //}

        //for(int i = 0; i < 10; i++)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha0+i))
        //    {
        //        Debug.Log(LoadTimer(i));
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Debug.Log(LoadTotalTime());
        //}
    }

    private void OnDestroy()
    {
        ResetLoggerTest();
    }

}
