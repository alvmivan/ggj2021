using Bones.Scripts.Features.Currency.Services;
using Injector.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
    [RequireComponent(typeof(Button))]
    public class ResetCurrencyButton : MonoBehaviour
    {
        public string currencyName;
        private void Start()
        {
            var service = Injection.Get<CurrenciesService>();
            gameObject
                .GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ => service[currencyName]=0);
        }
    }
}