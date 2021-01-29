using Bones.Scripts.Features.Audio.SFX;
using Injector.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Bones.Scripts.Features.Audio.Components
{
    [RequireComponent(typeof(Button))]
    public class SFXButton : MonoBehaviour
    {
        public string sfxKey;

        [Range(0, 1)] public float vol;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private void Awake()
        {
            var button = GetComponent<Button>();
            button
                .OnClickAsObservable()
                .Do(_ => Injection.Get<SFXPlayer>().Play(sfxKey, vol))
                .Subscribe()
                .AddTo(disposables);
        }
    }
}