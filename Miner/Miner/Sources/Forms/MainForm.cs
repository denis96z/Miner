using System;
using System.Windows.Forms;

using Miner.Data;
using Miner.View;
using Miner.Input;
using Miner.Sound;

namespace Miner.Forms
{
    public partial class MainForm : Form
    {
        private readonly IField field;
        private readonly IFieldView fieldView;
        private readonly IInputManager inputManager;
        private readonly ISoundPlayer soundPlayer;

        public MainForm()
        {
            InitializeComponent();

            field = new Field(10, 10, 10);
            inputManager = new InputManager(field, this);
            fieldView = new ControlFieldView(field, this, inputManager);
            soundPlayer = new WaveSoundPlayer(field);
            field.Modified += FieldModified;
        }

        ~MainForm()
        {
            field.Modified -= FieldModified;
        }

        private void FieldModified(object sender, FieldModType modType)
        {
            miStartGame.Enabled = miSetup.Enabled = field.State !=
                FieldState.SomeCellsMarkedOrRevealed &&
                field.State != FieldState.AllCellsHidden;
        }

        private void miStartGame_Click(object sender, EventArgs e)
        {
            field.Initialize();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            mainMenu.Visible = e.Y <= mainMenu.Height;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите завершить игру?",
                String.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                JoystickManager.Instance.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void HandleUserAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, String.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (field.State != FieldState.NotInitialized)
            {
                fieldView.ShowField();
            }
        }
    }
}
