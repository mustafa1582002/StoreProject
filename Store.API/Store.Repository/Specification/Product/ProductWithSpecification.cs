using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductWithSpecification : BaseSpecification<Data.Entities.Product>
    {
        public ProductWithSpecification(ProductSpecification specification)
            : base(product => (!specification.BrandId.HasValue || product.BrandId == specification.BrandId.Value) &&
                             (!specification.TypeId.HasValue || product.TypeId == specification.TypeId.Value) &&
                             (string.IsNullOrEmpty(specification.Search) || product.Name.Trim().ToLower().Contains(specification.Search))
                  )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderBy(x => x.Name);
            ApplyPagination(specification.PageSize * (specification.PageIndex - 1),specification.PageSize);

            if (!string.IsNullOrEmpty(specification.Sort))
            {
                switch (specification.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x=>x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x=>x.Price);
                        break;
                    default:
                        AddOrderBy(x=>x.Name);
                        break;
                    
                }
            }
        }
        public ProductWithSpecification(int? id)    : base(product => product.Id == id)
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
    }
}
