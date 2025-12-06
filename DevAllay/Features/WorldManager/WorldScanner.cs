namespace DevAllay.Features.WorldManager;

public static class WorldScanner
{
    private static readonly string BedrockWorldsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Packages",
        "Microsoft.MinecraftUWP_8wekyb3d8bbwe",
        "LocalState",
        "games",
        "com.mojang",
        "minecraftWorlds"
    );

    private static readonly string EducationWorldsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Packages",
        "Microsoft.MinecraftEducationEdition_8wekyb3d8bbwe",
        "LocalState",
        "games",
        "com.mojang",
        "minecraftWorlds"
    );

    public static List<WorldInfo> ScanWorlds()
    {
        var worlds = new List<WorldInfo>();

        // Scan Bedrock Edition worlds
        ScanWorldsFromPath(BedrockWorldsPath, "Bedrock", worlds);

        // Scan Education Edition worlds
        ScanWorldsFromPath(EducationWorldsPath, "Education", worlds);

        return worlds;
    }

    private static void ScanWorldsFromPath(string basePath, string edition, List<WorldInfo> worlds)
    {
        if (!Directory.Exists(basePath))
        {
            return;
        }

        try
        {
            var worldFolders = Directory.GetDirectories(basePath);

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
                            LastModified = lastModified,
                            Edition = edition
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
            // Skip if scanning fails
        }
    }

    public static string GetBedrockWorldsPath() => BedrockWorldsPath;
    public static string GetEducationWorldsPath() => EducationWorldsPath;
}
