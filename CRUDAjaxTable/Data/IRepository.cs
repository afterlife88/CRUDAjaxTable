using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDAjaxTable.Data
{
    public interface IRepository<T> :  IDisposable where T : class
    {
        Task <IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T operation);
        Task<T> UpdateAsync(T item);
        Task<object> Delete(int id);
    }
}