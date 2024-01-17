using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

public class OpenCVWrapper
{
    [DllImport("opencv_core")]
    public static extern IntPtr CreateORB();

    [DllImport("opencv_imgcodecs")]
    public static extern IntPtr cvCreateImage();

    [DllImport("opencv_features2d")]
    public static extern IntPtr DetectAndCompute();

    [DllImport("opencv_core")]
    public static extern IntPtr CreateCvMat();


}
