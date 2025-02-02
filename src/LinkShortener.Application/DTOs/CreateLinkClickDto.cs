using System;

namespace LinkShortener.Application.DTOs;

public class CreateLinkClickDto
{
    public CreateLinkClickDto()
    {
        IpAddress = string.Empty;
        UserAgent = string.Empty;
        Referer = string.Empty;
        Country = string.Empty;
        City = string.Empty;
        DeviceType = string.Empty;
        OperatingSystem = string.Empty;
    }

    public Guid LinkId { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Referer { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string DeviceType { get; set; }
    public string OperatingSystem { get; set; }
} 