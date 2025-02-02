using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LinkShortener.Application.DTOs;
using LinkShortener.Application.Interfaces;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Interfaces;
using LinkShortener.Domain.Services;
using Microsoft.Extensions.Logging;

namespace LinkShortener.Application.Services;

/// <summary>
/// Kullanıcı işlemleri için uygulama servisi
/// </summary>
public class UserAppService : IUserAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserAppService> _logger;

    public UserAppService(
        IUserRepository userRepository,
        IUserService userService,
        IMapper mapper,
        ILogger<UserAppService> logger)
    {
        _userRepository = userRepository;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto input)
    {
        // Kullanıcı adı ve e-posta kontrolü
        if (!_userService.ValidateUsername(input.Username))
        {
            throw new ArgumentException("Geçersiz kullanıcı adı formatı.");
        }

        if (!_userService.ValidateEmail(input.Email))
        {
            throw new ArgumentException("Geçersiz e-posta formatı.");
        }

        // Kullanıcı adı ve e-posta benzersizlik kontrolü
        if (await _userRepository.GetByUsernameAsync(input.Username) != null)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten kullanımda.");
        }

        if (await _userRepository.GetByEmailAsync(input.Email) != null)
        {
            throw new InvalidOperationException("Bu e-posta adresi zaten kullanımda.");
        }

        // Şifre hash'leme
        var passwordHash = await _userService.HashPasswordAsync(input.Password);

        // Kullanıcı oluşturma
        var user = _mapper.Map<User>(input);
        user.PasswordHash = passwordHash;
        user = await _userRepository.AddAsync(user);

        _logger.LogInformation("Yeni kullanıcı kaydı yapıldı: {Username}", input.Username);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> LoginAsync(string usernameOrEmail, string password)
    {
        var user = await _userRepository.ValidateCredentialsAsync(usernameOrEmail);

        if (user == null || !await _userService.VerifyPasswordAsync(password, user.PasswordHash))
        {
            throw new InvalidOperationException("Geçersiz kullanıcı adı/e-posta veya şifre.");
        }

        await _userRepository.UpdateLastLoginAsync(user.Username);

        _logger.LogInformation("Kullanıcı girişi yapıldı: {Username}", user.Username);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, CreateUserDto input)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        // Kullanıcı adı ve e-posta kontrolü
        if (!_userService.ValidateUsername(input.Username))
        {
            throw new ArgumentException("Geçersiz kullanıcı adı formatı.");
        }

        if (!_userService.ValidateEmail(input.Email))
        {
            throw new ArgumentException("Geçersiz e-posta formatı.");
        }

        // Benzersizlik kontrolü (mevcut kullanıcı hariç)
        var existingUsername = await _userRepository.GetByUsernameAsync(input.Username);
        if (existingUsername != null && existingUsername.Id != id)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten kullanımda.");
        }

        var existingEmail = await _userRepository.GetByEmailAsync(input.Email);
        if (existingEmail != null && existingEmail.Id != id)
        {
            throw new InvalidOperationException("Bu e-posta adresi zaten kullanımda.");
        }

        // Şifre güncelleme (eğer yeni şifre girilmişse)
        if (!string.IsNullOrEmpty(input.Password))
        {
            user.PasswordHash = await _userService.HashPasswordAsync(input.Password);
        }

        _mapper.Map(input, user);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Kullanıcı güncellendi: {Username}", user.Username);
        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        await _userRepository.DeleteAsync(user);
        _logger.LogInformation("Kullanıcı silindi: {Username}", user.Username);
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task VerifyEmailAsync(string email, string token)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        // Token doğrulama işlemi burada yapılacak
        await _userRepository.UpdateEmailVerificationStatusAsync(email, true);
        _logger.LogInformation("Kullanıcı e-postası doğrulandı: {Email}", email);
    }

    public async Task SendPasswordResetEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        var token = await _userService.GeneratePasswordResetTokenAsync(email);
        // E-posta gönderme işlemi burada yapılacak

        _logger.LogInformation("Şifre sıfırlama e-postası gönderildi: {Email}", email);
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        // Token doğrulama işlemi burada yapılacak
        user.PasswordHash = await _userService.HashPasswordAsync(newPassword);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Kullanıcı şifresi sıfırlandı: {Email}", email);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
} 