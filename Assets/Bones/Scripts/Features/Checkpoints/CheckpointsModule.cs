using Bones.Scripts.Architecture.Context;
using Injector.Core;

namespace Bones.Scripts.Features.Checkpoints
{
    public class CheckpointsModule : Module
    {
        public void Init()
        {
            Injection.Register<CheckpointsService>();
        }
    }
}