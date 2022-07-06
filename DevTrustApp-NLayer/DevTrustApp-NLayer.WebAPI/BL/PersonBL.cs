using DevTrustApp_NLayer.Entities;
using DevTrustApp_NLayer.WebAPI.BL.Interfaces;
using DevTrustApp_NLayer.WebAPI.DAL.Interfaces;
using DevTrustApp_NLayer.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCustomJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.BL
{
    public class PersonBL : IPersonBL
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAddressRepository _addressRepository;

        public PersonBL(IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _addressRepository = addressRepository;
        }

        public async Task<long> Save(string personString)
        {
            var person = JSONSerDer.Deserialize<Person>(personString);

            Person result;
            if (!(await _personRepository.IsExistAsync(person.Id.ToString())))
            {
                result = await _personRepository.InsertPersonAsync(person);
                return result.Id;
            }
            await _addressRepository.UpdateAddressAsync(person.Address);
            var updatedPerson = await _personRepository.UpdatePersonAsync(person);
            return updatedPerson.Id;
        }

        public async Task<string> GetAll(GetAllRequest request)
        {
            var query = _personRepository.GetPersons();

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

            var peopleList = query.Include(p => p.Address).ToList();
            return JSONSerDer.Serialize(peopleList);
        }


    }
}
