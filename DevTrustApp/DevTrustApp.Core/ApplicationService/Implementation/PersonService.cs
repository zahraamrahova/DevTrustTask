using CustomJSON;
using DevTrustApp.Core.ApplicationService.Interface;
using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using DevTrustApp.Core.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrustApp.Core.ApplicationService.Implementation
{
    public class PersonService : IPersonService
    {
        private readonly IPersonWriteRepository _personWriteRepository;
        private readonly IPersonReadRepository _personReadRepository;
        private readonly IAddressWriteRepository _addressWriteRepository;

        public object MyCustomSerilizer { get; private set; }

        public PersonService(IPersonWriteRepository personWriteRepository, IPersonReadRepository personReadRepository, IAddressWriteRepository addressWriteRepository)
        {
            _personWriteRepository = personWriteRepository;
            _personReadRepository = personReadRepository;
            _addressWriteRepository = addressWriteRepository;
        }


        public async Task<long> Save(string personString)
        {
            var person = JSONSerDer.Deserialize<Person>(personString);           

            Person result;
            if (!(await _personReadRepository.IsExistAsync(person.Id.ToString())))
            {
                result = await _personWriteRepository.AddAsync(person);
                await _personWriteRepository.SaveAsync();
                return result.Id;
            }

            _addressWriteRepository.Update(person.Address);
            var updatedPerson = _personWriteRepository.Update(person);
            await _personWriteRepository.SaveAsync();


            return updatedPerson.Id;
        }

        public async Task<string> GetAll(GetAllRequest request)
        {
            var query = _personReadRepository.GetAll();

            if (request != null)
            {
                if (request.FirstName != null)
                {
                    query = query.Where(x => x.FirstName == request.FirstName);
                }
                if (request.LastName != null)
                {
                    query = query.Where(x => x.LastName == request.LastName);
                }
                if (request.City != null)
                {
                    query = query.Where(x => x.Address.City == request.City);
                }
            }

            var peopleList = await query.Include(p => p.Address).ToListAsync();
            return JSONSerDer.Serialize(peopleList);
        }
    }
}
