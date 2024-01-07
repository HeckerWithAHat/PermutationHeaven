using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public int distanceFromPlayer = 5;
    public Vector2 cameraOffset;
    private bool freezeCamera = false;
    private int pov = 3;


    float rotationX;
    float rotationY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (freezeCamera) return;
        if (pov == 3) 
        { 
            rotationX += Input.GetAxis("Mouse Y");
            rotationX = Mathf.Clamp(rotationX, -90, 90);
            rotationY += Input.GetAxis("Mouse X");

            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

            var focusPosition = Player.transform.position + new Vector3(cameraOffset.x, cameraOffset.y);
            transform.position = focusPosition - targetRotation * new Vector3(0, 0, distanceFromPlayer);
            transform.rotation = targetRotation;
        } else if (pov == 1)
        {
            rotationX += Input.GetAxis("Mouse Y");
            rotationX = Mathf.Clamp(rotationX, -90, 90);
            rotationY += Input.GetAxis("Mouse X");
            transform.position = Player.transform.position + new Vector3(0, 0.6f, 0);
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }   
    }


    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

    public bool FreezeCamera { get => freezeCamera; set => freezeCamera = value; }
    public int POV { get => pov; set => pov = value; }
}