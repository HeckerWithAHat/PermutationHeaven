
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using System.IO;
using System.Collections;
using System;
public class DrawingComparisonManager : MonoBehaviour
{

   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    


    

    // Function to convert a texture to grayscale
    public static Texture2D ConvertToGrayscale(Texture2D inputTexture)
    {
        Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

        for (int x = 0; x < inputTexture.width; x++)
        {
            for (int y = 0; y < inputTexture.height; y++)
            {
                UnityEngine.Color pixel = inputTexture.GetPixel(x, y);

                if (pixel.a > 0.0f)
                {
                    float grayValue = (pixel.r + pixel.g + pixel.b) / 3f;
                    outputTexture.SetPixel(x, y, new UnityEngine.Color(grayValue, grayValue, grayValue));
                } else {
                    outputTexture.SetPixel(x, y, UnityEngine.Color.clear);
                }
            }
        }

        outputTexture.Apply();
        return outputTexture;
    }







    public static Texture2D recenterImage(Texture2D originalImage, int topY, int bottomY, int leftX, int rightX)
    {
        Vector2 bottomRightCorner = new Vector2(rightX, bottomY);
        Vector2 topLeftCorner = new Vector2(leftX, topY);
        int width = (int)(bottomRightCorner.x - topLeftCorner.x);
        int height = (int)(topLeftCorner.y - bottomRightCorner.y);
        Debug.Log("Width:" + width);
        Debug.Log("Height:" + height);
        Texture2D centeredImage = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int originalX = (int)(topLeftCorner.x + x);
                int originalY = (int)(topLeftCorner.y + y);

                UnityEngine.Color pixel = originalImage.GetPixel(originalX, originalY);
                centeredImage.SetPixel(x, y, pixel);
            }
        }

        centeredImage.Apply();
        return centeredImage;
    }


 




}
