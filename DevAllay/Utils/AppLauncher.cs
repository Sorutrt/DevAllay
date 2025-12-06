using System.Diagnostics;

namespace DevAllay.Utils;

public static class AppLauncher
{
    public static void OpenExplorer(string path)
    {
        try
        {
            Process.Start("explorer.exe", $"\"{path}\"");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"エクスプローラーの起動に失敗しました: {ex.Message}", 
                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static void OpenTerminal(string path)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "wt.exe",
                Arguments = $"-d \"{path}\"",
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Windows Terminalの起動に失敗しました: {ex.Message}", 
                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static void OpenVSCode(string path)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "code",
                Arguments = $"\"{path}\"",
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"VS Codeの起動に失敗しました: {ex.Message}", 
                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
