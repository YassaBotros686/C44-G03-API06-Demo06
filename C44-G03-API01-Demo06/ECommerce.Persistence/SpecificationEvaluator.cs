using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public class SpecificationEvaluator
    {
        // Method to Create Query
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint;

            if (specification is not null)
            {
                // Where
                if(specification.Criteria is not null)
                {
                    Query = Query.Where(specification.Criteria);
                }

                // Include
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    //// Inject for Include
                    //foreach (var includeExp in specification.IncludeExpression)
                    //{
                    //    Query = Query.Include(includeExp);
                    //    // _dbContext.Products.Include(............)
                    //}

                    Query = specification.IncludeExpression
                                 .Aggregate(Query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
                }

                // OrderBy
                if (specification.OrderBy is not null)
                {
                    Query = Query.OrderBy(specification.OrderBy);
                }
                else if (specification.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specification.OrderByDescending);
                }

                // Pagination
                if (specification.isPaginated)
                {
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }
            }
            return Query;
        }
    }
}
