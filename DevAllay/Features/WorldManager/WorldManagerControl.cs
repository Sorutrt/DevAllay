using DevAllay.Utils;

namespace DevAllay.Features.WorldManager;

public class WorldManagerControl : UserControl
{
    private ListBox worldListBox = null!;
    private Button refreshButton = null!;
    private Button openExplorerButton = null!;
    private Button openTerminalButton = null!;
    private Button openVSCodeButton = null!;
    private Label statusLabel = null!;
    private List<WorldInfo> worlds = new();

    public WorldManagerControl()
    {
        InitializeComponents();
        LoadWorlds();
    }

    private void InitializeComponents()
    {
        Padding = new Padding(20);

        // Title Label
        var titleLabel = new Label
        {
            Text = "Minecraft Bedrock ワールド一覧",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(0, 0)
        };

        // Refresh Button
        refreshButton = new Button
        {
            Text = "🔄 更新",
            Location = new Point(0, 35),
            Size = new Size(100, 30)
        };
        refreshButton.Click += (s, e) => LoadWorlds();

        // World ListBox
        worldListBox = new ListBox
        {
            Location = new Point(0, 75),
            Size = new Size(500, 350),
            Font = new Font("Segoe UI", 10)
        };
        worldListBox.AllowDrop = true;
        worldListBox.DragEnter += OnDragEnter;
        worldListBox.DragDrop += OnDragDrop;
        worldListBox.SelectedIndexChanged += OnWorldSelected;

        // Buttons Panel
        var buttonsPanel = new Panel
        {
            Location = new Point(520, 75),
            Size = new Size(250, 350)
        };

        openExplorerButton = new Button
        {
            Text = "📁 Open Explorer",
            Location = new Point(0, 0),
            Size = new Size(200, 40),
            Enabled = false
        };
        openExplorerButton.Click += OnOpenExplorer;

        openTerminalButton = new Button
        {
            Text = "⌨️ Open Terminal",
            Location = new Point(0, 50),
            Size = new Size(200, 40),
            Enabled = false
        };
        openTerminalButton.Click += OnOpenTerminal;

        openVSCodeButton = new Button
        {
            Text = "💻 Open VS Code",
            Location = new Point(0, 100),
            Size = new Size(200, 40),
            Enabled = false
        };
        openVSCodeButton.Click += OnOpenVSCode;

        var dropHintLabel = new Label
        {
            Text = "💡 ヒント:\n画像ファイル(.png, .jpg)を\nワールド名にドラッグ&ドロップすると\nアイコンを変更できます",
            Location = new Point(0, 160),
            Size = new Size(200, 100),
            ForeColor = Color.Gray
        };

        buttonsPanel.Controls.AddRange(new Control[]
        {
            openExplorerButton,
            openTerminalButton,
            openVSCodeButton,
            dropHintLabel
        });

        // Status Label
        statusLabel = new Label
        {
            AutoSize = true,
            Location = new Point(0, 435),
            Text = string.Empty
        };

        Controls.AddRange(new Control[]
        {
            titleLabel,
            refreshButton,
            worldListBox,
            buttonsPanel,
            statusLabel
        });
    }

    private void LoadWorlds()
    {
        worlds = WorldScanner.ScanWorlds();
        worldListBox.Items.Clear();
        
        foreach (var world in worlds)
        {
            worldListBox.Items.Add(world);
        }

        statusLabel.Text = $"{worlds.Count} 個のワールドが見つかりました";
        statusLabel.ForeColor = Color.Green;
    }

    private void OnWorldSelected(object? sender, EventArgs e)
    {
        bool hasSelection = worldListBox.SelectedIndex >= 0;
        openExplorerButton.Enabled = hasSelection;
        openTerminalButton.Enabled = hasSelection;
        openVSCodeButton.Enabled = hasSelection;
    }

    private WorldInfo? GetSelectedWorld()
    {
        if (worldListBox.SelectedIndex < 0)
            return null;
        return worlds[worldListBox.SelectedIndex];
    }

    private void OnOpenExplorer(object? sender, EventArgs e)
    {
        var world = GetSelectedWorld();
        if (world != null)
        {
            AppLauncher.OpenExplorer(world.FolderPath);
        }
    }

    private void OnOpenTerminal(object? sender, EventArgs e)
    {
        var world = GetSelectedWorld();
        if (world != null)
        {
            AppLauncher.OpenTerminal(world.FolderPath);
        }
    }

    private void OnOpenVSCode(object? sender, EventArgs e)
    {
        var world = GetSelectedWorld();
        if (world != null)
        {
            AppLauncher.OpenVSCode(world.FolderPath);
        }
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
            {
                string ext = Path.GetExtension(files[0]).ToLower();
                if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
        }
        e.Effect = DragDropEffects.None;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
            return;

        var files = e.Data.GetData(DataFormats.FileDrop) as string[];
        if (files == null || files.Length == 0)
            return;

        string imageFile = files[0];
        string ext = Path.GetExtension(imageFile).ToLower();
        if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
            return;

        // Get the world at drop position
        Point clientPoint = worldListBox.PointToClient(new Point(e.X, e.Y));
        int index = worldListBox.IndexFromPoint(clientPoint);
        
        if (index < 0 || index >= worlds.Count)
        {
            statusLabel.Text = "ワールドを選択してください";
            statusLabel.ForeColor = Color.Red;
            return;
        }

        var world = worlds[index];
        
        if (IconUpdater.UpdateWorldIcon(world.FolderPath, imageFile))
        {
            statusLabel.Text = $"✓ {world.WorldName} のアイコンを更新しました";
            statusLabel.ForeColor = Color.Green;
        }
        else
        {
            statusLabel.Text = "アイコンの更新に失敗しました";
            statusLabel.ForeColor = Color.Red;
        }
    }
}
