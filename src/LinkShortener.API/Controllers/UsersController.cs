using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LinkShortener.Application.DTOs;
using LinkShortener.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API.Controllers;

/// <summary>
/// Kullanıcı işlemleri için controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserAppService _userAppService;

    public UsersController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    /// <summary>
    /// Tüm kullanıcıları getir
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userAppService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// ID'ye göre kullanıcı getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userAppService.GetByIdAsync(id);
        return Ok(user);
    }

    /// <summary>
    /// Mevcut kullanıcının bilgilerini getir
    /// </summary>
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Forbid();
        }

        var user = await _userAppService.GetByIdAsync(Guid.Parse(userId));
        return Ok(user);
    }

    /// <summary>
    /// Kullanıcı bilgilerini güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] CreateUserDto request)
    {
        var user = await _userAppService.UpdateAsync(id, request);
        return Ok(user);
    }

    /// <summary>
    /// Kullanıcı sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userAppService.DeleteAsync(id);
        return NoContent();
    }
} 