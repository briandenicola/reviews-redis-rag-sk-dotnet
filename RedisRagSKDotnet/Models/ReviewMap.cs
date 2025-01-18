using CsvHelper.Configuration;

namespace Reviews.RedisRag.SK.Dotnet.Models;

/// <summary>
/// Maps the Review class properties to CSV columns.
/// </summary>
internal sealed class ReviewMap : ClassMap<Review>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewMap"/> class.
    /// Configures the mapping between Review properties and CSV columns.
    /// </summary>
    public ReviewMap()
    {
        Map(m => m.Id);
        Map(m => m.ProductId);
        Map(m => m.UserId);
        Map(m => m.ProfileName);
        Map(m => m.HelpfulnessDenominator);
        Map(m => m.HelpfulnessNumerator);
        Map(m => m.Score);
        Map(m => m.Time);
        Map(m => m.Summary);
        Map(m => m.Text);
    }
}
