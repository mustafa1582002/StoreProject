using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Data.Migrations;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketService basketService,IMapper mapper,IPaymentService 
            paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket == null)
                throw new Exception("Basket Not Exist");

            var orderItems = new List<OrderItemDto>();

            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);

                if (productItem == null)
                    throw new Exception($"Product with id : {basketItem.ProductId} Not Exist");

                var itemOrdered = new ProductItemOrder
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl,
                };
                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.QuantityPerUnit,
                    ItemOrdered = itemOrdered,
                };
                var mappedOrderedItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderedItem);
            }
            var delivaryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if(delivaryMethod is null)
                throw new Exception("delivaryMethod Not Exist");

            var subtotal = orderItems.Sum(item =>item.Quantity * item.Price);

            //TODO : Check if OrderExist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);

            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order, Guid>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);

            }

            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                DeliveryMethodId = delivaryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BuyerEmail = input.BuyerEmail,
                OrderItems = mappedOrderItems,
                SubTotal = subtotal,
                BasketId = basket.Id,
                PaymentIntentId = basket.PaymentIntentId
            };
            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);

            await _unitOfWork.CompeleteAsync();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(BuyerEmail);

            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationAsync(specs);

            if (orders is { Count: <= 0 })
                throw new Exception("Current User Doesnot Have any Orders Yet");

            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);   
            
            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id,BuyerEmail);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);

            if(order is null)
                throw new Exception($"there is no order with id :{id}");


            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}
