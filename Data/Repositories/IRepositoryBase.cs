using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestPoint.Data.Models;

namespace TestPoint.Data.Repositories
{
    public interface IRepositoryBase<T> where T : class, IEntity
    {
        Task<T> InsertAsync(T entity);
        Task<T> SelectAsync(Guid id);
        Task<IEnumerable<T>> SelectAllAsync();
        T Update(T entity);
    }
}
