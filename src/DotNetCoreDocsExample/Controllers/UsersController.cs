using System.Collections.Generic;
using System.Linq;
using DotNetCoreApplicationsExample.Data;
using DotNetCoreDocsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreDocsExample.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;   
        }
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _context.Users.SingleOrDefault(x=> x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]User User)
        {
            _context.Users.Add(User);
            _context.SaveChanges();

            var result = new ObjectResult(User);
            result.StatusCode = 201;
            return result;
        }
    }
}
