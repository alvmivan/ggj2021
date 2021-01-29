using Bones.Scripts.Architecture.Context;
using Bones.Scripts.Features.Audio.Music;
using Bones.Scripts.Features.Audio.SFX;
using Injector.Core;
using UnityEngine;

namespace Bones.Scripts.Features.Audio
{
    public class AudioModule : ScriptModule
    {
        [SerializeField] private UnityMusicPlayer musicPlayer;
        [SerializeField] private UnitySFXPlayer sfxPlayer;

        private void OnValidate()
        {
            musicPlayer.gameObject.SetActive(false);
            sfxPlayer.gameObject.SetActive(false);
        }

        public override void Init()
        {
            musicPlayer.gameObject.SetActive(true);
            sfxPlayer.gameObject.SetActive(true);
            Injection.Register<MusicPlayer>(musicPlayer);
            Injection.Register<SFXPlayer>(sfxPlayer);
        }
    }
}