using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetFiltered(Expression<Func<T, bool>> expression, Filter filter, out int totalNumberOfPages);

        void Add(T entity);

        void Delete(T entity);

        long Count();

        long Count(Expression<Func<T, bool>> expression);

        List<T> Find(Expression<Func<T, bool>> expression);

        TResult Min<TResult>(Expression<Func<T, TResult>> expression);

        TResult Max<TResult>(Expression<Func<T, TResult>> expression);

        T Single(Expression<Func<T, bool>> expression);

        T SingleOrDefault(Expression<Func<T, bool>> expression);

        IEnumerable<T> Find(Expression<Func<T, bool>> expression, int skip, int take);
    }
}
