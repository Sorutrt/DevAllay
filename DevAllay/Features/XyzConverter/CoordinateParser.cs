namespace DevAllay.Features.XyzConverter;

public static class CoordinateParser
{
    public static (bool Success, string Result, string ErrorMessage) Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (false, string.Empty, string.Empty);
        }

        try
        {
            // Remove command prefix (starts with /)
            string cleaned = input.Trim();
            if (cleaned.StartsWith('/'))
            {
                int spaceIndex = cleaned.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    cleaned = cleaned.Substring(spaceIndex + 1).Trim();
                }
                else
                {
                    return (false, string.Empty, "コマンド部分のみで座標がありません");
                }
            }

            // Split by space and parse numbers
            string[] parts = cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length == 3)
            {
                // Case 1: x, y, z
                if (!double.TryParse(parts[0], out double x) ||
                    !double.TryParse(parts[1], out double y) ||
                    !double.TryParse(parts[2], out double z))
                {
                    return (false, string.Empty, "数値のパースに失敗しました");
                }

                string result = $"x={x},y={y},z={z}";
                return (true, result, string.Empty);
            }
            else if (parts.Length == 6)
            {
                // Case 2: x1, y1, z1, x2, y2, z2
                if (!double.TryParse(parts[0], out double x1) ||
                    !double.TryParse(parts[1], out double y1) ||
                    !double.TryParse(parts[2], out double z1) ||
                    !double.TryParse(parts[3], out double x2) ||
                    !double.TryParse(parts[4], out double y2) ||
                    !double.TryParse(parts[5], out double z2))
                {
                    return (false, string.Empty, "数値のパースに失敗しました");
                }

                // Sort and calculate
                double minX = Math.Min(x1, x2);
                double maxX = Math.Max(x1, x2);
                double minY = Math.Min(y1, y2);
                double maxY = Math.Max(y1, y2);
                double minZ = Math.Min(z1, z2);
                double maxZ = Math.Max(z1, z2);

                double dx = maxX - minX;
                double dy = maxY - minY;
                double dz = maxZ - minZ;

                string result = $"x={minX},y={minY},z={minZ},dx={dx},dy={dy},dz={dz}";
                return (true, result, string.Empty);
            }
            else
            {
                return (false, string.Empty, $"座標の数が不正です (3個または6個が必要、{parts.Length}個が入力されました)");
            }
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"エラー: {ex.Message}");
        }
    }
}
