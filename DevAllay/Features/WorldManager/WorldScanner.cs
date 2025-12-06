namespace DevAllay.Features.WorldManager;

public static class WorldScanner
{
    private static readonly string WorldsBasePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Packages",
        "Microsoft.MinecraftUWP_8wekyb3d8bbwe",
        "LocalState",
        "games",
        "com.mojang",
        "minecraftWorlds"
    );

    public static List<WorldInfo> ScanWorlds()
    {
        var worlds = new List<WorldInfo>();

        if (!Directory.Exists(WorldsBasePath))
        {
            return worlds;
        }

        try
        {
            var worldFolders = Directory.GetDirectories(WorldsBasePath);

            foreach (var folder in worldFolders)
            {
                var levelnameFile = Path.Combine(folder, "levelname.txt");
                
                if (File.Exists(levelnameFile))
                {
                    try
                    {
                        string worldName = File.ReadAllText(levelnameFile).Trim();
                        var lastModified = Directory.GetLastWriteTime(folder);

                        worlds.Add(new WorldInfo
                        {
                            FolderPath = folder,
                            WorldName = string.IsNullOrWhiteSpace(worldName) 
                                ? Path.GetFileName(folder) 
                                : worldName,
                            LastModified = lastModified
                        });
                    }
                    catch
                    {
                        // Skip worlds that can't be read
                        continue;
                    }
                }
            }
        }
        catch
        {
            // Return empty list if scanning fails
        }

        return worlds;
    }

    public static string GetWorldsBasePath() => WorldsBasePath;
}
