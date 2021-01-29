namespace Bones.Scripts.Features.Audio.Music
{
    public interface MusicPlayer
    {
        void Play(string trackId, float fadeDuration = 0);
        void Stop();
    }
}