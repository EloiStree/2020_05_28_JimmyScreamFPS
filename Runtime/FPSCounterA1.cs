using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//SOURCE: https://catlikecoding.com/unity/tutorials/frames-per-second/
public class FPSCounterA1 : MonoBehaviour
{
    public int FPS { get; private set; }
    public int AverageFPS { get; private set; }
    int[] fpsBuffer;
    int fpsBufferIndex; 
    public int frameRange = 60;


    public Text [] m_displayFps;
    public string m_display;
    void Update()
    {
        FPS = (int)(1f / Time.deltaTime);
        FPS = (int)(1f / Time.unscaledDeltaTime);
        //FPS = Mathf.Clamp(FPS, 0, 99);

        if (fpsBuffer == null || fpsBuffer.Length != frameRange)
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
        m_display = string.Format("{0:000}\t{1:000}\tl {2:00}\t H {3:00}", FPS, AverageFPS, LowestFPS, HighestFPS);
        for (int i = 0; i < m_displayFps.Length; i++)
        {
            if (m_displayFps[i])
                m_displayFps[i].text = m_display;
        }
    }

    void UpdateBuffer()
    {
        fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (fpsBufferIndex >= frameRange)
        {
            fpsBufferIndex = 0;
        }
    }
    public int HighestFPS { get; private set; }
    public int LowestFPS { get; private set; }
    void CalculateFPS()
    {
        int sum = 0;
        int highest = 0;
        int lowest = int.MaxValue;
        for (int i = 0; i < frameRange; i++)
        {
            int fps = fpsBuffer[i];
            sum += fps;
            if (fps > highest)
            {
                highest = fps;
            }
            if (fps < lowest)
            {
                lowest = fps;
            }
        }
        AverageFPS = sum / frameRange;
        HighestFPS = highest;
        LowestFPS = lowest;
    }
    void InitializeBuffer()
    {
        if (frameRange <= 0)
        {frameRange = 1;}
        fpsBuffer = new int[frameRange];
        fpsBufferIndex = 0;
    }
}
