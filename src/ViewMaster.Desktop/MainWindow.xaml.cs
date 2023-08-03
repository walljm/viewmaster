using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var writers = new List<IWriter> { new LogWriter() };

            var cue1 = new Cue(1, "Cue1", writers, new CircleOperation(TimeSpan.FromSeconds(30), 0.25));
            var cue2 = new Cue(2, "Cue2", writers, new PanOperation(new Coordinate(8000, 8000), 95, TimeSpan.FromSeconds(10), 0.25));
            var cue3 = new Cue(3, "Cue3", writers, new MoveOperation(new Coordinate(8000, 8000)));

            ObservableCollection<Cue> cueList = new() { cue1, cue2, cue3 };
            CueGrid.DataContext = cueList;
            CueGrid.CurrentCellChanged += this.CueGrid_CurrentCellChanged;
        }

        private void CueGrid_CurrentCellChanged(object? sender, EventArgs e)
        {
            var cue = (Cue)CueGrid.CurrentItem;
            Debug.WriteLine(cue.Label);
            //cue.Execute().Wait();
        }
    }
}
