namespace DevAllay.Features.WorldManager;

public class WorldInfo
{
    public string FolderPath { get; set; } = string.Empty;
    public string WorldName { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
    public string Edition { get; set; } = string.Empty;

    public override string ToString() => $"{WorldName} [{Edition}]";
}
