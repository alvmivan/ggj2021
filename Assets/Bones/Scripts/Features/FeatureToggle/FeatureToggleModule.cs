using Bones.Scripts.Architecture.Context;
using Injector.Core;

namespace Bones.Scripts.Features.FeatureToggle
{
    public class FeatureToggleModule : Module
    {
        public void Init()
        {
            Injection.Register<FeatureService, LocalFeatureService>();
        }
    }
}