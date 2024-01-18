
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using UnityEngine;


public class ImageComparison : MonoBehaviour
{
    public static void compareEmgu(String image1path, String image2path /*, String resultPath*/ )
    {
        // Load the images
        Image<Bgr, byte> img1 = new Image<Bgr, byte>(image1path);
        Image<Bgr, byte> img2 = new Image<Bgr, byte>(image2path);

        // Initialize the ORB detector and matcher
        ORB orb = new ORB(500);
    BFMatcher matcher = new BFMatcher(DistanceType.Hamming);

    // Detect keypoints and compute descriptors in both images
    VectorOfKeyPoint keypoints1 = new VectorOfKeyPoint();
    VectorOfKeyPoint keypoints2 = new VectorOfKeyPoint();
        Matrix<byte> descriptors1 = null;
        Matrix<byte> descriptors2 = null;
    orb.DetectAndCompute(img1, null, keypoints1, descriptors1, false);
       orb.DetectAndCompute(img2, null, keypoints2, descriptors2, false);

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

        Debug.Log("Ratio of good matches: " + numGoodMatches/matches.Length);
        /*
// Draw the matches
Image<Bgr, byte> result = img1.Clone();
        Features2DToolbox.DrawMatches(img1, keypoints1, img2, keypoints2, matches, result, new MCvScalar(), new MCvScalar(), null);
        result.Save(resultPath);
        */
    }
   
 
}
