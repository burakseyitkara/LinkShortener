using System;
using System.Threading.Tasks;
using LinkShortener.API.Models.Auth;
using LinkShortener.API.Services.Auth;
using LinkShortener.Application.DTOs;
using LinkShortener.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LinkShortener.API.Controllers;

/// <summary>
/// Kimlik doğrulama işlemleri için controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAppService _userAppService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public AuthController(
        IUserAppService userAppService,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthController> logger)
    {
        _userAppService = userAppService;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userAppService.LoginAsync(request.UsernameOrEmail, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Kullanıcı adı/email veya şifre hatalı." });

            var token = _jwtService.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiresIn = _jwtSettings.ExpiryMinutes * 60,
                User = user
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Giriş sırasında hata oluştu. UsernameOrEmail: {UsernameOrEmail}", request.UsernameOrEmail);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }

    /// <summary>
    /// Yeni kullanıcı kaydı
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto request)
    {
        try
        {
            var user = await _userAppService.RegisterAsync(request);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kayıt sırasında hata oluştu. Username: {Username}", request.Username);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }

    /// <summary>
    /// Email doğrulama
    /// </summary>
    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(string email, string token)
    {
        try
        {
            await _userAppService.VerifyEmailAsync(email, token);
            return Ok(new { message = "Email başarıyla doğrulandı." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email doğrulama sırasında hata oluştu. Email: {Email}", email);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }

    /// <summary>
    /// Şifre sıfırlama emaili gönder
    /// </summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            await _userAppService.SendPasswordResetEmailAsync(email);
            return Ok(new { message = "Şifre sıfırlama bağlantısı email adresinize gönderildi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifre sıfırlama emaili gönderirken hata oluştu. Email: {Email}", email);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }

    /// <summary>
    /// Şifre sıfırla
    /// </summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
    {
        try
        {
            await _userAppService.ResetPasswordAsync(email, token, newPassword);
            return Ok(new { message = "Şifreniz başarıyla sıfırlandı." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifre sıfırlama sırasında hata oluştu. Email: {Email}", email);
            return StatusCode(500, "Bir hata oluştu.");
        }
    }
} 