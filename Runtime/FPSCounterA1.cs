using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JimmyScreamFPS
{

    //
    //Code based on this source: https://catlikecoding.com/unity/tutorials/frames-per-second/
    public class FPSCounterA1 : MonoBehaviour
    {

        [Tooltip("The number of frame use to estimate the FPS average")]
        [SerializeField] int m_frameRangeToCaliber = 60;
        [Tooltip("Quick tool to have a debug log in your game as text")]
        [SerializeField] Text[] m_debugSupportAsTextForFps;

        public Debug m_debug = new Debug();
        [System.Serializable]
        public class Debug
        {
            [SerializeField] public string m_frameDebugAsText;
            [SerializeField] public int[] fpsBuffer;
            [SerializeField] public int m_fpsBufferIndex;
            public int m_framePerSeconds;
            public int m_averageFramePerSeconds;
            public int m_highestFramePerSeoncds;
            public int m_lowestFramePerSeoncds;
            public int FPS { get { return m_framePerSeconds; } set { m_framePerSeconds = value; } }
            public int AverageFPS { get { return m_averageFramePerSeconds; } set { m_averageFramePerSeconds = value; } }
            public int HighestFPS { get { return m_highestFramePerSeoncds; } set { m_highestFramePerSeoncds = value; } }
            public int LowestFPS { get { return m_lowestFramePerSeoncds; } set { m_lowestFramePerSeoncds = value; } }
        }

        void Update()
        {
            m_debug.FPS = (int)(1f / Time.unscaledDeltaTime);
            if (m_debug.fpsBuffer == null || m_debug.fpsBuffer.Length != m_frameRangeToCaliber)
            {
                InitializeBuffer();
            }
            UpdateBuffer();
            CalculateFPS();
            m_debug.m_frameDebugAsText = string.Format("FPS:{0:000}\t average:{1:000}\t lowest:{2:00}\t highest:{3:00}", m_debug.FPS, m_debug.AverageFPS, m_debug.LowestFPS, m_debug.HighestFPS);
            for (int i = 0; i < m_debugSupportAsTextForFps.Length; i++)
            {
                if (m_debugSupportAsTextForFps[i])
                    m_debugSupportAsTextForFps[i].text = m_debug.m_frameDebugAsText;
            }
        }

        void UpdateBuffer()
        {
            m_debug.fpsBuffer[m_debug.m_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
            if (m_debug.m_fpsBufferIndex >= m_frameRangeToCaliber)
            {
                m_debug.m_fpsBufferIndex = 0;
            }
        }
        void CalculateFPS()
        {
            int sum = 0;
            int highest = 0;
            int lowest = int.MaxValue;
            for (int i = 0; i < m_frameRangeToCaliber; i++)
            {
                int fps = m_debug.fpsBuffer[i];
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
            m_debug.AverageFPS = sum / m_frameRangeToCaliber;
            m_debug.HighestFPS = highest;
            m_debug.LowestFPS = lowest;
        }
        void InitializeBuffer()
        {
            if (m_frameRangeToCaliber <= 0)
            { m_frameRangeToCaliber = 1; }
            m_debug.fpsBuffer = new int[m_frameRangeToCaliber];
            m_debug.m_fpsBufferIndex = 0;
        }
    }
}