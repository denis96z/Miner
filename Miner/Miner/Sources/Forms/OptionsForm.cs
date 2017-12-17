using System;
using System.Windows.Forms;

namespace Miner.Forms
{
    public partial class OptionsForm : MinerForm
    {
        public int FieldWidth { get; private set; }
        public int FieldHeight { get; private set; }
        public int NumMines { get; private set; }

        public OptionsForm()
        {
            InitializeComponent();

            FieldWidth = (int)nudFieldWidth.Value;
            FieldHeight = (int)nudFieldHeight.Value;
            NumMines = (int)nudNumMines.Value;
            nudNumMines.Maximum = nudFieldWidth.Value * nudFieldHeight.Value;

            nudFieldWidth.ValueChanged += OnFieldWidthChanged;
            nudFieldHeight.ValueChanged += OnFieldHeightChanged;
            nudNumMines.ValueChanged += OnNumMinesChanged;
        }

        private void OnFieldWidthChanged(object sender, EventArgs e)
        {
            FieldWidth = (int)nudFieldWidth.Value;
        }

        private void OnFieldHeightChanged(object sender, EventArgs e)
        {
            FieldHeight = (int)nudFieldHeight.Value;
        }

        private void OnNumMinesChanged(object sender, EventArgs e)
        {
            NumMines = (int)nudNumMines.Value;
        }

        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            nudNumMines.Maximum = nudFieldWidth.Value * nudFieldHeight.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
