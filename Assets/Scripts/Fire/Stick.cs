using System;
using Bones.Scripts.Features.Currency;
using Bones.Scripts.Features.Currency.Services;
using Gameplay;
using Injector.Core;
using UniRx;
using UnityEngine;
using Wolf;

namespace Fire
{
    public class Stick : MonoBehaviour
    {
        public int stickLevel;

        public Transform stickRender;

        private void Start()
        {
            var currenciesService = Injection.Get<CurrenciesService>();
            
            currenciesService
                .OnCurrencyChange(GlobalProperties.LevelsCurrencyName)
                .Subscribe(UpdateData);
            UpdateData(currenciesService[GlobalProperties.LevelsCurrencyName]);
        }

        private void UpdateData(int amount)
        {
            if (stickLevel <= amount)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<WolfController>(out var wolf))
            {
                wolf.GrabStick(this);
            }
        }

        public void OnDrop()
        {
            
        }
    }
}
