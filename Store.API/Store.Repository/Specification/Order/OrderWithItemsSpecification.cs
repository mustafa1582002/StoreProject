using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Order
{
    public class OrderWithItemsSpecification : BaseSpecification<Store.Data.Entities.OrderEntities.Order>
    {
        public OrderWithItemsSpecification(string buyerEmail):
            base(order =>order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderWithItemsSpecification(Guid Id,string buyerEmail):
            base(order =>order.BuyerEmail == buyerEmail && order.Id==Id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
