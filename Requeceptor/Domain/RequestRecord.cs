using System.ComponentModel.DataAnnotations;

namespace Requeceptor.Domain;

public class RequestRecord
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string Project { get; set; }

    [MaxLength(10)]
    public string Scheme { get; set; }

    [MaxLength(255)]
    public string Host { get; set; }

    [MaxLength(2048)]
    public string Path { get; set; }

    public string? QueryString { get; set; }

    [MaxLength(100)]
    public string? Action { get; set; }

    [MaxLength(10)]
    public string? Method { get; set; }

    [MaxLength(20)]
    public string? Protocol { get; set; }

    [MaxLength(100)]
    public string? ContentType { get; set; }

    public long? ContentLength { get; set; }

    public string? Headers { get; set; }

    public string? Cookies { get; set; }

    public string? Body { get; set; }

    [MaxLength(50)]
    public string? RemoteIpAddress { get; set; }

    [MaxLength(50)]
    public string? LocalIpAddress { get; set; }

    public DateTime ReceivedAt { get; set; } = DateTime.Now;
}
