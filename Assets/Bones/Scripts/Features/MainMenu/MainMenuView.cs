using System.Collections;
using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.Audio.Music;
using Bones.Scripts.Features.Settings.View;
using CurrentGame;
using GameName;
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
        [SerializeField] private Button startGame;

        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private ViewManager viewManager;

        protected override void OnInit()
        {
            viewManager = Injection.Get<ViewManager>();
        }

        protected override void OnShow()
        {
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
            startGame
                .OnClickAsObservable()
                .Select(_ => viewManager.GetOut("game"))
                .Subscribe()
                .AddTo(disposables);
        }


        protected override void OnHide()
        {
            disposables.Clear();
        }
    }
}