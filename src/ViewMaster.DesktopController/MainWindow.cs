using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using System.Text.Json;
using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Sequences;

namespace ViewMaster.DesktopController
{
    public partial class MainWindow : Form, IWinFormsShell
    {
        private Sequence currentSession = new("New Session", new List<Cue>());
        private CancellationTokenSource cancellationTokenSource = new();
        private readonly ICueDispatcher cueDispatcher;

        public MainWindow(ICueDispatcher cueDispatcher)
        {
            InitializeComponent();
            this.cueDispatcher = cueDispatcher;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (File.Exists("./sequence.jsonc"))
            {
                var data = File.ReadAllText("./sequence.jsonc");
                var sequence = JsonSerializer.Deserialize<SequenceData>(data, JsonSerializerSettingsProvider.Default);
                if (sequence is null)
                {
                    MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.currentSession = new Sequence(sequence);
            }
            InitGrid();
        }

        private void InitGrid()
        {
            this.sequenceGrid.SelectionChanged -= this.SequenceGrid_SelectionChanged;

            this.sequenceGrid.DataSource = this.currentSession.Cues;
            foreach (DataGridViewColumn c in this.sequenceGrid.Columns)
            {
                if (c.Name == nameof(Cue.Operations))
                {
                    c.Visible = false;
                }
                else if (c.Name == nameof(Cue.Label))
                {
                    c.Width = 410;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (c.Name == nameof(Cue.Ordinal))
                {
                    c.Width = 120;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // set event after grid has initialized to avoid triggering unecessary executions of the same cue.
            this.sequenceGrid.SelectionChanged += this.SequenceGrid_SelectionChanged;

            this.TriggerCue(this.currentSession?.Cues?.FirstOrDefault()).Wait();
        }

        private void LoadSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "ViewMaster files (*.vwm)|*.vwm|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.AddToRecent = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = openFileDialog.FileName;
                var data = File.ReadAllText(filePath);
                var sequence = JsonSerializer.Deserialize<SequenceData>(data, JsonSerializerSettingsProvider.Default);
                if (sequence is null)
                {
                    MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.currentSession = new Sequence(sequence);
                InitGrid();
            }
        }

        private void SaveSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = "c:\\";
            fileDialog.Filter = "ViewMaster files (*.vwm)|*.vwm|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = fileDialog.FileName;

                var sequence = JsonSerializer.Serialize(this.currentSession, JsonSerializerSettingsProvider.Default);
                if (sequence is null)
                {
                    MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.WriteAllText(filePath, sequence);
            }
        }

        private async void SequenceGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.sequenceGrid?.CurrentRow?.DataBoundItem is not Cue cue)
            {
                return;
            }
            await TriggerCue(cue);
        }

        private async Task TriggerCue(Cue? cue)
        {
            if (cue is null)
            {
                return;
            }

            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource = new CancellationTokenSource();
            await this.cueDispatcher.DispatchAsync(new CueArguments(cue, this.cancellationTokenSource.Token));
        }
    }
}