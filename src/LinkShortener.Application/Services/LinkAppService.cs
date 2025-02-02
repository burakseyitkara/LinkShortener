using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Interfaces;
using LinkShortener.Domain.Services;
using LinkShortener.Infrastructure.Caching;
using Microsoft.Extensions.Logging;
using LinkShortener.Application.Validators;
using LinkShortener.Application.Exceptions;
using LinkShortener.Domain.Exceptions;
using LinkShortener.Application.Interfaces;

namespace LinkShortener.Application.Services;

/// <summary>
/// Link işlemleri için uygulama servisi
/// </summary>
public class LinkAppService : ILinkAppService
{
    private readonly ILinkRepository _linkRepository;
    private readonly ILinkClickRepository _linkClickRepository;
    private readonly ILinkService _linkService;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<LinkAppService> _logger;

    public LinkAppService(
        ILinkRepository linkRepository,
        ILinkClickRepository linkClickRepository,
        ILinkService linkService,
        ICacheService cacheService,
        IMapper mapper,
        ILogger<LinkAppService> logger)
    {
        _linkRepository = linkRepository;
        _linkClickRepository = linkClickRepository;
        _linkService = linkService;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<LinkDto> CreateAsync(CreateLinkDto input)
    {
        var validator = new CreateLinkDtoValidator(_linkService);
        var validationResult = await validator.ValidateAsync(input);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var shortCode = !string.IsNullOrEmpty(input.CustomShortCode)
            ? input.CustomShortCode
            : await _linkService.GenerateShortCodeAsync();

        var link = new Link
        {
            OriginalUrl = input.OriginalUrl,
            ShortCode = shortCode,
            Title = input.Title,
            Description = input.Description,
            ExpiresAt = input.ExpiresAt,
            UserId = input.UserId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _linkRepository.AddAsync(link);
        await _cacheService.SetAsync($"link:{shortCode}", link, TimeSpan.FromHours(1));

        return _mapper.Map<LinkDto>(link);
    }

    public async Task<LinkDto> UpdateAsync(Guid id, CreateLinkDto input)
    {
        var validator = new CreateLinkDtoValidator(_linkService);
        var validationResult = await validator.ValidateAsync(input);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var link = await _linkRepository.GetByIdAsync(id);
        if (link == null)
            throw new NotFoundException("Link not found");

        var shortCode = !string.IsNullOrEmpty(input.CustomShortCode)
            ? input.CustomShortCode
            : link.ShortCode;

        if (shortCode != link.ShortCode)
        {
            var isValidShortCode = await _linkService.ValidateShortCode(shortCode);
            if (!isValidShortCode)
                throw new ValidationException("Invalid or already used short code");
        }

        link.OriginalUrl = input.OriginalUrl;
        link.ShortCode = shortCode;
        link.Title = input.Title;
        link.Description = input.Description;
        link.ExpiresAt = input.ExpiresAt;
        link.UpdatedAt = DateTime.UtcNow;

        await _linkRepository.UpdateAsync(link);
        await _cacheService.SetAsync($"link:{shortCode}", link, TimeSpan.FromHours(1));

        if (shortCode != link.ShortCode)
            await _cacheService.RemoveAsync($"link:{link.ShortCode}");

        return _mapper.Map<LinkDto>(link);
    }

    public async Task DeleteAsync(Guid id)
    {
        var link = await _linkRepository.GetByIdAsync(id);
        if (link == null)
            throw new NotFoundException("Link not found");

        await _linkRepository.DeleteAsync(link);
        await _cacheService.RemoveAsync($"link:{link.ShortCode}");
    }

    public async Task<LinkDto> GetByIdAsync(Guid id)
    {
        var link = await _linkRepository.GetByIdAsync(id);
        if (link == null)
            throw new NotFoundException("Link not found");

        return _mapper.Map<LinkDto>(link);
    }

    public async Task<LinkDto> GetByShortCodeAsync(string shortCode)
    {
        // Try to get from cache first
        var cachedLink = await _cacheService.GetAsync<Link>($"link:{shortCode}");
        if (cachedLink != null)
            return _mapper.Map<LinkDto>(cachedLink);

        // If not in cache, get from database
        var link = await _linkRepository.GetByShortCodeAsync(shortCode);
        if (link == null)
            throw new NotFoundException("Link not found");

        // Add to cache
        await _cacheService.SetAsync($"link:{shortCode}", link, TimeSpan.FromHours(1));

        return _mapper.Map<LinkDto>(link);
    }

    public async Task<IEnumerable<LinkDto>> GetByUserIdAsync(Guid userId)
    {
        var links = await _linkRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<LinkDto>>(links);
    }

    public async Task<IEnumerable<LinkDto>> GetMostClickedLinksAsync(int count)
    {
        var links = await _linkRepository.GetMostClickedLinksAsync(count);
        return _mapper.Map<IEnumerable<LinkDto>>(links);
    }

    public async Task RecordClickAsync(string shortCode, string ipAddress, string userAgent, string referer)
    {
        var link = await _linkRepository.GetByShortCodeAsync(shortCode);
        if (link == null)
            throw new NotFoundException("Link not found");

        var isValid = await _linkService.IsLinkValid(link);
        if (!isValid)
            throw new ValidationException("Link is not active or has expired");

        var linkClick = new LinkClick
        {
            LinkId = link.Id,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Referer = referer,
            CreatedAt = DateTime.UtcNow
        };

        await _linkClickRepository.AddAsync(linkClick);
        await _linkService.UpdateLinkStatisticsAsync(link.Id, ipAddress, userAgent, referer);
    }

    public async Task<IEnumerable<LinkClickDto>> GetClickStatisticsAsync(Guid linkId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var link = await _linkRepository.GetByIdAsync(linkId);
        if (link == null)
            throw new NotFoundException("Link not found");

        var clicks = await _linkClickRepository.GetClicksByLinkIdAsync(linkId, startDate, endDate);
        return _mapper.Map<IEnumerable<LinkClickDto>>(clicks);
    }
} 