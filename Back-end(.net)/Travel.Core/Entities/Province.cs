using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Province
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<City> City { get; set; } = new List<City>();
}
