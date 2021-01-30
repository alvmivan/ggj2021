using System;
using UniRx;

namespace Gameplay
{
    public static class GlobalProperties
    {
        public static readonly IReactiveProperty<bool> PlayerInputBlocked = new ReactiveProperty<bool>(false);
        
        
        public const string LevelsCurrencyName = "CurrentLevel";
        
    }
    
    
}