using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Context;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace OrderService.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IServiceScopeFactory scopeFactory;

        public GenericRepository(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public IUnitOfWork UnitOfWork => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<OrderDbContext>();

        #region AddAsync
        public virtual async Task<T> AddAsync(T entity)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                await dbContext.Set<T>().AddAsync(entity);
                return entity;
            }
        }
        #endregion
        #region Get
        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                IQueryable<T> query = dbContext.Set<T>();

                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }

                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                    query = orderBy(query);

                return await query.ToListAsync();
            }
        }

        public virtual Task<List<T>> Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            return Get(filter, null, includes);
        }
        #endregion
        #region GetAll
        public virtual async Task<List<T>> GetAll()
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                return await dbContext.Set<T>().ToListAsync();
            }
            
        }
        #endregion
        #region GetById
        public virtual async Task<T> GetById(Guid id)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                return await dbContext.Set<T>().FindAsync(id);
            }
        }

        public async virtual Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                IQueryable<T> query = dbContext.Set<T>();

                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(i => i.Id == id);
            }
        }
        #endregion
        #region GetSingleAsync
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                IQueryable<T> query = dbContext.Set<T>();

                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }

                return await query.Where(expression).SingleOrDefaultAsync();
            }
        }

        #endregion
        #region Update
        public virtual T Update(T entity)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                dbContext.Set<T>().Update(entity);
                return entity;
            }
        }
        #endregion
    }
}
