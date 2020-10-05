using CoreApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CoreApi.Controllers
{
     [Route("api/[controller]")]
    public class ServerController
    {
        private readonly ApiContext _ctx;

        public ServerController(ApiContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = _ctx.Servers.OrderBy(s => s.Id).ToList();
            
              return new ObjectResult(response);
        }

         [HttpGet("{id}", Name="GetServer")]
        public IActionResult Get(int id)
        {
            var response = _ctx.Servers.Find(id);
            
              return new ObjectResult(response);
        }

          [HttpPut("{id}")]
        public IActionResult Message(int id, [FromBody] ServerMessage msg)
        {
            var server = _ctx.Servers.Find(id);
            
            if(server == null)
            {
                return new NotFoundResult();
            }

            if(msg.Payload == "activate")
            {
                server.IsOnline = true;
                
            }

            if(msg.Payload == "deactivate")
            {
                server.IsOnline = false;
                
            }

            _ctx.SaveChanges();
            return new NoContentResult();
        }

        

    }
}