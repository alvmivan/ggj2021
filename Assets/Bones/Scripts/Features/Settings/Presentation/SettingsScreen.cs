using UniRx;

namespace Bones.Scripts.Features.Settings.Presentation
{
    public interface SettingsScreen
    {
        IReactiveProperty<float> MusicVol { get; }
        IReactiveProperty<float> SFXVol { get; }
        IReactiveProperty<float> MasterVol { get; }
        IReactiveProperty<int> Difficulty { get; }
        IReactiveProperty<int> Quality { get; }
        IReactiveProperty<bool> FullScreen { get; }
    }
}