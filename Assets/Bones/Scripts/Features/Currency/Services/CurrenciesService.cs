using System;

namespace Bones.Scripts.Features.Currency.Services
{
    public interface CurrenciesService
    {
        int this[string key] { get; set; }
        IObservable<int> OnCurrencyChange(string currencyName);
    }
}