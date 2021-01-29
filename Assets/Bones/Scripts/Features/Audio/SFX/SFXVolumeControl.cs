using Bones.Scripts.Features.Audio.Core;
using Bones.Scripts.Features.Settings.Domain;

namespace Bones.Scripts.Features.Audio.SFX
{
    public class SFXVolumeControl : VolumeControl
    {
        protected override float GetSourceVol(AudioSettingsModel settingsModel)
        {
            return settingsModel.SFXVol;
        }
    }
}