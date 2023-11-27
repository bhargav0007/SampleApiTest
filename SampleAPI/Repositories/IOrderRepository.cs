using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetRecentOrders();

        public Task AddNewOrder(AddOrderDto order);
    }
}
