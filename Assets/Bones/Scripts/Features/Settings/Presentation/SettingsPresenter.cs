using Bones.Scripts.Architecture.MVP;
using Bones.Scripts.Features.Settings.Domain;
using Bones.Scripts.Features.Settings.Services;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Bones.Scripts.Features.Settings.Presentation
{
    [UsedImplicitly]
    public class SettingsPresenter : Presenter
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly SettingsScreen screen;
        private readonly SettingsRepository settingsRepository;

        public SettingsPresenter(SettingsRepository settingsRepository, SettingsScreen screen)
        {
            this.settingsRepository = settingsRepository;
            this.screen = screen;
        }

        public override void OnShow()
        {
            Present();
            BindScreenInput();
        }

        private void Present()
        {
            var settings = settingsRepository.LoadSettings();
            screen.MusicVol.Value = settings.AudioSettings.MusicVol;
            screen.SFXVol.Value = settings.AudioSettings.SFXVol;
            screen.MasterVol.Value = settings.AudioSettings.MasterVol;
            screen.Difficulty.Value = (int) settings.GameSettings.Difficulty;
            screen.Quality.Value = (int) settings.VideoSettings.Quality;
            screen.FullScreen.Value = settings.VideoSettings.FullScreen;
        }

        private void BindScreenInput()
        {
            screen.MusicVol.Subscribe(_ => UpdateRepo()).AddTo(disposables);
            screen.SFXVol.Subscribe(_ => UpdateRepo()).AddTo(disposables);
            screen.MasterVol.Subscribe(_ => UpdateRepo()).AddTo(disposables);
            screen.Difficulty.Subscribe(_ => UpdateRepo()).AddTo(disposables);
            screen.Quality.Subscribe(_ => UpdateRepo()).AddTo(disposables);
            screen.FullScreen.Do(FullScreenChange).Subscribe(_ => UpdateRepo()).AddTo(disposables);
        }

        private void FullScreenChange(bool isFull)
        {
            Screen.fullScreen = isFull;
        }

        private void UpdateRepo()
        {
            var settings = new SettingsModel
            {
                AudioSettings =
                {
                    MusicVol = screen.MusicVol.Value,
                    SFXVol = screen.SFXVol.Value,
                    MasterVol = screen.MasterVol.Value
                },
                GameSettings =
                {
                    Difficulty = (DifficultyLevel) screen.Difficulty.Value
                },
                VideoSettings =
                {
                    Quality = (VideoQuality) screen.Quality.Value, FullScreen = screen.FullScreen.Value
                }
            };
            settingsRepository.SaveSettings(settings);
        }

        public override void OnHide()
        {
            disposables.Clear();
        }
    }
}