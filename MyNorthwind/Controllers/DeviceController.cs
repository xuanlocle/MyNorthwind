using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNorthwind.Models;

namespace MyNorthwind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly NorthwindContext _db;

    public DeviceController(NorthwindContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceToken deviceToken)
    {
        if (string.IsNullOrEmpty(deviceToken.Token))
        {
            return BadRequest("Device token is required.");
        }

        // Check if the token is already registered
        var existingToken = await _db.DeviceTokens
            .FirstOrDefaultAsync(dt => dt.Token == deviceToken.Token);

        if (existingToken != null)
        {
            return Ok("Device token already registered.");
        }

        // Store the new device token
        deviceToken.RegisteredAt = DateTime.UtcNow;
        _db.DeviceTokens.Add(deviceToken);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(RegisterDevice), new { id = deviceToken.DeviceTokenId }, deviceToken);
    }

    // Check if the device token is already registered
    [HttpGet("check/{token}")]
    public async Task<IActionResult> CheckDeviceRegistration(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Device token is required.");
        }

        // Check if the device token exists in the database
        var existingToken = await _db.DeviceTokens
            .FirstOrDefaultAsync(dt => dt.Token == token);

        if (existingToken != null)
        {
            return Ok(new { message = "Device token is already registered.", isRegistered = true });
        }
        else
        {
            return Ok(new { message = "Device token is not registered.", isRegistered = false });
        }
    }
}