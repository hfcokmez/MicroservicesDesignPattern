using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Model;
using Shared.Event;
using Shared.Messages;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(AppDbContext context, IPublishEndpoint publish)
        {
            _context = context;
            _publishEndpoint = publish;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Model.Order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address { Line = orderCreate.Address.Line, District = orderCreate.Address.District, Province = orderCreate.Address.Province },
                CreatedDate = DateTime.Now
            };

            orderCreate.OrderItems.ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem { Price = item.Price, ProductId = item.ProductId, Count = item.Count });
            });

            //await _context.AddAsync(newOrder);
            //await _context.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                PaymentMessage = new PaymentMessage { CardName = orderCreate.Payment.CardName, CardNumber = orderCreate.Payment.CardNumber, CVV = orderCreate.Payment.CVV,
                    Expiration = orderCreate.Payment.Expiration, TotalPrice = orderCreate.OrderItems.Sum(x => x.Price * x.Count)}
            };
            orderCreate.OrderItems.ForEach(item => {
                orderCreatedEvent.OrderItems.Add(new OrderItemMessage { Count = item.Count, ProductId = item.ProductId });
            });
            await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok();
        }

    }
}
