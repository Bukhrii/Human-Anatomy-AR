using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Vuforia;


public class CameraFocus : MonoBehaviour {

    void Start()
    {
    VuforiaApplication.Instance.OnVuforiaStarted += StartVuforiaFocus;
    }
    public void StartVuforiaFocus()
    {
    VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
    // Update is called once per frame
    void Update() 
    {

    }
}