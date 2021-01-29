using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bones.Scripts.Features.ExploreFiles
{
    
    public class DataButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI label;
        
        public string Text
        {
            get => label.text;
            set => label.text = value;
        }

        public RectTransform RectTransform => transform as RectTransform;

        public void OnCLick(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
    }
}