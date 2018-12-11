using System.Threading.Tasks;
using Uni.Identity.Web.ViewModels.Consent;

namespace Uni.Identity.Web.Interfaces
{
    /// <summary>
    ///     Вспомогательный сервис для контроллера контроля доступа.
    /// </summary>
    public interface IConsentService
    {
        Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null);

        Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model);
    }
}
