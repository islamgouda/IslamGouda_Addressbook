using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T,bool>> creteria)
        {
            this.Criteria = creteria;  
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; }=new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int skip { get; private set; }

        public int take { get; private set; }

        public bool isPagingEnabled { get; private set; }
        protected void ApplyPaging(int Take,int Skip)
        {
            skip = Skip;
            take = Take;
            isPagingEnabled = true;
        }
        protected void AddIncludes(Expression<Func<T,object>> Includexpression) {
        Includes.Add(Includexpression);
        }
        protected void AddOrderBy(Expression<Func<T, object>> OrderByxpression) {
            OrderBy=OrderByxpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> OrderByDescendingxpression)
        {
            OrderByDescending = OrderByDescendingxpression;
        }
    }
}
