using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
	
	public static Settings instance; 
	[Header("Helm Params")]
    public int SetLenghtAudioData = 64;
    public static int lenghtAudioData = 64;

    public static int playableChannel = 0;
	[Header("Oscilloscope Params")]
    public float zStretch = 0.7f;
    public float xStretch = 0.4f;
    public float yStretch = 0.4f;
    public int resolution = 8000;
	[Header("LineRenderer Params")]
    public float minScale = 0.0002f;
    public float maxScale = 2f;
    public int filter = 2;
    public float minLine = 0.0002f;
    public float maxLine = 2f;
    public int lineFilter = 4;
    [Header("Haptic Params")]
    public ushort minForce = 200;
    public ushort maxForce = 3500;
    [Header("Color Params")]
	public bool ValueOctaveInterpolate = true;
	public float ValueInterpolateFilter = 1.5f;
    public Color A;
	public Color Asharp,B,C,Csharp,D,Dsharp,E,F,Fsharp,G,Gsharp;
	
	void Awake(){
		instance = this;
	}
	// Update is called once per frame
	void Update () {
		OscilloscopeManager.zStretch = zStretch;
		OscilloscopeManager.xStretch = xStretch;
		OscilloscopeManager.yStretch = yStretch;
		OscilloscopeManager.resolution = resolution;
		OscilloscopeManager.minScale = minScale;
		OscilloscopeManager.maxScale = maxScale;
		OscilloscopeManager.filter = filter;
		OscilloscopeManager.minLine = minLine;
		OscilloscopeManager.maxLine = maxLine;
		OscilloscopeManager.lineFilter = lineFilter;

        lenghtAudioData = SetLenghtAudioData;
	}

}
