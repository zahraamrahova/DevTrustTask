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
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public AddressRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Address> GetAddresses()
        {
            return _dbContext.Addresses;
        }
        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _dbContext.Addresses.FindAsync(addressId);
        }
        public async Task<Address> InsertAddressAsync(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }
        public async Task<Address> UpdateAddressAsync(Address address)
        {
            _dbContext.Update(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            _dbContext.Remove(address);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsExistAsync(string Id)
        {
            return await _dbContext.Addresses.AnyAsync(data => data.Id == long.Parse(Id));

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
