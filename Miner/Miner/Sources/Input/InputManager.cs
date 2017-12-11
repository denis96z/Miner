using System;
using System.Windows.Forms;
using Miner.Data;

namespace Miner.Input
{
    public class InputManager : IInputManager
    {
        private int selectorRow = 0, selectorCol = 0;

        public int SelectorRow
        {
            get => selectorRow;

            set
            {
                if (value >= 0 && value < field.Height)
                {
                    selectorRow = value;
                }
            }
        }

        public int SelectorCol
        {
            get => selectorCol;

            set
            {
                if (value >= 0 && value < field.Width)
                {
                    selectorCol = value;
                }
            }
        }

        private readonly IField field;
        private readonly Control control;

        public InputManager(IField field, Control control)
        {
            this.field = field ?? throw new ArgumentNullException();
            this.control = control ?? throw new ArgumentNullException();

            SelectorMoved += (sender, e) => { };
            JoystickManager.Instance.CommandReceived += DeviceCommandReceived;
        }

        ~InputManager()
        {
            JoystickManager.Instance.CommandReceived -= DeviceCommandReceived;
        }

        private void DeviceCommandReceived(object sender, DeviceCommand command)
        {
            switch (command)
            {
                case DeviceCommand.MoveUp:
                    PerformSelectorAction(() => SelectorRow--);
                    break;

                case DeviceCommand.MoveDown:
                    PerformSelectorAction(() => SelectorRow++);
                    break;

                case DeviceCommand.MoveLeft:
                    PerformSelectorAction(() => SelectorCol--);
                    break;

                case DeviceCommand.MoveRight:
                    PerformSelectorAction(() => SelectorCol++);
                    break;

                case DeviceCommand.RevealCell:
                    PerformFieldAction(() => field
                        .RevealCell(selectorRow, selectorCol));
                    break;

                case DeviceCommand.MarkCell:
                    PerformFieldAction(() => field
                        .MarkCell(selectorRow, selectorCol));
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void PerformSelectorAction(Action action)
        {
            action.Invoke();
            SelectorMoved.Invoke(this, EventArgs.Empty);
        }

        private void PerformFieldAction(Action action)
        {
            switch (field.State)
            {
                case FieldState.AllCellsHidden:
                case FieldState.SomeCellsMarkedOrRevealed:
                    action.Invoke();
                    break;
            }
        }

        public event SelectorMoved SelectorMoved;
    }
}
