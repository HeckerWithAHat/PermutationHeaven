using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DrawingAreaButtonController : MonoBehaviour
{

    public string currentElement = "fire";
    public DrawingAreaDrawManager dadm;
    public Button fire;
    public Button water;
    public Button earth;
    public Button air;
    public Button ice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
            
    }

    public void setCurrentElement(string s)
    {
        switch (s.ToLower())
        {
            case "fire":
                fire.interactable = false;
                water.interactable = true;
                earth.interactable = true;
                air.interactable = true;
                ice.interactable = true;
                currentElement = "fire";
               
                break;
            case "water":
                fire.interactable = true;
                water.interactable = false;
                earth.interactable = true;
                air.interactable = true;
                ice.interactable = true;
                currentElement = "water";
                break;
            case "earth":
                fire.interactable = true;
                water.interactable = true;
                earth.interactable = false;
                air.interactable = true;
                ice.interactable = true;
                currentElement = "earth";
                break;
            case "air":
                fire.interactable = true;
                water.interactable = true;
                earth.interactable = true;
                air.interactable = false;
                ice.interactable = true;
                currentElement = "air";
                break;
            case "ice":
                fire.interactable = true;
                water.interactable = true;
                earth.interactable = true;
                air.interactable = true;
                ice.interactable = false;
                currentElement = "ice";
                break;
        }
        dadm.StartNewDrawing(currentElement);
    }

    public void Submit()
    {
        dadm.Submit();
        //File.Delete(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png");

    }
}
