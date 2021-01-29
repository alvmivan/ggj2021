using Bones.Scripts.Architecture.MVP;
using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.MainMenu;
using Bones.Scripts.Features.Settings.Presentation;
using Bones.Scripts.Shared.View;
using Injector.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Bones.Scripts.Features.Settings.View
{
    public class SettingsView : ViewNode, SettingsScreen
    {
        [SerializeField] private ReactiveDropdown difficultyLevelDropdown;
        [SerializeField] private ReactiveDropdown videoQualityLevelDropdown;
        [SerializeField] private ReactiveSlider mainVolumeSlider;
        [SerializeField] private ReactiveSlider sfxVolumeSlider;
        [SerializeField] private ReactiveSlider musicVolumeSlider;
        [SerializeField] private ReactiveToggle fullScreenToggle;

        [SerializeField] private Button backButton;

        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private Presenter presenter;

        private ViewManager viewManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) GoBack();
        }

        public IReactiveProperty<float> MusicVol => musicVolumeSlider;
        public IReactiveProperty<float> SFXVol => sfxVolumeSlider;
        public IReactiveProperty<float> MasterVol => mainVolumeSlider;
        public IReactiveProperty<int> Difficulty => difficultyLevelDropdown;
        public IReactiveProperty<int> Quality => videoQualityLevelDropdown;
        public IReactiveProperty<bool> FullScreen => fullScreenToggle;

        protected override void OnInit()
        {
            viewManager = Injection.Get<ViewManager>();
            Injection.Register((SettingsScreen) this);
            presenter = Injection.Create<SettingsPresenter>();
            presenter.OnInit();
            Debug.Log("asdsad");
        }

        protected override void OnShow()
        {
            presenter.OnShow();

            backButton
                .OnPointerClickAsObservable()
                .SelectMany(_=>viewManager.ShowSwipe<MainMenuView>(0.2f, Vector2.right))
                .Subscribe()
                .AddTo(disposables);
        }

        private void GoBack()
        {
            viewManager.Show<MainMenuView>();
        }

        protected override void OnHide()
        {
            presenter.OnHide();
            disposables.Clear();
        }
    }
}