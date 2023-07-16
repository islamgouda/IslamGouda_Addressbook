using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> Inputquery, ISpecification<TEntity> spec) {
            var query = Inputquery;
            if (spec.Criteria != null)
            {
                query=query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query=query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderBy(spec.OrderByDescending);
            }
            if (spec.isPagingEnabled)
            {
                query = query.Skip(spec.skip).Take(spec.take);
            }
            
            query =spec.Includes.Aggregate(query,(current,Include)=>current.Include(Include));
            return query;
        }
    }
}
