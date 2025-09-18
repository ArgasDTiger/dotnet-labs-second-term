using System.ComponentModel.DataAnnotations;

namespace Shared.Settings;

public sealed class ConnectionStringSettings
{
    public const string SectionName = "ConnectionStrings";
    [Required] 
    public string Default { get; init; } = string.Empty;
}