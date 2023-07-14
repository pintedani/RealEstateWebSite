using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
    using System.Data.Entity;

    using Imobiliare.Entities;

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext, SortSpec defaultSortSpec)
        {
            this.IncludeList = new List<string>();
            this.DbContext = dbContext;
            this.SortSpec = defaultSortSpec;
        }

        public List<TEntity> GetFiltered(Expression<Func<TEntity, bool>> expression, Filter filter, out int totalNumberOfPages)
        {
            var sortingPropertyName = this.SortSpec.PropertyName;
            var results = this.AddIncludes(this.DbContext.Set<TEntity>().Where(expression).OrderBy($"{sortingPropertyName} {SortSpec.SortDirection}"));

            totalNumberOfPages = results.Count();
            return results.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        }

        /// <summary>
        /// Gets or sets the sort directions.
        /// </summary>
        /// <value>The sort directions.</value>
        public SortSpec SortSpec { get; set; }

        /// <summary>
        ///   Gets the total count of entities in the database.
        /// </summary>
        /// <returns> Number of entities. </returns>
        public long Count()
        {
            long count = this.DbContext.Set<TEntity>().Count();
            return count;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Gets the minimum value given the specified expression.
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The minimum value.</returns>
        public TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            var min = this.DbContext.Set<TEntity>().Min(expression);
            return min;
        }

        /// <summary>
        /// Gets the maximum value given the specified expression.
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The maximum value.</returns>
        public virtual TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            var max = this.DbContext.Set<TEntity>().Max(expression);
            return max;
        }

        /// <summary>
        /// Gets the total count of entities in the database that satisfy the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// Number of entities.
        /// </returns>
        public long Count(Expression<Func<TEntity, bool>> expression)
        {
            long count = this.DbContext.Set<TEntity>().Count(expression);
            return count;
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// true if any elements in the source sequence pass the test in the specified expression; otherwise, false.
        /// </returns>
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            var result = this.DbContext.Set<TEntity>().Any(expression);
            return result;
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> expression)
        {
            var entity = this.AddIncludes(this.DbContext.Set<TEntity>()).Single(expression);
            return entity;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            var entity = this.AddIncludes(this.DbContext.Set<TEntity>()).SingleOrDefault(expression);
            return entity;
        }

        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            var sortingPropertyName = this.SortSpec.PropertyName;
            var entities = this.AddIncludes(this.DbContext.Set<TEntity>()).Where(expression).OrderBy($"{sortingPropertyName} {SortSpec.SortDirection}").ToList();
            return entities;
        }

        //Maybe problems with includelist, only some dependencies included
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression, int skip, int take)
        {
            var sortingPropertyName = this.SortSpec.PropertyName;
            var entities = this.DbContext.Set<TEntity>().Where(expression).OrderBy($"{sortingPropertyName} {SortSpec.SortDirection}").Skip(skip).Take(take).ToList();

            return entities.AsEnumerable();
        }

        protected List<string> IncludeList { get; private set; }

        protected virtual IQueryable<TEntityDerived> AddIncludes<TEntityDerived>(IQueryable<TEntityDerived> dbSet) where TEntityDerived : TEntity
        {
            var dbQuery = dbSet;

            foreach (var include in this.IncludeList)
            {
                dbQuery = dbQuery.Include(include);
            }

            return dbQuery;
        }
    }
}
