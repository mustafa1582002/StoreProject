using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductWithFilterAndCountSpecification : BaseSpecification<Data.Entities.Product>
    {
        public ProductWithFilterAndCountSpecification(ProductSpecification specification)
           : base(product => (!specification.BrandId.HasValue || product.BrandId == specification.BrandId.Value) &&
                             (!specification.TypeId.HasValue || product.TypeId == specification.TypeId.Value) &&
                             (string.IsNullOrEmpty(specification.Search) || product.Name.Trim().ToLower().Contains(specification.Search)) 
                  )
        {
        }
    }
}
