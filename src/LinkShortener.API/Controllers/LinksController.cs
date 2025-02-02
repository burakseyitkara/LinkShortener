using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LinkShortener.Application.DTOs;
using LinkShortener.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkShortener.API.Controllers
{
    /// <summary>
    /// Link işlemleri için controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly ILinkAppService _linkAppService;
        private readonly ILogger<LinksController> _logger;

        public LinksController(ILinkAppService linkAppService, ILogger<LinksController> logger)
        {
            _linkAppService = linkAppService;
            _logger = logger;
        }

        /// <summary>
        /// Yeni link oluştur
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LinkDto>> Create([FromBody] CreateLinkDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                request.UserId = Guid.Parse(userId);
            }

            var link = await _linkAppService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
        }

        /// <summary>
        /// Link güncelle
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<LinkDto>> Update(Guid id, [FromBody] CreateLinkDto request)
        {
            var link = await _linkAppService.UpdateAsync(id, request);
            return Ok(link);
        }

        /// <summary>
        /// Link sil
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _linkAppService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// ID'ye göre link getir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LinkDto>> GetById(Guid id)
        {
            var link = await _linkAppService.GetByIdAsync(id);
            return Ok(link);
        }

        /// <summary>
        /// Kısa koda göre link getir
        /// </summary>
        [HttpGet("by-code/{shortCode}")]
        public async Task<ActionResult<LinkDto>> GetByShortCode(string shortCode)
        {
            var link = await _linkAppService.GetByShortCodeAsync(shortCode);
            return Ok(link);
        }

        /// <summary>
        /// Kullanıcıya ait linkleri getir
        /// </summary>
        [HttpGet("my")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LinkDto>>> GetMyLinks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }

            var links = await _linkAppService.GetByUserIdAsync(Guid.Parse(userId));
            return Ok(links);
        }

        /// <summary>
        /// En çok tıklanan linkleri getir
        /// </summary>
        [HttpGet("most-clicked")]
        public async Task<ActionResult<IEnumerable<LinkDto>>> GetMostClicked([FromQuery] int count = 10)
        {
            var links = await _linkAppService.GetMostClickedLinksAsync(count);
            return Ok(links);
        }

        /// <summary>
        /// Link tıklanma olayını kaydet
        /// </summary>
        [HttpPost("{shortCode}/click")]
        public async Task<IActionResult> RecordClick(string shortCode)
        {
            // ipAddress için null kontrolü sağlanarak varsayılan "unknown" değeri atanıyor
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var referer = HttpContext.Request.Headers["Referer"].ToString();

            // _linkAppService.RecordClickAsync metodu herhangi bir değer döndürmüyorsa atama yapılmamalı
            await _linkAppService.RecordClickAsync(shortCode, ipAddress, userAgent, referer);
            return Ok();
        }

        /// <summary>
        /// Link istatistiklerini getir
        /// </summary>
        [HttpGet("{id}/statistics")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LinkClickDto>>> GetStatistics(
            Guid id,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var statistics = await _linkAppService.GetClickStatisticsAsync(id, startDate, endDate);
            return Ok(statistics);
        }

        [HttpGet("{shortCode}/redirect")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var link = await _linkAppService.GetByShortCodeAsync(shortCode);
            
            // Background task için gerekli değerler alınır
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers.UserAgent.ToString();
            var referer = Request.Headers.Referer.ToString();
            
            // Fire and forget şeklinde tıklama kaydı asenkron olarak başlatılır
            _ = Task.Run(async () =>
            {
                try
                {
                    await _linkAppService.RecordClickAsync(shortCode, ipAddress, userAgent, referer);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error recording click for shortCode: {ShortCode}", shortCode);
                }
            });

            return RedirectPermanent(link.OriginalUrl);
        }
    }
}
