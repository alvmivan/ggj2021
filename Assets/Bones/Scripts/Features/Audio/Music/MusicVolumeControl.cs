using Bones.Scripts.Features.Audio.Core;
using Bones.Scripts.Features.Settings.Domain;

namespace Bones.Scripts.Features.Audio.Music
{
    public class MusicVolumeControl : VolumeControl
    {
        protected override float GetSourceVol(AudioSettingsModel settingsModel)
        {
            return settingsModel.MusicVol;
        }
    }
}