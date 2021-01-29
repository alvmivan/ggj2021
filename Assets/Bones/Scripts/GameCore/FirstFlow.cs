using System.Collections;
using Bones.Scripts.Architecture.Context;
using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.MainMenu;
using Bones.Scripts.Features.Settings.View;
using CurrentGame;
using Injector.Core;
using UniRx;

namespace Bones.Scripts.GameCore
{

    public enum Flow
    {
        MainMenu,
        Gameplay,
        Settings,
    }
    
    public class FirstFlow : ScriptModule
    {
        public Flow flow;
        
        public override void Init()
        {
            IEnumerator WaitAndDo()
            {
                yield return null;
                var viewManager = Injection.Get<ViewManager>();
                if (flow == Flow.MainMenu)
                    viewManager.Show<MainMenuView>();
                if(flow == Flow.Gameplay)
                    viewManager.Show<CurrentGameMainView>();
                if (flow == Flow.Settings)
                    viewManager.Show<SettingsView>();
            }
            WaitAndDo().ToObservable().Subscribe();
            
        }
    }
}