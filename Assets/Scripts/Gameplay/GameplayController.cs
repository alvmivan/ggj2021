using UnityEngine;

namespace Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        private void Awake()
        {
            GlobalProperties.PlayerInputBlocked.Value = false;
        }
    }
}