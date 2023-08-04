using ViewMaster.Core.Models;
using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.DesktopController
{
    public partial class MainWindow : Form
    {
        private Session currentSession = new(new Sequence("New Session", new List<Cue>()));
        private CancellationTokenSource cancellationTokenSource = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            var writers = new List<IWriter>() { new LogWriter() };
            this.currentSession = new(new Sequence("Run in a circle", new List<Cue> {
                // set zoom to a specific level
                new Cue(1, "Zoom", new List<CueTarget>{new CueTarget(writers, new ZoomOperation(1000)) }),
                new Cue(2, "Pan Left", new List<CueTarget>{new CueTarget(writers, new PanOperation(new Degrees(180, 90), 280, TimeSpan.FromSeconds(15), 0.20, -10))}),
                new Cue(3, "Move",new List<CueTarget>{new CueTarget( writers, new MoveOperation(new Degrees(180, 90)))}),
            }));
            initGrid();
        }

        private void initGrid()
        {
            this.sequenceGrid.DataSource = this.currentSession.Sequence.Cues;
            foreach (DataGridViewColumn c in this.sequenceGrid.Columns)
            {
                if (c.Name == nameof(Cue.Targets))
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
                    c.Width = 80;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void loadSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "ViewMaster files (*.vwm)|*.vwm|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = openFileDialog.FileName;

                var sequence = System.Text.Json.JsonSerializer.Deserialize<SequenceData>(filePath);
                if (sequence is null)
                {
                    MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.currentSession = new Session(sequence);
            }
        }

        private void saveSequenceToolStripMenuItem_Click(object sender, EventArgs e)
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

                var sequence = System.Text.Json.JsonSerializer.Serialize(this.currentSession.Sequence);
                if (sequence is null)
                {
                    MessageBox.Show("File was in the wrong format!", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.WriteAllText(filePath, sequence);
            }
        }

        private async void sequenceGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.sequenceGrid?.CurrentRow?.DataBoundItem is not Cue cue)
            {
                return;
            }
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource = new CancellationTokenSource();
            await cue.Execute(this.cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}