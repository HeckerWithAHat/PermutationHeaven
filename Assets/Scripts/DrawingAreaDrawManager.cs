using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using Accord.Imaging;

public class DrawingAreaDrawManager : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public RawImage drawingarea;
    public GameObject SpellCastingScreen; // The screen for spell casting
    public Camera Camera; // The camera object
    public GameObject Player; // The player object

    public GameObject Fireball; // The fireball projectile

    Texture2D currentTexture;
    string element;
    UnityEngine.Color elementColor = UnityEngine.Color.magenta;
    public Texture2D Vortex;
    public Texture2D drawnImage;

    int topY, bottomY, leftX, rightX;
    Vector2Int lastPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawingarea.texture = currentTexture;
    }

    private void OnApplicationQuit()
    {
        File.Delete(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png");
        
    }
    public void StartNewDrawing(string element)
    {
        currentTexture = new Texture2D(900, 900);
        var texColors = new Color32[900 * 900];
        for (int i = 0; i < texColors.Length; i++)
        {
            texColors[i] = Color.clear;
        }
        currentTexture.SetPixels32(texColors);
        currentTexture.Apply();

        this.element = element;

        switch(this.element)
        {
            case "water": elementColor = UnityEngine.Color.blue; break;
            case "ice": elementColor = UnityEngine.Color.white; break;
            case "fire": elementColor = UnityEngine.Color.red; break;
            case "earth": elementColor = UnityEngine.Color.green; break;
            case "air": elementColor = UnityEngine.Color.gray; break;
            default: elementColor = UnityEngine.Color.magenta; break;

        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        for (int x = -7; x < 8; x++)
        {
            for (int y = -7; y < 8; y++)
            {
                currentTexture.SetPixel(Mathf.FloorToInt(Input.mousePosition.x) - 990 + x, Mathf.FloorToInt(Input.mousePosition.y) - 90 + y, elementColor);
            }
        }
        lastPoint = new Vector2Int(Mathf.FloorToInt(Input.mousePosition.x) - 990, Mathf.FloorToInt(Input.mousePosition.y) - 90);

        topY = Mathf.FloorToInt(Input.mousePosition.y+7) - 90;
        bottomY = Mathf.FloorToInt(Input.mousePosition.y-7) - 90;
        leftX = Mathf.FloorToInt(Input.mousePosition.x-7) - 990;
        rightX = Mathf.FloorToInt(Input.mousePosition.x+7) - 990;


    }

    public void OnDrag(PointerEventData eventData)
    {

        line(lastPoint.x, lastPoint.y, Mathf.FloorToInt(Input.mousePosition.x) - 990, Mathf.FloorToInt(Input.mousePosition.y) - 90, elementColor);
        lastPoint = new Vector2Int(Mathf.FloorToInt(Input.mousePosition.x) - 990, Mathf.FloorToInt(Input.mousePosition.y) - 90);
        if (lastPoint.x+7>rightX) rightX = lastPoint.x+7;
        if (lastPoint.x-7<leftX) leftX = lastPoint.x-7; 
        if (lastPoint.y+7>topY) topY = lastPoint.y+7;
        if (lastPoint.y-7<bottomY) bottomY = lastPoint.y-7; 
        currentTexture.Apply();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
    }


    public void Submit()
    {
        drawnImage = currentTexture;
        byte[] ba = currentTexture.EncodeToPNG();

        var fs = File.OpenWrite(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png");
        using var writer = new BinaryWriter(fs);
        writer.Write(ba);

                // Convert textures to grayscale
        Texture2D grayscaleDrawn = DrawingComparisonManager.ConvertToGrayscale(DrawingComparisonManager.resizeImage(DrawingComparisonManager.recenterImage(drawnImage, topY, bottomY, leftX, rightX), 900, 900));
        Texture2D grayscaleTarget = DrawingComparisonManager.ConvertToGrayscale(Vortex);


        byte[] ba1 = grayscaleDrawn.EncodeToPNG();

        var fs1 = File.OpenWrite(Directory.GetCurrentDirectory() + "/Assets/temp/temp1.png");
        using var writer1 = new BinaryWriter(fs1);
        writer1.Write(ba1);
        // Calculate SSIM
        float SSIM = DrawingComparisonManager.CalculateSimilarityPercentage(grayscaleDrawn, grayscaleTarget);

        


        //float GCP = DrawingComparisonManager.GCP(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png", Directory.GetCurrentDirectory() + "/Assets/Runes/Air/Vortex.png"); 





        Debug.Log("drawnImage: " + drawnImage.width+"x"+drawnImage.height + ". drawnImageGray: " + grayscaleDrawn.width+"x"+grayscaleDrawn.height + ". Vortex: " + Vortex.height+"x"+Vortex.width + ". VortexGray: " + grayscaleTarget.width+"x"+grayscaleTarget.height);

        Debug.Log("Percent Of Accuracy with SSIM: " + SSIM + "%");
        //Debug.Log("Percent Of Accuracy with GithubCopilot: " + GCP + "%");
        SpellCastingScreen.SetActive(false); // Toggle the visibility of the spell casting screen
        Cursor.visible = false; // Toggle the visibility of the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        
        Camera.GetComponent<CameraController>().FreezeCamera = false; // Toggle the freeze camera flag in the camera controller
        Player.GetComponent<PlayerController>().isCasting = true; // Toggle the is casting flag in the player controller
        Player.GetComponent<PlayerController>().ProjectileToFire = Fireball; // Set the projectile to fire in the player controller
    }
    public void line(int x, int y, int x2, int y2, Color color)
    {
        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            for (int xi = -10; xi < 10; xi++)
            {
                for (int yi = -10; yi < 10; yi++)
                {
                    currentTexture.SetPixel(x+xi, y+yi, color);
                }
            }
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }
}
