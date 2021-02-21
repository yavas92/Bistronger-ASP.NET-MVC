using Bistronger.Data.Enums;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data
{
    public class BusinessFilter : DefaultFilter
    {
        public string Name { get; set; }
        public int? Zipcode { get; set; }
        public BusinessType? Type { get; set; }
        public bool ShowOpenOnly { get; set; }
    }
}