using System;
using UnityEngine;

namespace Tutorial_5
{
    public enum EffectType { None, ILD, ITD }

    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private EffectType currentEffect = EffectType.None;
        [Tooltip("Use position of player and audio source in the scene instead of the stereo pan slider")]
        [SerializeField] private bool useScene;

        [Range(0, 1)]
        [SerializeField] private float volume = 1f;
        [Range(0, 1)]
        [Tooltip("0 is left, 1 is right")]
        [SerializeField] private float stereoPosition = 0.5f;
        [Tooltip("Maximum ITD delay in milliseconds")]
        [SerializeField] private float maxHaasDelay = 20f;
        [SerializeField] private int sampleRate = 44100;

        [Header("References")]
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private Transform player;
        [SerializeField] private float maxPanDistance = 10f; // tweak in inspector


        private AudioSource audioSource;

        private float[] leftDelayBuffer;
        private float[] rightDelayBuffer;
        private int bufferSize;
        private int leftWriteIndex = 0;
        private int rightWriteIndex = 0;
        private int leftReadIndex = 0;
        private int rightReadIndex = 0;
        private int leftDelaySamples;
        private int rightDelaySamples;

        void Start()
        {
            // Initialize audio source
            audioSource.clip = audioClip;

            // Initialize delay buffers
            bufferSize = Mathf.CeilToInt((maxHaasDelay / 1000f) * sampleRate);
            leftDelayBuffer = new float[bufferSize];
            rightDelayBuffer = new float[bufferSize];

            // main camera is the player
            player = Camera.main.transform;
        }

        private void Update()
        {
            //if (useScene)
            //{
            //    UpdateParamsFromScene();
            //}
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            if (channels < 2 || leftDelayBuffer == null || rightDelayBuffer == null)
            {
                Debug.LogError("This script requires a stereo audio source with initialized delay buffers.");
                return;
            }

            for (var i = 0; i < data.Length; i++)
            {
                // apply volume
                data[i] *= volume;
            }

            // Apply the selected effect
            switch (currentEffect)
            {
                case EffectType.ILD:
                    ApplyILD(data, channels);
                    break;
                case EffectType.ITD:
                    ApplyITD(data, channels);
                    break;
            }
        }

        private void ApplyILD(float[] data, int channels)
        {
            if (channels < 2) return;

            // stereoPosition: 0 = left, 1 = right
            float a = Mathf.Clamp01(stereoPosition) * (Mathf.PI * 0.5f);
  
            // Equal-power (sine/cosine) panning
            float gL = Mathf.Cos(a);
            float gR = Mathf.Sin(a);
       

            // Apply gains to interleaved stereo data: L, R, L, R...
            for (int i = 0; i < data.Length; i += channels)
            {
                data[i] *= gL; // Left
                data[i + 1] *= gR; // Right
            }
        }


        private void ApplyITD(float[] data, int channels)
        {
            if (channels < 2) return;

            // stereoPosition: 0 left, 1 right => pan in [-1, +1]
            float pan = Mathf.Clamp(2f * stereoPosition - 1f, -1f, 1f);

            // Delay amount in ms (0 at center, max at edges)
            float delayMs = Mathf.Abs(pan) * maxHaasDelay;

            // Convert ms -> samples: dn = dt * f
            int d = Mathf.RoundToInt((delayMs / 1000f) * sampleRate);
            d = Mathf.Clamp(d, 0, bufferSize - 1);

            // Delay only one channel:
            // pan < 0 => sound left => delay RIGHT
            // pan > 0 => sound right => delay LEFT
            bool delayLeft = pan > 0f;

            for (int i = 0; i < data.Length; i += channels)
            {
                float inL = data[i];
                float inR = data[i + 1];

                // write incoming samples
                leftDelayBuffer[leftWriteIndex] = inL;
                rightDelayBuffer[leftWriteIndex] = inR;

                // read delayed samples (d samples behind)
                int readIndex = leftWriteIndex - d;
                if (readIndex < 0) readIndex += bufferSize;

                float outL = delayLeft ? leftDelayBuffer[readIndex] : inL;
                float outR = delayLeft ? inR : rightDelayBuffer[readIndex];

                // output
                data[i] = outL;
                data[i + 1] = outR;

                // advance shared write index
                leftWriteIndex++;
                if (leftWriteIndex >= bufferSize) leftWriteIndex = 0;
            }
        }

        private void UpdateParamsFromScene()
        {
            if (player == null) return;

            Vector3 srcPos = transform.position;
            //Basically sound - camera
            Vector3 rel = srcPos - player.position;

            float side = Vector3.Dot(rel, player.right); 
            float pan = Mathf.Clamp(side / maxPanDistance, -1f, 1f); // -1..+1
            stereoPosition = 0.5f * (pan + 1f);            // -> 0..1

            float distance = rel.magnitude;
            volume = 1f / (1f + distance * 0.2f);
        }

        private void OnValidate()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            audioSource.hideFlags = HideFlags.HideInInspector;
        }


        public void PlayOneShotAt(Vector3 worldPos)
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (player == null && Camera.main != null) player = Camera.main.transform;

            transform.position = worldPos;
            useScene = true;

            audioSource.loop = false;

            leftWriteIndex = 0;
            System.Array.Clear(leftDelayBuffer, 0, leftDelayBuffer.Length);
            System.Array.Clear(rightDelayBuffer, 0, rightDelayBuffer.Length);

            UpdateParamsFromScene();
            audioSource.Play();
        }


    }
}
