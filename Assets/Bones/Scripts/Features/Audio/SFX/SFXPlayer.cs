namespace Bones.Scripts.Features.Audio.SFX
{
    public interface SFXPlayer
    {
        void Play(string sfxKey, float vol = 1);
    }
}