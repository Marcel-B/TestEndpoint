using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestPoint.Data.Models;

namespace TestPoint.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {
        public ImageContext Db { get; }
        public ILogger<T> Logger { get; }

        protected RepositoryBase(
            ImageContext db,
            ILogger<T> logger)
        {
            Db = db;
            Logger = logger;
        }

        public async Task<T> InsertAsync(
            T entity)
        {
            entity.Created = DateTime.Now;
            var result = await Db.Set<T>().AddAsync(entity);
            _ = Db.SaveChanges();
            return result.Entity;
        }

        public async Task<IEnumerable<T>> SelectAllAsync()
        {
            return await Db.Set<T>().ToListAsync();
        }

        public async Task<T> SelectAsync(
            Guid id)
        {
            var result = await Db.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
            return result;
        }

        public T Update(
            T entity)
        {
            var result = Db.Set<T>().Update(entity);
            _ = Db.SaveChanges();
            return result.Entity;
        }
    }
}
