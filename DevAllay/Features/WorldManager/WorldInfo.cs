namespace DevAllay.Features.WorldManager;

public class WorldInfo
{
    public string FolderPath { get; set; } = string.Empty;
    public string WorldName { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }

    public override string ToString() => WorldName;
}
