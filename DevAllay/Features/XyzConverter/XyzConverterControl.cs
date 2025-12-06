namespace DevAllay.Features.XyzConverter;

public class XyzConverterControl : UserControl
{
    private TextBox inputTextBox = null!;
    private TextBox outputTextBox = null!;
    private Label statusLabel = null!;

    public XyzConverterControl()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Layout
        Padding = new Padding(20);
        
        // Input Label
        var inputLabel = new Label
        {
            Text = "座標入力 (3個または6個の数値をスペース区切り):",
            AutoSize = true,
            Location = new Point(0, 0)
        };

        // Input TextBox
        inputTextBox = new TextBox
        {
            Multiline = true,
            Location = new Point(0, 25),
            Size = new Size(800, 100),
            Font = new Font("Consolas", 10),
            PlaceholderText = "例: 100 64 200  または  /tp 100 64 200  または  100 64 200 150 80 250"
        };
        inputTextBox.TextChanged += OnInputTextChanged;

        // Output Label
        var outputLabel = new Label
        {
            Text = "変換結果 (自動的にクリップボードにコピーされます):",
            AutoSize = true,
            Location = new Point(0, 140)
        };

        // Output TextBox
        outputTextBox = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Location = new Point(0, 165),
            Size = new Size(800, 100),
            Font = new Font("Consolas", 10),
            BackColor = SystemColors.Control
        };

        // Status Label
        statusLabel = new Label
        {
            AutoSize = true,
            Location = new Point(0, 275),
            ForeColor = Color.Red,
            Text = string.Empty
        };

        Controls.AddRange(new Control[] 
        { 
            inputLabel, 
            inputTextBox, 
            outputLabel, 
            outputTextBox, 
            statusLabel 
        });
    }

    private void OnInputTextChanged(object? sender, EventArgs e)
    {
        var (success, result, errorMessage) = CoordinateParser.Parse(inputTextBox.Text);

        if (success)
        {
            outputTextBox.Text = result;
            
            // Copy to clipboard with delayed execution to avoid conflicts
            CopyToClipboardAsync(result);
        }
        else if (!string.IsNullOrEmpty(errorMessage))
        {
            outputTextBox.Text = string.Empty;
            statusLabel.Text = errorMessage;
            statusLabel.ForeColor = Color.Red;
        }
        else
        {
            outputTextBox.Text = string.Empty;
            statusLabel.Text = string.Empty;
        }
    }

    private async void CopyToClipboardAsync(string text)
    {
        // Wait a bit to avoid rapid clipboard access during typing
        await Task.Delay(100);
        
        // Verify the text is still current
        if (outputTextBox.Text != text)
            return;

        try
        {
            // Retry mechanism for clipboard access
            int maxRetries = 5;
            bool copied = false;
            
            for (int i = 0; i < maxRetries && !copied; i++)
            {
                try
                {
                    // Use SetDataObject with retry parameter
                    Clipboard.SetDataObject(text, true, 10, 100);
                    copied = true;
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    // Clipboard is busy, wait and retry
                    if (i < maxRetries - 1)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            
            if (copied)
            {
                statusLabel.Text = "✓ クリップボードにコピーしました";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                statusLabel.Text = "⚠ クリップボードへのコピーに失敗しました（他のアプリが使用中）";
                statusLabel.ForeColor = Color.Orange;
            }
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"⚠ クリップボードエラー: {ex.Message}";
            statusLabel.ForeColor = Color.Orange;
        }
    }
}
