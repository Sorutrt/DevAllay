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
            statusLabel.Text = "✓ クリップボードにコピーしました";
            statusLabel.ForeColor = Color.Green;
            
            // Copy to clipboard
            try
            {
                Clipboard.SetText(result);
            }
            catch
            {
                statusLabel.Text = "クリップボードへのコピーに失敗しました";
                statusLabel.ForeColor = Color.Orange;
            }
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
}
