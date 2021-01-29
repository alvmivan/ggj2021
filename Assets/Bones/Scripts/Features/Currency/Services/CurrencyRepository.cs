namespace Bones.Scripts.Features.Currency.Services
{
    public interface CurrencyRepository
    {
        int this[string key] { get; set; }
    }
}
