using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;
using Shared.Abstract;
using Shared.Concrete;
using Shared.Event;
using Shared.Messages;
using Shared.Settings;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISendEndpointProvider _sendEndpoint;

        public OrdersController(AppDbContext context, ISendEndpointProvider sendEndpoint)
        {
            _context = context;
            _sendEndpoint = sendEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order
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

            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var orderCreatedRequestEvent = new OrderCreatedRequestEvent
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage { CardName = orderCreate.Payment.CardName, CardNumber = orderCreate.Payment.CardNumber, CVV = orderCreate.Payment.CVV,
                    Expiration = orderCreate.Payment.Expiration, TotalPrice = orderCreate.OrderItems.Sum(x => x.Price * x.Count)}
            };
            orderCreate.OrderItems.ForEach(item => {
                orderCreatedRequestEvent.OrderItems.Add(new OrderItemMessage { Count = item.Count, ProductId = item.ProductId });
            });

            var sendEndpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue{RabbitMQSettings.OrderSaga}"));

            await sendEndpoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent) ;
            return Ok();
        }

    }
}
