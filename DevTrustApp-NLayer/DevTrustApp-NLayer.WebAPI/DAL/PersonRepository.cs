using DevTrustApp_NLayer.Entities;
using DevTrustApp_NLayer.WebAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.DAL
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Person> GetPersons()
        {
            return _dbContext.Persons;
        }
        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            return await _dbContext.Persons.FindAsync(personId);
        }
        public async Task<Person> InsertPersonAsync(Person person)
        {
            await _dbContext.Persons.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            _dbContext.Update(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }
        public async Task<bool> DeletePersonAsync(Person person)
        {
            _dbContext.Remove(person);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> IsExistAsync(string Id)
        {
            return await _dbContext.Persons.AnyAsync(data => data.Id == long.Parse(Id));

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
