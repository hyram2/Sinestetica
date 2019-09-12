using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilloscopeManager {
    public static float zStretch = 0.7f;
    public static float xStretch = 0.4f;
    public static float yStretch = 0.4f;
    
    public static int resolution = 8000;
    #region Scale params
    public static float minScale = 0.0002f;
    public static float maxScale = 2f;
    public static int filter = 2;
    public static float minLine = 0.0002f;
    public static float maxLine = 2f;
    public static int lineFilter = 4;
    #endregion

	public static Vector3[] LineOscillation (float[] audioData)
    {
        try{
          var points = new Vector3[audioData.Length];
          float increment = 1f / (resolution - 1);

            float z = 0;

            int lenghtTotal = audioData.Length;
        
            for (int i = 0; i < lenghtTotal; i++)
            {
                int actualPosition = i;

                points[actualPosition] += (new Vector3((resolution * audioData[i]) * increment * xStretch,
                                            (resolution * audioData[i]) * increment * yStretch,
                                            z * increment * zStretch)/5);

                z++;
            }
            return points;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    public static void SetOscilloScaler(Transform transform,LineRenderer lineRenderer, SynthValues values)
    {
        Vector3 miScale = new Vector3(minScale,minScale,minScale);
        Vector3 maScale = new Vector3(maxScale, maxScale, maxScale);
        var weight = (values.primaryPose.position.y + values.secondaryPose.position.y) / 2f;
        
        //muda a espessura da linha
        var lineWidth = LineFilter(maxLine, minLine, weight, lineFilter);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        transform.localScale = ScaleFilter(maScale, miScale, weight, filter);
    }

    static Vector3 ScaleFilter(Vector3 maxScale, Vector3 minScale, float weight, int filterCicle) {
        for (int i = 0; i < filter; i++) {
            maxScale = Vector3.Lerp(maxScale, minScale, weight);
        }
        return maxScale;
    }

    static float LineFilter(float maxLine, float minLine, float weight, int lineFilter) {
        for (int i = 0; i < lineFilter; i++)
        {
            maxLine = Mathf.Lerp(maxLine, minLine, weight);
        }
        return maxLine;
    }

    public static void SetColor(LineRenderer lineRenderer, float value, float value2){
        lineRenderer.SetColors(GetColor(value),GetColor(value2));
    }
    static Color GetColor(float value)
    {
        return Color.HSVToRGB(value,1,1);
    }
}