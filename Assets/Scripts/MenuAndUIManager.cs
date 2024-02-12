using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuAndUIManager : MonoBehaviour
{

    public Camera Camera; // The camera object


    public GameObject SpellCastingScreen; // The screen for spell casting
    CameraController cameraController; // The camera controller component
    public PlayerController playerController; // The player controller component
    // Start is called before the first frame update
    private void Awake()
    {
        cameraController = Camera.GetComponent<CameraController>(); // Get the camera controller component
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerController.isCasting) return;
            SpellCastingScreen.SetActive(true); // Toggle the visibility of the spell casting screen
            Cursor.visible = true; // Toggle the visibility of the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor if it is currently locked
            File.Delete(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png"); // Delete a temporary file
            cameraController.POV = 1; // Set the camera POV to 1
            cameraController.FreezeCamera = true; // Toggle the freeze camera flag in the camera controller

        }
    }

}
