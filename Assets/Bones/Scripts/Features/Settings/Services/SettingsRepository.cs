using System;
using Bones.Scripts.Features.Settings.Domain;
using UniRx;
using UnityEngine;

namespace Bones.Scripts.Features.Settings.Services
{
    public interface SettingsRepository
    {
        SettingsModel LoadSettings();
        void SaveSettings(SettingsModel settings);
        IObservable<SettingsModel> OnChange();
    }


    public class LocalSettingsRepository : SettingsRepository
    {
        private const string SettingsKey = "__settings__";


        private static readonly SettingsDTO DefaultSettings = new SettingsDTO
        {
            fullScreen = true,
            quality = 1,
            masterVol = 1,
            musicVol = 1,
            sfxVol = 1,
            difficulty = (int) DifficultyLevel.Normal
        };

        private readonly ISubject<SettingsModel> subject = new Subject<SettingsModel>();


        public SettingsModel LoadSettings()
        {
            var settings = PlayerPrefs.GetString(SettingsKey, string.Empty);

            var settingsDTO = string.IsNullOrEmpty(settings)
                ? DefaultSettings
                : JsonUtility.FromJson<SettingsDTO>(settings);

            var model = new SettingsModel
            {
                VideoSettings = new VideoSettingsModel
                {
                    Quality = (VideoQuality) settingsDTO.quality,
                    FullScreen = settingsDTO.fullScreen
                },
                AudioSettings = new AudioSettingsModel
                {
                    SFXVol = settingsDTO.sfxVol,
                    MusicVol = settingsDTO.musicVol,
                    MasterVol = settingsDTO.masterVol
                },
                GameSettings = new GameSettingsModel
                {
                    Difficulty = (DifficultyLevel) settingsDTO.difficulty
                }
            };
            return model;
        }

        public void SaveSettings(SettingsModel settings)
        {
            var dto = new SettingsDTO
            {
                difficulty = (int) settings.GameSettings.Difficulty,
                quality = (int) settings.VideoSettings.Quality,
                fullScreen = settings.VideoSettings.FullScreen,
                sfxVol = settings.AudioSettings.SFXVol,
                musicVol = settings.AudioSettings.MusicVol,
                masterVol = settings.AudioSettings.MasterVol
            };
            var json = JsonUtility.ToJson(dto);
            PlayerPrefs.SetString(SettingsKey, json);
            PlayerPrefs.Save();
            subject.OnNext(settings);
        }

        public IObservable<SettingsModel> OnChange()
        {
            return subject;
        }
    }

    [Serializable]
    public struct SettingsDTO
    {
        public float masterVol;
        public float musicVol;
        public float sfxVol;
        public int quality;
        public bool fullScreen;
        public int difficulty;
    }
}