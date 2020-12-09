using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(object id);
        void Insert(T obj);
        ValueTask<EntityEntry<T>> InsertAsync(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        Task<int> SaveAsync();
    }
}
