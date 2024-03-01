using System.ComponentModel.DataAnnotations;

namespace BarberBookingAgent.Application.Common.Options;

public record OpenAiOptions
{
    [Required]
    public string ModelId { get; init; } = null!;

    [Required]
    public string ApiKey { get; init; } = null!;

    [Required]
    public string OrganizationId { get; init; } = null!;
}