using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmyScreamFPS
{
    public class JimmyScreamer : MonoBehaviour
    {

        public FPSCounterA1 m_fpsState;
        [Tooltip("Source of the voice sound. Put a sound of someone having pain.")]
        public AudioSource m_voiceSound;
        [Tooltip("Source of the bit sound. Put sound that the ear don't like.")]
        public AudioSource m_alarmSound;

        [Tooltip("Will auto start the debugger")]
        public bool autoStart = true;

        [Header("Configure Range")]
        [Tooltip("If under this FPS start to scream")]
        public int m_boundaryJimmyStart = 60;
        [Tooltip("If FPS reach this value, scream sound will be max volume")]
        public int m_boundaryJimmyMax = 40;
        [Tooltip("If under this FPS start to bip")]
        public int m_boundaryHellStart = 40;
        [Tooltip("If FPS reach this value, bip sound will be max volume")]
        public int m_boundaryHellMax = 2;

        [Tooltip("Use to turn on and off the debugger")]
        [SerializeField] bool m_isRequestAsActive;

        [SerializeField] DebugValue m_debug= new DebugValue();
        [System.Serializable]
        public class DebugValue { 
            [Header("Debug value")]
            [SerializeField] public int m_fps;
            [SerializeField] public float m_jimmy;
            [SerializeField] public bool m_jimmyState;
            [SerializeField] public float m_hell;
            [SerializeField] public bool m_hellState;

        }

        public void Awake()
        {
            if (autoStart)
                Invoke("StartListening", 1.5f);
        }

        public void StartListening()
        {
            m_isRequestAsActive = true;
        }
        public void StopListening()
        {
            m_isRequestAsActive = false;
        }

        void Update()
        {
            if (!m_isRequestAsActive) return;
            bool previous = false;
            int fps = m_fpsState.m_debug.AverageFPS;

            previous = m_debug.m_jimmyState;
            m_debug.m_jimmy = GetPourcentFrom(fps, m_boundaryJimmyMax, m_boundaryJimmyStart);
            m_debug.m_jimmyState = m_debug.m_jimmy > 0f;
            if (previous != m_debug.m_jimmyState)
            {
                if (m_debug.m_jimmyState)
                    m_voiceSound.Play();
                else m_voiceSound.Stop();
            }

            previous = m_debug.m_hellState;
            m_debug.m_hell = GetPourcentFrom(fps, m_boundaryHellMax, m_boundaryHellStart);
            m_debug.m_hellState = m_debug.m_hell > 0f;
            if (previous != m_debug.m_hellState)
            {
                if (m_debug.m_hellState)
                    m_alarmSound.Play();
                else m_alarmSound.Stop();
            }

            m_voiceSound.volume = m_debug.m_jimmy;
            m_alarmSound.volume = m_debug.m_hell;


        }

        private float GetPourcentFrom(float fps, float critiqueFps, float OkFps)
        {
            return 1f - Mathf.Clamp(((fps - critiqueFps) / (OkFps - critiqueFps)), 0f, 1f);
        }
    }
}