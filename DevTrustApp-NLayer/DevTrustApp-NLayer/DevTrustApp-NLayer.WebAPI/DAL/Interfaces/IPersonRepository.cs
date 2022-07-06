using DevTrustApp_NLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.DAL.Interfaces
{
    public interface IPersonRepository : IDisposable 
    {
        IQueryable<Person> GetPersons();
        Task<Person> GetPersonByIdAsync(int personId);
        Task<Person> InsertPersonAsync(Person person);  
        Task<Person> UpdatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(Person person);       
        Task<bool> IsExistAsync(string Id);
       
    }
}
