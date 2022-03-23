using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    public class StocksController : Controller
    {
        private readonly AppDbContext _context;

        public StocksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _context.Stocks.ToListAsync());
        }
    }
}
