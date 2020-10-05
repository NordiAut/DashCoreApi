using CoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace CoreApi.Controllers
{
    
    [Route("api/[controller]")]
    public class CustomerController
    {
        
        private readonly ApiContext _ctx;

        public CustomerController(ApiContext ctx)
        {
            _ctx = ctx;
        }
        
        // GET api/customer
        [HttpGet]
        public IActionResult Get()
        {
            var data = _ctx.Customers.OrderBy(c => c.Id);            

              return new ObjectResult(data);

        }

        // GET api/customer/id
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
           var customer = _ctx.Customers.Find(id);

           return new ObjectResult(customer);
        }

        
        // GET api/customer/id
        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
          if(customer == null){
              return new BadRequestResult();
          }

          _ctx.Customers.Add(customer);
          _ctx.SaveChanges();

          return new CreatedAtRouteResult("GetCustomer", new {id = customer.Id}, customer);

        } 
    }
}