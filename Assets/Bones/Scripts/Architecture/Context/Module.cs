using UnityEngine;

namespace Bones.Scripts.Architecture.Context
{
    public interface Module
    {
        void Init();
    }

    public abstract class ScriptModule : MonoBehaviour, Module
    {
        public abstract void Init();
    }
}