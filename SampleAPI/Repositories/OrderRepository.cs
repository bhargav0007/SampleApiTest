using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SampleApiDbContext _context;
        public OrderRepository(SampleApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Recent Orders
        /// </summary>
        /// <returns>List of recent orders</returns>
        public async Task<List<Order>> GetRecentOrders()
        {
            DateTime previousDate = DateTime.Now.AddDays(-1); // One Day Before 
            List<Order> orders = await _context.Orders.Where(x => x.EntryDate > previousDate && x.Status == true).OrderByDescending(x => x.EntryDate).ToListAsync();
            return orders;
        }

        /// <summary>
        /// Add New Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task AddNewOrder(AddOrderDto addOrder)
        {
            try
            {
                Order order = new();
                order.Id = Guid.NewGuid();
                order.DescriptionName = addOrder.DescriptionName;
                order.EntryDate = addOrder.EntryDate;

                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // For time being logs table is not created.
                Console.WriteLine("Issue occurs while adding a new order" + ex.Message);
                throw;
            }

        }
    }
}
