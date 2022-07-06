using DevTrustApp.Core.Queries;
using System.Threading.Tasks;

namespace DevTrustApp.Core.ApplicationService.Interface
{
    public interface IPersonService
    {
         Task<long> Save(string person);
         Task<string> GetAll(GetAllRequest request);

    }
}
