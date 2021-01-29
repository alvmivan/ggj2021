using System;
using System.Collections.Generic;
using Bones.Scripts.Features.Checkpoints;
using Bones.Scripts.Features.Currency;
using Bones.Scripts.Features.Settings;

namespace Bones.Scripts.Architecture.Context
{
    public class GameModules : List<Module>
    {
        public void Declare()
        {
            Dependency<SettingsModule>();
            Dependency<ScoresModule>();
            Dependency<CheckpointsModule>();
        }
        
        private void Dependency<T>() where T : class, Module, new() => Add(new T());
    }
}