using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace SyncFaction.Services.FactionFiles;

public class MapPackResponse : Dictionary<string, MapPack>
{
}

public interface IMod
{
    public string IdString { get; }
    public string BrowserUrl { get; }
    public string ImageUrl { get; }
    public string ImagePath { get; }
    public string DownloadUrl { get; }
    public DateTime CreatedAt { get; }
    public string Markdown { get; }
}

public class SeparatorItem : IMod
{
    private readonly string value;

    public SeparatorItem(string value)
    {
        this.value = value;
    }

    public string IdString => string.Empty;
    public string BrowserUrl { get; }
    public string ImageUrl { get; }
    public string ImagePath { get; }
    public string DownloadUrl { get; }
    public DateTime CreatedAt { get; }
    public string Markdown { get; }

    public override string ToString() => value;
}

public class MapPack : IMod
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string Author { get; init; }
    public string Description { get; init; }
    public string DescriptionMd { get; set; }
    public long Size { get; init; }
    public long UploadTime { get; init; }
    public long DownloadCount { get; init; }
    public bool StaffFeatured { get; init; }

    public string? ImageUrl { get; set; }

    [JsonPropertyName("image_thumb_4by3_url")]
    public string ImageThumb4By3Url { get; set; }
    public string DownloadUrl { get; init; }

    public string IdString => $"{GetType().Name}_{Id}";
    public string BrowserUrl => string.Format(Constants.BrowserUrlTemplate, Id);
    public DateTime CreatedAt => DateTime.UnixEpoch.AddSeconds(UploadTime);
    public string? ImagePath => ImageThumb4By3Url != null ? Path.Combine(Path.GetTempPath(), $"ff_{Id}.png") : null;
    public string Markdown => @$"# **{Name}** by {Author}

*Added: {CreatedAt:yyyy MMMM dd}  #  Downloads: {DownloadCount}*  #  [See on FactionFiles]({BrowserUrl}) 

{ImageMd}

{DescriptionMd}
";

    public override string ToString() => $"{Name}";

    private string ImageMd => ImageThumb4By3Url != null ? $"![image]({ImagePath})" : string.Empty;
}
