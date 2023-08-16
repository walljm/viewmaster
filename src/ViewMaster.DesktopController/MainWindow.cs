using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using System.Text.Json;
using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Sequences;

namespace ViewMaster.DesktopController
{
    public partial class MainWindow : Form, IWinFormsShell
    {
        private Sequence currentSession = new("New Session", new List<Cue>(), new List<WriterData>());
        private CancellationTokenSource cancellationTokenSource = new();
        private readonly ICueDispatcher cueDispatcher;

        public MainWindow(ICueDispatcher cueDispatcher)
        {
            InitializeComponent();
            this.cueDispatcher = cueDispatcher;
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            if (File.Exists("./sequence.jsonc"))
            {
                this.LoadSequence("./sequence.jsonc");
            }
            else
            {
                await InitSequenceGrid();
            }
        }

        private void LoadSequenceToolStripMenuItem_Click(object? sender, EventArgs e)
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
                this.LoadSequence(openFileDialog.FileName);
            }
        }

        private void SaveSequenceToolStripMenuItem_Click(object? sender, EventArgs e)
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

        private async void SequenceGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (this.grdCues?.CurrentRow?.DataBoundItem is not Cue cue)
            {
                return;
            }

            _ = UpdateCueDetails(cue);

            await TriggerCue(cue);
        }

        private void GrdCameras_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (this.grdCameras?.CurrentRow?.DataBoundItem is not WriterData writer)
            {
                return;
            }
            var frm = new CameraView(writer);
            frm.Show();
        }

        private async void LoadSequence(string filePath)
        {
            var data = File.ReadAllText(filePath);
            var sequence = JsonSerializer.Deserialize<SequenceData>(data, JsonSerializerSettingsProvider.Default);
            if (sequence is null)
            {
                MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.currentSession = sequence.ToSequence();
            await InitSequenceGrid();
        }

        private async Task InitSequenceGrid()
        {
            this.grdCues.SelectionChanged -= this.SequenceGrid_SelectionChanged;

            this.grdCues.DataSource = this.currentSession.Cues;
            foreach (DataGridViewColumn c in this.grdCues.Columns)
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
            this.grdCues.SelectionChanged += this.SequenceGrid_SelectionChanged;

            var firstCue = this.currentSession?.Cues?.FirstOrDefault();
            await this.TriggerCue(firstCue);

            this.grdCameras.DataSource = this.currentSession?.Writers ?? new List<WriterData>();

            _ = UpdateCueDetails(firstCue);
        }

        private Task UpdateCueDetails(Cue? cue)
        {
            if (cue is null)
            {
                return Task.CompletedTask;
            }

            this.ctlLabel.Text = cue.Label;
            this.ctlOrdinal.Value = cue.Ordinal;

            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            this.ctlOperations.Columns.Add("Operation Type", -2, HorizontalAlignment.Left);
            this.ctlOperations.Columns.Add("Column 2", -2, HorizontalAlignment.Left);

            //Add the items to the ListView.
            this.ctlOperations.Items.AddRange(cue.Operations.Select(o =>
                new ListViewItem(o.Operation.Kind.ToString())
            ).ToArray());

            return Task.CompletedTask;
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