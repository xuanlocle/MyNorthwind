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
    public async Task<ActionResult<IEnumerable<Customer>>> Get(
        int pageNumber = 1,
        int pageSize = 10)
    {
        // Validate pageNumber and pageSize
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        // Total number of customers
        var totalCustomers = await _db.Customers.CountAsync();

        // Get the paginated customers along with related Orders
        var customers = await _db.Customers
            .Include(c => c.Orders) // Include Orders if needed
            .Skip((pageNumber - 1) * pageSize) // Skip the records for previous pages
            .Take(pageSize) // Take only the number of records for the current page
            .ToListAsync();

        // Return pagination information with the customers
        var paginationMetadata = new
        {
            TotalCount = totalCustomers,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling((double)totalCustomers / pageSize)
        };

        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(string id)
    {
        // Use Include to load related Orders for the Customer
        var c = await _db.Customers
            .Include(c => c.Orders) // Include related Orders
            .FirstOrDefaultAsync(c => c.CustomerId == id); // Fetch by CustomerId

        // If customer is not found, return NotFound
        if (c == null)
        {
            return NotFound(new { message = "Customer not found." }); // Custom message in NotFound response
        }

        // Return the Customer object (it will be serialized as JSON)
        return Ok(c);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Post(Customer c)
    {
        // Check if CustomerId already exists in the database
        if (_db.Customers.Any(cust => cust.CustomerId == c.CustomerId))
        {
            return BadRequest("Customer with this ID already exists.");
        }

        // Add the new customer (with orders if any)
        _db.Customers.Add(c);

        // Optionally, validate orders or perform other checks (e.g., validate if orders belong to this customer)
        foreach (var order in c.Orders)
        {
            order.CustomerId = c.CustomerId; // Ensure the order is linked to the customer
            _db.Orders.Add(order);
        }

        await _db.SaveChangesAsync();

        // Return the created customer with the status code 201 and the URI to get the customer by ID
        return CreatedAtAction(nameof(Get), new { id = c.CustomerId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Customer c)
    {
        // Check if the provided ID in the URL matches the Customer's ID
        if (id != c.CustomerId)
        {
            return BadRequest("Customer ID in the URL does not match the Customer object.");
        }

        // Check if the customer exists in the database
        var existingCustomer = await _db.Customers.FindAsync(id);
        if (existingCustomer == null)
        {
            return NotFound("Customer not found.");
        }

        // Update the existing customer with new values
        _db.Entry(existingCustomer).CurrentValues.SetValues(c);

        // Save changes to the database
        await _db.SaveChangesAsync();

        // Return NoContent status (successful update, but no data to return)
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        // Check if the customer exists
        var customer = await _db.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        // Remove the customer
        _db.Customers.Remove(customer);

        // Save changes to the database
        await _db.SaveChangesAsync();

        // Return NoContent status (successful deletion, but no data to return)
        return NoContent();
    }
}