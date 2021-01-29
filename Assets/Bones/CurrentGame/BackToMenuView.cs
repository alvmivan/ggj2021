using System;
using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.MainMenu;
using Injector.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CurrentGame
{
    public class BackToMenuView : MonoBehaviour
    {
        public Button button;

        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private void Awake()
        {
            button
                .OnClickAsObservable()
                .Subscribe(_=>Injection.Get<ViewManager>().GetOut("menu"))
                .AddTo(disposables);
        }

        private void OnDestroy()
        {
            disposables.Clear();
        }
    }
}
