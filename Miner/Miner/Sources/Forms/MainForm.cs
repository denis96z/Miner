using System;
using System.Windows.Forms;

using Miner.Data;
using Miner.View;
using Miner.Input;
using Miner.Sound;
using Miner.Time;
using Miner.Game;

namespace Miner.Forms
{
    public partial class MainForm : MinerForm
    {
        private readonly IField field;
        private readonly IFieldView fieldView;
        private readonly IInputManager inputManager;
        private readonly ISoundPlayer soundPlayer;
        private readonly IStopwatch stopwatch;

        private OptionsForm optionsForm = new OptionsForm();
        private SubmitResultForm submitResultForm = new SubmitResultForm();

        public MainForm()
        {
            InitializeComponent();

            field = new Field(10, 10, 10);
            inputManager = new InputManager(field, this);
            fieldView = new ControlFieldView(field, this, inputManager);
            soundPlayer = new WaveSoundPlayer(field);
            stopwatch = new StdStopwatch();
            field.Modified += FieldModified;
        }

        ~MainForm()
        {
            field.Modified -= FieldModified;
        }

        private void FieldModified(object sender, FieldModType modType)
        {
            InvokeAction(() =>
            {
                switch (field.State)
                {
                    case FieldState.AllMinesMarked:
                    case FieldState.AllCellsRevealed:
                        Invoke(new Action(EndGame));
                        break;
                }
            });
        }

        private void miStartGame_Click(object sender, EventArgs e)
        {
            InvokeAction(() =>
            {
                StartGame();
            });
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
            InvokeAction(() =>
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
            });
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            InvokeAction(() =>
            {
                if (field.State != FieldState.NotInitialized)
                {
                    fieldView.ShowField();
                }
            });
        }

        private void miOptions_Click(object sender, EventArgs e)
        {
            InvokeAction(() =>
            {
                if (optionsForm.ShowDialog() == DialogResult.OK)
                {
                    field.Width = optionsForm.FieldWidth;
                    field.Height = optionsForm.FieldHeight;
                    field.NumMines = optionsForm.NumMines;
                }
            });
        }

        private void StartGame()
        {
            field.Initialize();
            stopwatch.Restart();

            miStartGame.Enabled = false;
            miOptions.Enabled = false;
        }

        private void EndGame()
        {
            stopwatch.Stop();

            miStartGame.Enabled = true;
            miOptions.Enabled = true;

            submitResultForm.ShowDialog(field.State == FieldState.AllMinesMarked ?
                GameResult.Win : GameResult.Loss, stopwatch.ElapsedSeconds);
        }
    }
}
