
using Emgu.CV;
using Emgu.CV.Features2D;

using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.IO;
using UnityEngine;

public class ImageComparison : MonoBehaviour
{
    public static double compareEmgu(string image1path, string image2path)
    {
            // Load the images
        Image<Bgr, byte> img1 = new Image<Bgr, byte>(image1path);
        Image<Bgr, byte> img2 = new Image<Bgr, byte>(image2path);

        // Initialize the ORB detector and matcher
        ORB detector = new ORB(500);
        BFMatcher matcher = new BFMatcher(DistanceType.Hamming);

        // Detect keypoints and compute descriptors in both images
        VectorOfKeyPoint keypoints1 = new VectorOfKeyPoint();
        VectorOfKeyPoint keypoints2 = new VectorOfKeyPoint();
        Mat descriptors1 = new Mat();
        Mat descriptors2 = new Mat();
        detector.DetectAndCompute(img1, null, keypoints1, descriptors1, false);
        detector.DetectAndCompute(img2, null, keypoints2, descriptors2, false);

        // Match the descriptors
        VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
        matcher.KnnMatch(descriptors1, descriptors2, matches, 2);

        // Filter the matches using ratio test
        double ratioThresh = 0.7;
        int numGoodMatches = 0;
        for (int i = 0; i < matches.Size; i++)
        {
            if (matches[i].Size == 2 && matches[i][0].Distance < ratioThresh * matches[i][1].Distance)
                numGoodMatches++;
        }
        Image<Bgr, byte> img1keypoints = img1.Clone();
        Image<Bgr, byte> img2keypoints = img2.Clone();
        Image<Bgr, byte> result = img1.Clone();

        Features2DToolbox.DrawKeypoints(img1, keypoints1, img1keypoints, new Bgr(System.Drawing.Color.Red));
        Features2DToolbox.DrawKeypoints(img2, keypoints2, img2keypoints, new Bgr(System.Drawing.Color.Red));
        Features2DToolbox.DrawMatches(img1, keypoints1, img2, keypoints2, matches, result, new MCvScalar(0,0,255), new MCvScalar(0, 0, 255), null);
        result.Save(Directory.GetCurrentDirectory() + "/Assets/temp/result.png");
        img1keypoints.Save(Directory.GetCurrentDirectory() + "/Assets/temp/img1keypoints.png");
        img2keypoints.Save(Directory.GetCurrentDirectory() + "/Assets/temp/img2keypoints.png");

        // Calculate the image similarity percentage
        double similarityPercentage = (double)numGoodMatches / matches.Size * 100;

        return similarityPercentage;
    }
}


    
 

