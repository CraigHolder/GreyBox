using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (background.activeSelf == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                background.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                background.SetActive(true);
            }
        }
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        background.SetActive(false);
    }
}
