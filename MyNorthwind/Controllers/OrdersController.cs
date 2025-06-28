using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNorthwind.Models;
using MyNorthwind.Services;

namespace MyNorthwind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly NorthwindContext _db;

    public OrdersController(NorthwindContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> Get(
        int pageNumber = 1,
        int pageSize = 10)
    {
        // Validate pageNumber and pageSize
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        // Total number of orders
        var totalOrders = await _db.Orders.CountAsync();

        // Get the paginated orders with related data (e.g., Customer)
        var orders = await _db.Orders
            .Include(o => o.Customer) // If you want to include related Customer
            .Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
            .Take(pageSize) // Take only the number of records for the current page
            .ToListAsync();

        // If no orders are found
        if (orders.Count == 0)
        {
            return NotFound("No orders found.");
        }

        // Create pagination metadata
        var paginationMetadata = new
        {
            TotalCount = totalOrders,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize)
        };

        // Add pagination metadata to the response headers
        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<Order?> Get(int id)
    {
        return _db.Orders.FirstOrDefault(c => c.OrderId == id);
    }

    // Create a new order
    [HttpPost]
    public async Task<ActionResult<Order>> Post(Order? order)
    {
        if (order == null)
        {
            return BadRequest("Order cannot be null.");
        }

        // Optionally, you can validate if the customer exists before creating the order.
        var customer = await _db.Customers.FindAsync(order.CustomerId);
        if (customer == null)
        {
            return BadRequest("Customer not found.");
        }
        
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        var deviceTokens = await _db.DeviceTokens.ToListAsync();

        // Send notification to all devices
        await FirebaseService.SendPushNotificationAsync(
            deviceTokens.ConvertAll(e => e.Token), // FCM device token
            "New Order Created", // Notification title
            $"Order {order.OrderId} has been created successfully.", // Notification body
            customer.CustomerId
        );

        // Return the created order with a location header pointing to the new order
        return CreatedAtAction(nameof(Get), new { id = order.OrderId }, order);
    }

    // Update an existing order
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Order order)
    {
        if (id != order.OrderId)
        {
            return BadRequest("Order ID mismatch.");
        }

        var existingOrder = await _db.Orders.FindAsync(id);
        if (existingOrder == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        _db.Entry(existingOrder).CurrentValues.SetValues(order);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // Delete an order by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}