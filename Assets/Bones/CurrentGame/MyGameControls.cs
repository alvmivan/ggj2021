using Bones.Scripts.Architecture.ViewManager;
using Bones.Scripts.Features.MainMenu;
using Injector.Core;

namespace GameName
{
    public class MyGameControls : GameControls
    {
        public void BackToMenu()
        {
            Injection.Get<ViewManager>().Show<MainMenuView>();
        }
    }
}