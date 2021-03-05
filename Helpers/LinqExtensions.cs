using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace wordmeister_api.Helpers
{
    public static class LinqExtensions
    {
        public static System.Linq.IQueryable<T> WhereIf<T>(this System.Linq.IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }

        public static System.Linq.IQueryable<T> WhereIf<T>(this System.Linq.IQueryable<T> query, bool condition, string predicate)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }
    }
}
