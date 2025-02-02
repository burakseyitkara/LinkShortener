using System;
using System.Threading.Tasks;
using LinkShortener.Application.DTOs;
using LinkShortener.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkShortener.API.Controllers;

/// <summary>
/// Kısa URL yönlendirme controller'ı
/// </summary>
[ApiController]
[Route("r")]
public class RedirectController : ControllerBase
{
    private readonly ILinkAppService _linkAppService;
    private readonly ILogger<RedirectController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public RedirectController(
        ILinkAppService linkAppService,
        ILogger<RedirectController> logger)
    {
        _linkAppService = linkAppService;
        _logger = logger;
    }

    /// <summary>
    /// Kısa URL'yi orijinal URL'ye yönlendirir
    /// </summary>
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
    {
        try
        {
            var link = await _linkAppService.GetByShortCodeAsync(shortCode);
            if (link == null)
                return NotFound();

            // Tıklanma kaydı
            await _linkAppService.RecordClickAsync(
                shortCode,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                Request.Headers["User-Agent"].ToString(),
                Request.Headers["Referer"].ToString());

            return Redirect(link.OriginalUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Yönlendirme sırasında hata oluştu. ShortCode: {ShortCode}", shortCode);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }
} 