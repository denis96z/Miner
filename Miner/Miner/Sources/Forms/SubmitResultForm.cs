using System;
using System.Windows.Forms;

using Miner.Game;
using Miner.Database;
using System.Drawing;

namespace Miner.Forms
{
    public partial class SubmitResultForm : MinerForm
    {
        public int? Time { get; set; } = null;
        public GameResult? Result { get; set; } = null;

        private IDatabaseManager _databaseManager = new PgSqlManager();

        public SubmitResultForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(GameResult result, int time)
        {
            Time = time;
            Result = result;
            ShowResult();

            return ShowDialog();
        }

        private void lblRegistration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InvokeAction(() =>
            {
                CheckLoginPassword();
                _databaseManager.Register(tbLogin.Text, tbPassword.Text);
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
            DialogResult = DialogResult.Cancel;
        }

        private void btnSubmitResult_Click(object sender, EventArgs e)
        {
            InvokeAction(() =>
            {
                CheckLoginPassword();
                _databaseManager.SubmitResult(tbLogin.Text,
                    tbPassword.Text, (GameResult)Result, (int)Time);

                ClearData();
                DialogResult = DialogResult.OK;
            });
        }

        private void CheckLoginPassword()
        {
            int invalidCharIndex = Authentication.CheckLogin(tbLogin.Text);
            if (invalidCharIndex != -1)
            {
                throw new Exception("Логин содержит недопустимый " +
                    "символ в позиции " + (invalidCharIndex + 1) + ".");
            }

            invalidCharIndex = Authentication.CheckPassword(tbPassword.Text);
            if (invalidCharIndex != -1)
            {
                throw new Exception("Пароль содержит недопустимый " +
                    "символ в позиции " + (invalidCharIndex + 1) + ".");
            }
        }

        private void ShowResult()
        {
            switch (Result)
            {
                case GameResult.Loss:
                    lblResult.Text = "Поражение";
                    lblResult.ForeColor = Color.Red;
                    break;

                case GameResult.Win:
                    lblResult.Text = "Победа";
                    lblResult.ForeColor = Color.DarkGreen;
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void ClearData()
        {
            tbLogin.Text = String.Empty;
            tbPassword.Text = String.Empty;

            Result = null;
            Time = null;
        }
    }
}
