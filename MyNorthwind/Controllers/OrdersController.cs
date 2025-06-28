using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNorthwind.Models;

namespace MyNorthwind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly NorthwindContext _db;
    public OrdersController(NorthwindContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> Get() =>
        await _db.Orders.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        var c = await _db.Orders
            .FirstOrDefaultAsync();

        if (c == null) return NotFound();
        return c;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Post(Order c)
    {
        _db.Orders.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.OrderId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Order c)
    {
        // if (id != c.OrderId) return BadRequest();
        _db.Entry(c).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var c = await _db.Orders.FindAsync(id);
        if (c == null) return NotFound();
        _db.Orders.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}