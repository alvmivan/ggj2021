using System;
using Bones.Scripts.Features.Currency.Services;
using Gameplay;
using Injector.Core;
using UniRx;
using UnityEngine;
using Wolf;

namespace Fire
{
    public class FireController : MonoBehaviour
    {
        public int maxLevel = 3;
        
        public FireView view;
        private CurrenciesService currencies;

        private int fireLevel;

        private void Awake()
        {
            currencies = Injection.Get<CurrenciesService>();
            currencies
                .OnCurrencyChange(GlobalProperties.LevelsCurrencyName)
                .Subscribe(SetFireLevel);
            SetFireLevel(currencies[GlobalProperties.LevelsCurrencyName]);
        }

        private void SetFireLevel(int level)
        {
            fireLevel = level;
            var visualLevel = Mathf.InverseLerp(-1, maxLevel, level);
            view.SetFireIntensity(visualLevel);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<WolfController>(out var wolf))
            {
                wolf.SetNearFire(false);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<WolfController>(out var wolf))
            {
                wolf.SetNearFire(true);
                wolf.SetHeath(fireLevel);
                var stick = wolf.DropStick();
                
                if (stick)
                {
                    var levelCurrency = currencies[GlobalProperties.LevelsCurrencyName];
                    if (stick.stickLevel > levelCurrency)
                    {
                        currencies[GlobalProperties.LevelsCurrencyName] = stick.stickLevel;
                    }
                }
            }
        }
    }
}