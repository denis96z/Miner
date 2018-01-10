using System;
using System.Windows.Forms;
using Miner.Data;

namespace Miner.Input
{
    public class InputManager : IInputManager
    {
        private int _selectorRow = 0, _selectorCol = 0;

        public int SelectorRow
        {
            get => _selectorRow;

            private set
            {
                if (value >= 0 && value < _field.Height)
                {
                    _selectorRow = value;
                }
            }
        }

        public int SelectorCol
        {
            get => _selectorCol;

            private set
            {
                if (value >= 0 && value < _field.Width)
                {
                    _selectorCol = value;
                }
            }
        }

        private readonly IField _field;
        private readonly Control _control;

        protected readonly IDeviceManager JoystickManager =
            Input.JoystickManager.Instance;

        public InputManager(IField field, Control control)
        {
            this._field = field ?? throw new ArgumentNullException();
            this._control = control ?? throw new ArgumentNullException();

            this._field.Modified += OnFieldModified;

            SelectorMoved += (sender, e) => { };
            JoystickManager.CommandReceived += DeviceCommandReceived;
        }

        ~InputManager()
        {
            _field.Modified -= OnFieldModified;
            JoystickManager.CommandReceived -= DeviceCommandReceived;
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
                    PerformFieldAction(() => _field
                        .RevealCell(_selectorRow, _selectorCol));
                    break;

                case DeviceCommand.MarkCell:
                    PerformFieldAction(() => _field
                        .MarkCell(_selectorRow, _selectorCol));
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
            switch (_field.State)
            {
                case FieldState.AllCellsHidden:
                case FieldState.SomeCellsMarkedOrRevealed:
                    action.Invoke();
                    break;
            }
        }

        private void OnFieldModified(object sender, FieldModType modType)
        {
            if (modType == FieldModType.Initialized)
            {
                PerformSelectorAction(() =>
                {
                    SelectorRow = 0;
                    SelectorCol = 0;
                });
            }
        }

        public event SelectorMoved SelectorMoved;
    }
}
