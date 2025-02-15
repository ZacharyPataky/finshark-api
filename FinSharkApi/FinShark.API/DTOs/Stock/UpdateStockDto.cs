using System.ComponentModel.DataAnnotations;

namespace FinShark.API.DTOs.Stock;

public class UpdateStockDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(10, ErrorMessage = "CompanyName cannot exceed 10 characters.")]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000000000, ErrorMessage = "Purchase must be between 1 and 1,000,000,000,000, inclusive.")]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100, ErrorMessage = "LastDiv must be between 0.001 and 100, inclusive.")]
    public decimal LastDiv { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot exceed 10 characters.")]
    public string Industry { get; set; } = string.Empty;
    [Required]
    [Range(1, 5000000000000, ErrorMessage = "MarketCap must be between 1 and 5,000,000,000,000, inclusive.")]
    public long MarketCap { get; set; }
}
