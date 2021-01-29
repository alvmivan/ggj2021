using Bones.Scripts.Features.Currency.Services;
using JetBrains.Annotations;

namespace Bones.Scripts.Features.Checkpoints
{
    [UsedImplicitly]
    public class CheckpointsService
    {

        private readonly CurrenciesService currenciesService;

        public CheckpointsService(CurrenciesService currenciesService)
        {
            this.currenciesService = currenciesService;
        }
        
        public void ReachCheckpoint(int checkpointIndex)
        {
            var current = currenciesService["checkpoints"];
            if (checkpointIndex > current)
            {
                currenciesService["checkpoints"] = checkpointIndex;
            }
        }
        
        public int CheckPoint() => currenciesService["checkpoints"];

        public void Reset(int resetTo =0)
        {
            currenciesService["checkpoints"] = resetTo;
        }
    }
}