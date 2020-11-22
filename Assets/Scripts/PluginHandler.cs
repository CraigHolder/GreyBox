using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

//GreyBox Project

public class PluginHandler : MonoBehaviour
{

    const string DLL_NAME = "Stats_Logger";

    [DllImport(DLL_NAME)]
    private static extern void addJump();

    [DllImport(DLL_NAME)]
    private static extern void addBounce();

    [DllImport(DLL_NAME)]
    private static extern void addHazard(float time);

    [DllImport(DLL_NAME)]
    private static extern void setScore(int newScore);

    [DllImport(DLL_NAME)]
    private static extern void addStamina(float staminaUsed);

    [DllImport(DLL_NAME)]
    private static extern void writeToFile();

    [DllImport(DLL_NAME)]
    private static extern void resetLogger();

    public void recordJump() //Implemented
    {
        addJump();
    }

    public void recordBounce() //Implemented
    {
        addBounce();
    }

    public void recordHazard(float t) //Implemented
    {
        addHazard(t);
    }

    public void recordScore(int s) //Implemented
    {
        setScore(s);
    }

    public void recordStamina(float u) //Implemented
    {
        addStamina(u);
    }

    public void createFile() //Implemented
    {
        writeToFile();
    }

    public void resetLogFile() //Implemented
    {
        resetLogger();
    
    }



    //Start is called before the first frame update
    void Start()
    {
        //Call reset
        resetLogFile();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
