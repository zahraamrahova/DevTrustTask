using DevTrustApp_NLayer.WebAPI.Models.Requests;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.BL.Interfaces
{
    public interface IPersonBL
    {
         Task<long> Save(string person);
         Task<string> GetAll(GetAllRequest request);
    }
}
