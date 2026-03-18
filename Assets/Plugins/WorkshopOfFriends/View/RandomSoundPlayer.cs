using System.Collections.Generic;
using UnityEngine;

namespace Wof.Views
{
    public class RandomSoundPlayer : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private AudioSource audioSourceTemplate;
        [SerializeField] private int maxConcurrentSounds = 20;

        private List<AudioSource> audioSources;

        public static RandomSoundPlayer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            audioSources = new List<AudioSource>();
            for (int i = 0; i < maxConcurrentSounds; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();

                if (audioSourceTemplate != null)
                {
                    CopyAudioSourceSettings(audioSourceTemplate, source);
                }
                else
                {
                    source.playOnAwake = false;
                    source.spatialBlend = 0f;
                }

                audioSources.Add(source);
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void CopyAudioSourceSettings(AudioSource from, AudioSource to)
        {
            to.outputAudioMixerGroup = from.outputAudioMixerGroup;
            to.mute = from.mute;
            to.bypassEffects = from.bypassEffects;
            to.bypassListenerEffects = from.bypassListenerEffects;
            to.bypassReverbZones = from.bypassReverbZones;
            to.playOnAwake = from.playOnAwake;
            to.loop = from.loop;

            to.priority = from.priority;
            to.volume = from.volume;
            to.pitch = from.pitch;
            to.panStereo = from.panStereo;
            to.spatialBlend = from.spatialBlend;
            to.reverbZoneMix = from.reverbZoneMix;

            to.dopplerLevel = from.dopplerLevel;
            to.spread = from.spread;
            to.minDistance = from.minDistance;
            to.maxDistance = from.maxDistance;

            to.rolloffMode = from.rolloffMode;
        }

        /// <summary>
        /// Пытается воспроизвести случайный звук из переданного списка.
        /// </summary>
        /// <param name="sounds">Список звуков, из которого будет выбран случайный клип.</param>
        public void Play(IReadOnlyList<AudioClip> sounds)
        {
            if (sounds == null || sounds.Count == 0)
            {
                Debug.LogWarning("Передан пустой список звуков!");
                return;
            }

            AudioSource freeSource = audioSources.Find(source => !source.isPlaying);
            if (freeSource != null)
            {
                AudioClip clip = sounds[Random.Range(0, sounds.Count)];
                freeSource.clip = clip;
                freeSource.Play();
            }
            else
            {
                Debug.Log("Достигнут лимит одновременных звуков, пропускаем.");
            }
        }
    }
}
