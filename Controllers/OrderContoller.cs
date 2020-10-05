using CoreApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


namespace CoreApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderController
    {
        private readonly ApiContext _ctx;

        public OrderController(ApiContext ctx)
        {
            _ctx = ctx;
        }

        // GET api/order
        [HttpGet]
        public IActionResult Get()
        {
            var data = _ctx.Orders.OrderBy(c => c.Id);            

              return new ObjectResult(data);

        }

        // Get api/order/pageNumber/pageSize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get( int pageIndex, int pageSize)
        {
            var data = _ctx.Orders.Include(o => o.Customer).OrderByDescending(c => c.Placed);

            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);
            
            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return new ObjectResult(response);
        }

       
        [HttpGet("ByState")]
        public IActionResult ByState( )
        {
            // Get every orders with customers
            var orders = _ctx.Orders.Include(o => o.Customer).ToList();

            // Get every orders grouped by state
            var groupedResult = orders.GroupBy(o => o.Customer.State).ToList()
            //Create a new object from the grouping with the state and the number of total states
            .Select(Grp =>new 
            {
                State = Grp.Key,
                Total = Grp.Sum(x => x.OrderTotal)
            })
            .OrderByDescending(res => res.Total).ToList();

            return new ObjectResult(groupedResult);

        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n)
        {
            // Get every orders with customers
            var orders = _ctx.Orders.Include(o => o.Customer).ToList();

            // Get every orders grouped by customer id
            var groupedResult = orders.GroupBy(o => o.Customer.Id).ToList()
            //Create a new object from the grouping with the customername and total orders
            .Select(Grp =>new 
            {
                Name = _ctx.Customers.Find(Grp.Key).Name,
                Total = Grp.Sum(x => x.OrderTotal)
            })
            .OrderByDescending(res => res.Total)
            .Take(n)
            .ToList();

            return new ObjectResult(groupedResult);
        }

        [HttpGet("GetOrder/{id}", Name="GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _ctx.Orders.Include(o => o.Customer).First(o => o.Id == id);
            return new ObjectResult(order);
        }


    }
}