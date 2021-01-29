using Injector.Core;
using UnityEngine;

namespace Bones.Scripts.Features.Checkpoints
{
    
    class CheckpointSpawner : MonoBehaviour
    {
        [SerializeField] 
        private CheckpointArea[] checkPoints;
        
        private CheckpointsService checkpointsService;

        private void Awake()
        {
            Injection.Register(this);
            checkpointsService = Injection.Get<CheckpointsService>();
        }

        public (Vector3 position, Quaternion rotation) LoadCheckpoint(int? checkpointIndex)
        {
            var storedCheckpoint = checkpointIndex ?? checkpointsService.CheckPoint();

            var checkPoint = checkPoints[storedCheckpoint];

            for (var i = 0; i < checkPoints.Length; i++)
            { 
                checkPoints[i].SetAreaAvailable(i >= storedCheckpoint);
            }
            var position = checkPoint.transform.position;
            var rotation = checkPoint.transform.rotation;
            return (position, rotation);
        }
    }
}