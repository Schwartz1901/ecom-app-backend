using Order.Domain.Aggregates.Entities;
using Order.Domain.Aggregates.Enumerations;
using Order.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates
{
    public class Order : Entity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string Description { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        private int _orderStatusId;

        private List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItem => _orderItems;
        public Address Address { get; private set; }

        private Order() 
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(Guid userId, Address address, DateTime orderDate, string description, OrderStatus orderStatus, List<OrderItem> orderItems )
        {
            UserId = userId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            OrderDate = orderDate;
            Description = description;
            OrderStatus = orderStatus ?? throw new ArgumentNullException(nameof(orderStatus));
            _orderStatusId = orderStatus.Id;

            _orderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
        }


        public void UpdateStatus(OrderStatus status)
        {
            OrderStatus = status;
            _orderStatusId = status.Id;
        }

        public double GetTotal() => _orderItems.Sum(i => i.GetCurrentPrice());
    }
}
 