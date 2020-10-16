﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Utility {
    public class UniversalAudioSolution : MonoBehaviour
    {
        [SerializeField]
        private List<AudioStructure> _audioStructure = new List<AudioStructure>();

        public enum AudioType
        {
            UI, BGM, AudioClip2D, Other 
        }

        private static UniversalAudioSolution _instance;

        public static UniversalAudioSolution instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UniversalAudioSolution>();
                }
                return _instance;
            }
        }

        public void PlayAudio(AudioType audioType, AudioClip audioClip) {
            var audioSource = GetAudioByType(audioType);
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void StopAudio(AudioType audioType) {
            var audioSource = GetAudioByType(audioType);
            audioSource.Stop();

        }

        public void SetAudioTimestamp(AudioType audioType, float seconds)
        {
            var audioSource = GetAudioByType(audioType);
                audioSource.time = seconds;
        }

        private AudioSource GetAudioByType(AudioType audioType) {

            AudioStructure audioStructure = _audioStructure.Find(x => x.audioType == audioType);

            if (audioStructure.audioSource == null) {
                audioStructure = new AudioStructure();
                audioStructure.audioType = audioType;
                audioStructure.audioSource = this.gameObject.AddComponent<AudioSource>();
                audioStructure.audioSource.loop = false;
            }

            return audioStructure.audioSource;
        }

        [System.Serializable]
        public struct AudioStructure {
            public AudioType audioType;
            public AudioSource audioSource;
        }
    }
}

