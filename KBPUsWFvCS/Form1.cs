namespace KBPUsWFvCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SortedDictionary<string, float> userCache = new SortedDictionary<string, float>
            {
              {"Zelená", Settings.GreenH},
              {"Modrá", Settings.BlueH},
            };
            comboBox1.DataSource = new BindingSource(userCache, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;

            if (comboBox1.SelectedItem is KeyValuePair<string, float> dict)
                Settings.KeyColor = dict.Value;

            label1.Text = $"Hodnata '{nameof(Settings.Hue)}' {trackBar1.Value}";

            label2.Text = $"Hodnata '{nameof(Settings.Saturation)}' {trackBar2.Value}";

            label3.Text = $"Hodnata '{nameof(Settings.Brightness)}' {trackBar3.Value}";
        }
        private void trackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            Settings.Hue = trackBar1.Value;
            label1.Text = $"Hodnata '{nameof(Settings.Hue)}' {trackBar1.Value}";
        }

        private void trackBar2_ValueChanged(object sender, System.EventArgs e)
        {
            Settings.Saturation = trackBar2.Value;
            label2.Text = $"Hodnata '{nameof(Settings.Saturation)}' {trackBar2.Value}";
        }

        private void trackBar3_ValueChanged(object sender, System.EventArgs e)
        {
            Settings.Brightness = trackBar3.Value;
            label3.Text = $"Hodnata '{nameof(Settings.Brightness)}' {trackBar3.Value}";
        }
        private void comboBox1_SelectedValueChanged(object? sender, System.EventArgs e)
        {
            if(comboBox1.SelectedItem is KeyValuePair<string, float> dict)
                Settings.KeyColor = dict.Value;
        }
    }
}