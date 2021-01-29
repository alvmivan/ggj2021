using Bones.Scripts.Architecture.Context;
using Bones.Scripts.Features.Settings.Services;
using Injector.Core;

namespace Bones.Scripts.Features.Settings
{
    public class SettingsModule : Module
    {
        public void Init()
        {
            Injection.Register<SettingsRepository, LocalSettingsRepository>();
        }
    }
}