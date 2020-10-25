using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartButtonpush()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void StatButtonpush()
    {
        SceneManager.LoadScene("EndScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
