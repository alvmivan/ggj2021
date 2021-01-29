using Bones.Scripts.Features.Audio.Core;
using UnityEngine;

namespace Bones.Scripts.Features.Audio.SFX
{
    public class UnitySFXPlayer : MonoBehaviour, SFXPlayer
    {
        [SerializeField] private UnitySFXPlayerPool pool;
        [SerializeField] private ClipsCollection tracks;

        public void Play(string sfxKey, float vol = 1)
        {
            if (!tracks.TryGetClip(sfxKey, out var clip))
            {
                Debug.LogError("You are trying to play sfx : \"" + sfxKey + " \"but there is not matching clips");
                return;
            }

            pool.Play(clip, vol);
        }
    }
}