
using System.Collections.Generic;
using Accord.Imaging;
using Accord.Imaging.Filters;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using System.IO;
using System.Collections;

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

    // Function to calculate SSIM (Inverse of MSE)
    public static float CalculateSimilarityPercentage(Texture2D image1, Texture2D image2)
    {
        UnityEngine.Color[] pixels1 = image1.GetPixels();
        UnityEngine.Color[] pixels2 = image2.GetPixels();

        if (pixels1.Length != pixels2.Length)
        {
            Debug.LogError("Images have different dimensions.");
            return -1;
        }

        float sumSquaredDifference = 0f;
        int nonTransparentPixels = 0;



        for (int i = 0; i < pixels1.Length; i++)
        {
            if (pixels1[i].a > 0.0f && pixels2[i].a > 0.0f)
            {

                float difference = (pixels1[i].r - pixels2[i].r);
                sumSquaredDifference += difference * difference;
                nonTransparentPixels++;
            }
        }
        if (nonTransparentPixels == 0)
        {
            Debug.LogError("No non-transparent pixels found.");
            return -1;
        }
        return (1-(sumSquaredDifference / nonTransparentPixels))*100;
    }

    






    public static float CompareImages(Texture2D drawnImage, Texture2D targetImage)
    {
        // Convert textures to Bitmap objects
        Bitmap drawnBitmap = UnityTextureToBitmap(drawnImage);
        Bitmap targetBitmap = UnityTextureToBitmap(targetImage);
        // Convert to grayscale
        Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
        drawnBitmap = grayFilter.Apply(drawnBitmap);
        targetBitmap = grayFilter.Apply(targetBitmap);
        // Find contours
        BlobCounter blobCounter = new BlobCounter();
        blobCounter.FilterBlobs = true;
        blobCounter.MinWidth = 5;
        blobCounter.MinHeight = 5;
        blobCounter.ProcessImage(drawnBitmap);
        Blob[] drawnBlobs = blobCounter.GetObjectsInformation();
        blobCounter.ProcessImage(targetBitmap);
        Blob[] targetBlobs = blobCounter.GetObjectsInformation();
        // Compare contours (you may need to implement a more advanced matching algorithm)
        double similarityPercentage = ContourMatching(drawnBlobs, targetBlobs);
        return (float) similarityPercentage;
    }

    static double ContourMatching(Blob[] drawnBlobs, Blob[] targetBlobs)
    {
        // Implement your contour matching algorithm here
        // You may use blob area comparison, geometric features, or other techniques
        // For simplicity, let's assume a basic area comparison

        double drawnArea = drawnBlobs.Length > 0 ? drawnBlobs[0].Area : 0;
        double targetArea = targetBlobs.Length > 0 ? targetBlobs[0].Area : 0;

        double similarityPercentage = Mathf.Min(1f, (float)(drawnArea / targetArea) * 100f);

        return similarityPercentage;
    }



    public static float GCP(string fp1, string fp2)
    {
        Bitmap image1 = new Bitmap(fp1);
        Bitmap image2 = new Bitmap(fp2);

        // Apply transformations to the images if needed
        // For example, to handle warps, translations, rotations, and any kind of transformation,
        // you can use the Accord.NET library's image processing filters

        // Create an ExhaustiveTemplateMatching object
        ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.9f);

        // Find matching areas between the two images
        TemplateMatch[] matches = tm.ProcessImage(image1, image2);

        // Calculate the percent of similarity
        float similarity = (float)matches.Length / (float)image1.Width * 100f;

        return similarity;
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


    public static Texture2D resizeImage(Texture2D originalImage, int width, int height)
    {
        ResizeBilinear filter = new ResizeBilinear(width, height);
        Bitmap bitmap = UnityTextureToBitmap(originalImage);
        Bitmap resizedBitmap = filter.Apply(bitmap);
        Texture2D resizedTexture = BitmapToUnityTexture(resizedBitmap);
        return resizedTexture;
    }

    private static Bitmap UnityTextureToBitmap(Texture2D texture)
    {
        Bitmap bmp = new Bitmap(texture.width, texture.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color32 pixelColor = texture.GetPixel(x, y);
                bmp.SetPixel(x, texture.height - 1 - y, System.Drawing.Color.FromArgb(pixelColor.a, pixelColor.r, pixelColor.g, pixelColor.b));
            }
        }

        return bmp;
    }

    private static Texture2D BitmapToUnityTexture(Bitmap bitmap)
    {
        Texture2D texture = new Texture2D(bitmap.Width, bitmap.Height);

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                System.Drawing.Color pixelColor = bitmap.GetPixel(x, bitmap.Height - 1 - y);
                UnityEngine.Color unityColor = new UnityEngine.Color(pixelColor.R / 255f, pixelColor.G / 255f, pixelColor.B / 255f, pixelColor.A / 255f);
                texture.SetPixel(x, y, unityColor);
            }
        }

        texture.Apply();
        return texture;
    }

}
