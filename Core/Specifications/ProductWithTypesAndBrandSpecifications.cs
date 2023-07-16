using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandSpecifications : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandSpecifications(ProductSpecParams productSpecParams) : base(x =>
        (string.IsNullOrEmpty(productSpecParams.Search)||x.Name.ToLower().Contains(productSpecParams.Search))&&
        (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId) &&(!productSpecParams.TypeId.HasValue||x.ProductTypeId== productSpecParams.TypeId))
        {
            AddIncludes(e => e.productBrand);
            AddIncludes(e => e.ProductType);
            AddOrderBy(x => x.Name);
            ApplyPaging(productSpecParams.PageSize, productSpecParams.PageSize * (productSpecParams.PageIndex - 1));
            if (!string.IsNullOrEmpty(productSpecParams.order))
            {
                switch (productSpecParams.order)
                {
                    case "priceAscendeing":AddOrderBy(x=>x.Price); break;
                    case "priceDescendeing": AddOrderByDescending(x => x.Price); break;
                        default : AddOrderBy(x => x.Name); break;
                }
            }
            
        }
       public ProductWithTypesAndBrandSpecifications(int id):base(x=>x.Id==id)
        {
            AddIncludes(e => e.productBrand);
            AddIncludes(e => e.ProductType);
        }
    }
}
