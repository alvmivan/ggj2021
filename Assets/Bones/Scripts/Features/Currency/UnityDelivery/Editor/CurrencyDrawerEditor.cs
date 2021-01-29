using UnityEditor;

namespace Bones.Scripts.Features.Currency.UnityDelivery.Editor
{
    [CustomEditor(typeof(CurrencyDrawer))]
    public class CurrencyDrawerEditor : UnityEditor.Editor
    {
        private CurrencyDrawer Drawer => (CurrencyDrawer) target;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}