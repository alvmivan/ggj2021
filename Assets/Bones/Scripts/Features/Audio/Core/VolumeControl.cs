﻿using Bones.Scripts.Features.Settings.Domain;
using Bones.Scripts.Features.Settings.Services;
using Injector.Core;
using UniRx;
using UnityEngine;

namespace Bones.Scripts.Features.Audio.Core
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class VolumeControl : MonoBehaviour
    {
        public AudioSource audioSource;

        [SerializeField, Range(0, 1)] private float vol = 1;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private SettingsRepository settings;

        public float Vol
        {
            get => vol;
            set
            {
                vol = value;
                SetVolume();
            }
        }

        private void Awake()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            settings = Injection.Get<SettingsRepository>();
            settings
                .OnChange()
                .Subscribe(settingsModel => SetVolume())
                .AddTo(disposables);
            SetVolume();
        }

        private void OnDestroy()
        {
            disposables.Clear();
        }

        private void OnValidate()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        private void SetVolume()
        {
            var audioSettings = settings.LoadSettings().AudioSettings;
            audioSource.volume = vol * GetMasterVol(audioSettings) * GetSourceVol(audioSettings);
        }

        private float GetMasterVol(AudioSettingsModel settingsModel)
        {
            return settingsModel.MasterVol;
        }

        protected abstract float GetSourceVol(AudioSettingsModel settingsModel);


        public void Stop() => audioSource.Stop();
        public void Play() => audioSource.Play();
        public void Pause() => audioSource.Pause();
        public bool IsPlaying() => audioSource.isPlaying;

        public void SetClip(AudioClip clip)
        {
            audioSource.clip = clip;
        }

        public AudioClip GetClip()
        {
            return audioSource.clip;
        }
    }
}