using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bones.Scripts.Architecture.Context
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] ScriptModule addFirst;
        [SerializeField] List<ScriptModule> scriptModules;
        [SerializeField] ScriptModule addLast;


        private readonly GameModules gameModules = new GameModules();
        
        private void OnValidate()
        {
            if (addFirst)
            {
                var first = addFirst;
                addFirst = null;
                var mods = scriptModules;
                scriptModules = new List<ScriptModule> {first};
                scriptModules.AddRange(mods);
            }

            if (addLast)
            {
                var last = addLast;
                addLast = null;
                scriptModules.Add(last);
            }
        }


        private void Awake()
        {
            gameModules.Declare();
            gameModules.ForEach(module => module.Init());
            scriptModules.ForEach(module => module.Init());
        }
    }
}