using DevTrustApp_NLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.DAL.Interfaces
{
   public interface IAddressRepository
    {
        IQueryable<Address> GetAddresses();
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<Address> InsertAddressAsync(Address address);    
        Task<Address> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(Address address);
        Task<bool> IsExistAsync(string Id);
    }
}
