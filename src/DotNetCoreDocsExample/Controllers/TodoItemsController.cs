using System.Collections.Generic;
using System.Linq;
using DotNetCoreApplicationsExample.Data;
using DotNetCoreDocsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreDocsExample.Controllers
{
    [Route("api/[controller]")]
    public class TodoItemsController : Controller
    {
        private readonly AppDbContext _context;
        public TodoItemsController(AppDbContext context)
        {
            _context = context;   
        }
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return _context.TodoItems;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public TodoItem Get(int id)
        {
            return _context.TodoItems.SingleOrDefault(x=> x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            var result = new ObjectResult(todoItem);
            result.StatusCode = 201;
            return result;
        }
    }
}
