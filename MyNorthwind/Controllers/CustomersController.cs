using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNorthwind.Models;

namespace MyNorthwind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly NorthwindContext _db;
    public CustomersController(NorthwindContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> Get() =>
        await _db.Customers.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(string id)
    {
        var c = await _db.Customers
            .Include(c=>c.Orders)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (c == null) return NotFound();
        return c;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Post(Customer c)
    {
        _db.Customers.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.CustomerId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Customer c)
    {
        if (id != c.CustomerId) return BadRequest();
        _db.Entry(c).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var c = await _db.Customers.FindAsync(id);
        if (c == null) return NotFound();
        _db.Customers.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}