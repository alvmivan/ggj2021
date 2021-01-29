using Bones.Scripts.Architecture.Context;
using Bones.Scripts.Features.Currency.Infrastructure;
using Bones.Scripts.Features.Currency.Services;
using Injector.Core;

namespace Bones.Scripts.Features.Currency
{
    public class ScoresModule : Module
    {
        public void Init()
        {
            Injection.Register<CurrencyRepository, LocalCurrencyRepository>();
            Injection.Register<CurrenciesService, ReactiveCurrenciesService>();
        }
    }
}