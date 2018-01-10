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
        private readonly IField _field;
        private readonly IFieldView _fieldView;
        private readonly IInputManager _inputManager;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IStopwatch _stopwatch;

        private OptionsForm _optionsForm = new OptionsForm();
        private SubmitResultForm _submitResultForm = new SubmitResultForm();

        public MainForm()
        {
            InitializeComponent();

            _field = new Field(10, 10, 10);
            _inputManager = new InputManager(_field, this);
            _fieldView = new ControlFieldView(_field,
                new ControlViewAdapter(this), _inputManager);
            _soundPlayer = new WaveSoundPlayer(_field);
            _stopwatch = new StdStopwatch();
            _field.Modified += FieldModified;
        }

        ~MainForm()
        {
            _field.Modified -= FieldModified;
        }

        private void FieldModified(object sender, FieldModType modType)
        {
            InvokeAction(() =>
            {
                switch (_field.State)
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
                var result = MessageBox.Show(@"Вы уверены, что хотите завершить игру?",
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
                if (_field.State != FieldState.NotInitialized)
                {
                    _fieldView.ShowField();
                }
            });
        }

        private void miOptions_Click(object sender, EventArgs e)
        {
            InvokeAction(() =>
            {
                if (_optionsForm.ShowDialog() == DialogResult.OK)
                {
                    _field.Width = _optionsForm.FieldWidth;
                    _field.Height = _optionsForm.FieldHeight;
                    _field.NumMines = _optionsForm.NumMines;
                }
            });
        }

        private void StartGame()
        {
            _field.Initialize();
            _stopwatch.Restart();

            miStartGame.Enabled = false;
            miOptions.Enabled = false;
        }

        private void EndGame()
        {
            _stopwatch.Stop();

            miStartGame.Enabled = true;
            miOptions.Enabled = true;

            _submitResultForm.ShowDialog(_field.State == FieldState.AllMinesMarked ?
                GameResult.Win : GameResult.Loss, _stopwatch.ElapsedSeconds);
        }
    }
}
