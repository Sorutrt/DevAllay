using DevAllay.Features.XyzConverter;
using DevAllay.Features.WorldManager;

namespace DevAllay;

public class MainForm : Form
{
    private TabControl tabControl = null!;
    private TabPage worldManagerTab = null!;
    private TabPage xyzConverterTab = null!;

    public MainForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Form settings
        Text = "DevAllay - Bedrock World Manager & Utils";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;

        // TabControl
        tabControl = new TabControl
        {
            Dock = DockStyle.Fill
        };

        // World Manager Tab
        worldManagerTab = new TabPage("World Manager");
        var worldManagerControl = new WorldManagerControl { Dock = DockStyle.Fill };
        worldManagerTab.Controls.Add(worldManagerControl);
        tabControl.TabPages.Add(worldManagerTab);

        // XYZ Converter Tab
        xyzConverterTab = new TabPage("XYZ Converter");
        var xyzControl = new XyzConverterControl { Dock = DockStyle.Fill };
        xyzConverterTab.Controls.Add(xyzControl);
        tabControl.TabPages.Add(xyzConverterTab);

        Controls.Add(tabControl);
    }
}
