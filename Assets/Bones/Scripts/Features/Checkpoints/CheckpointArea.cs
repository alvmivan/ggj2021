using System.Collections.Generic;
using Bones.Scripts.Game;
using Injector.Core;
using UnityEngine;

namespace Bones.Scripts.Features.Checkpoints
{
    public class CheckpointArea : MonoBehaviour
    {
        [SerializeField] private List<GameObject> areaEntities;
        [SerializeField] private int checkpointIndex;

        public void SetAreaAvailable(bool available)
        {
            foreach (var areaEntity in areaEntities)
            {
                if (areaEntity)
                {
                    areaEntity.SetActive(available && areaEntity.activeSelf);
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                Injection.Get<CheckpointsService>().ReachCheckpoint(checkpointIndex);
            }
        }
    }
}