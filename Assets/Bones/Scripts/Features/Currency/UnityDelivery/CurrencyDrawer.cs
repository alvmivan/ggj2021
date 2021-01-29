using System;
using Bones.Scripts.Features.Currency.Services;
using Injector.Core;
using TMPro;
using UnityEngine;
using UniRx;

namespace Bones.Scripts.Features.Currency.UnityDelivery
{
    public class CurrencyDrawer : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public string currencyName;
        
        private CurrenciesService currenciesService;

        private void Start()
        {
            currenciesService = Injection.Get<CurrenciesService>();
            currenciesService
                .OnCurrencyChange(currencyName)
                .ObserveOnMainThread()
                .Subscribe(OnCurrencyChange);
            OnCurrencyChange(currenciesService[currencyName]);
        }

        private void OnCurrencyChange(int newValue)
        {
            label.text = newValue.ToString();
        }
    }
}