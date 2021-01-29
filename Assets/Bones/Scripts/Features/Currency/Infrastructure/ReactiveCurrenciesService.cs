using System;
using System.Collections.Generic;
using Bones.Scripts.Features.Currency.Services;
using JetBrains.Annotations;
using UniRx;

namespace Bones.Scripts.Features.Currency.Infrastructure
{
    [UsedImplicitly]
    public class ReactiveCurrenciesService : CurrenciesService 
    {
        private readonly Dictionary<string, IReactiveProperty<int>> properties = new Dictionary<string, IReactiveProperty<int>>();

        private readonly CurrencyRepository currencyRepository;

        public ReactiveCurrenciesService(CurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public IObservable<int> OnCurrencyChange(string currencyName)
        {
            return GetProperty(currencyName);
        }

        private IReactiveProperty<int> GetProperty(string key)
        {
            if (properties.TryGetValue(key, out var prop)) return prop;
            prop = new ReactiveProperty<int>();
            properties[key] = prop;
            prop.Value = currencyRepository[key];
            return prop;
        }

        public int this[string key]
        {
            get => currencyRepository[key];
            set
            {
                currencyRepository[key] = value;
                GetProperty(key).Value = value;
            }
        }
    }
}