using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Include

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        // Method to Add Includes to Property
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpression.Add(includeExp);
        }

        #endregion

        #region Where

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        #endregion

        #region OrderBy

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
        {
            OrderBy = orderByExp;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExp)
        {
            OrderByDescending = orderByDescExp;
        }

        #endregion

        #region Pagination

        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool isPaginated { get; set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            isPaginated = true;
            Take = pageSize;
            Skip = pageSize * (pageIndex - 1);
        }

        #endregion
    }
}
