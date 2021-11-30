using System.Threading.Tasks;

namespace FiscalCalendar.Api.Secrets.Services
{
    public interface ISecretService
    {
        Task<string> GetSecretValue(string secretName);
    }
}
