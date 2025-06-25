using System.ComponentModel.DataAnnotations;

namespace Requeceptor.Domain;

public class RuleRecord
{
    [Key]
    public int Id { get; set; }

    [MaxLength(2048)]
    public string? Path { get; set; }

    public string? QueryString { get; set; }

    public string? ResponseStatus { get; set; }
    public string? ResponseContentType { get; set; }
    public string? ResponseBody { get; set; }
    public bool Active { get; set; }
}
