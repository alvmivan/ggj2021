using System.Collections;
using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.Audio.Music;
using Bones.Scripts.Features.Currency.Services;
using Bones.Scripts.Features.Settings.View;
using CurrentGame;
using GameName;
using Gameplay;
using Injector.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Bones.Scripts.Features.MainMenu
{
    public class MainMenuView : ViewNode
    {
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button newGame;
        [SerializeField] private Button continueGame;

        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private ViewManager viewManager;
        private CurrenciesService currencies;

        protected override void OnInit()
        {
            viewManager = Injection.Get<ViewManager>();
        }

        protected override void OnShow()
        {
            currencies = Injection.Get<CurrenciesService>();
            Injection.Get<MusicPlayer>().Play("menu");

            settingsButton
                .OnClickAsObservable()
                .SelectMany(_ => viewManager.GetOutSwipe("settings"))
                .Subscribe()
                .AddTo(disposables);

            exitButton
                .OnClickAsObservable()
                .Do(_ =>
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                })
                .Subscribe()
                .AddTo(disposables);


            newGame
                .OnClickAsObservable()
                .Do(_ => currencies[GlobalProperties.LevelsCurrencyName] = 0)
                .Select(_ => viewManager.GetOut("game"))
                .Subscribe()
                .AddTo(disposables);

            if (currencies[GlobalProperties.LevelsCurrencyName] > 0)
            {
                continueGame.gameObject.SetActive(true);
                continueGame
                    .OnClickAsObservable()
                    .Select(_ => viewManager.GetOut("game"))
                    .Subscribe()
                    .AddTo(disposables);
            }
            else
            {
                continueGame.gameObject.SetActive(false);
            }
        }


        protected override void OnHide()
        {
            disposables.Clear();
        }
    }
}