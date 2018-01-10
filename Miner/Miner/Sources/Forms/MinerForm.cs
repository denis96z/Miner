using System;
using System.Windows.Forms;

namespace Miner.Forms
{
    public partial class MinerForm : Form
    {
        public MinerForm()
        {
            InitializeComponent();
        }

        protected void InvokeAction(Action action)
        {
            action.Invoke();
            /*try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, String.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
    }
}
