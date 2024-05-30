using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class SpecificationEvaluater<TEntity,TKey>  where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;
            if (specification.Critera is not null) 
                query=query.Where(specification.Critera);

            if (specification.OrderBy is not null) 
                query=query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending is not null) 
                query=query.OrderByDescending(specification.OrderByDescending);

            if(specification.IsPaginated)
                query=query.Skip(specification.Skip).Take(specification.Take);
            
            query=specification.Includes.Aggregate(query, (current,includeExpression)=>current.Include(includeExpression));

            return query;
        }
    }
}
