using Bistronger.Data;

/// <summary>
/// Stijn + Joren
/// </summary>
namespace Bistronger.Models.Businesses
{
    public class BusinessIndexViewModel
    {
        public DataSet<Business> Businesses { get; set; }
        public BusinessFilter Filter { get; set; }
    }
}