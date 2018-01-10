using System;
using System.Threading;
using SharpDX;
using SharpDX.DirectInput;

namespace Miner.Input
{
    public class JoystickManager : IDeviceManager
    {
        private Joystick _joystick = null;
        private readonly DirectInput _directInput = new DirectInput();

        private volatile bool _shouldPoll = true;

        private static readonly JoystickManager
            _instance = new JoystickManager();

        public static JoystickManager Instance => _instance;

        public JoystickManager()
        {
            CommandReceived += (sender, c) => { };
            new Thread(() =>
            {
                while (_shouldPoll)
                {
                    PollProcedure();
                }
            }).Start();
        }

        private void PollProcedure()
        {
            try
            {
                FindJoystick();
                PollJoystick();
            }
            catch (SharpDXException)
            {
                if (_joystick != null && !_joystick.IsDisposed)
                {
                    _joystick.Dispose();
                }
            }
        }

        private void FindJoystick()
        {
            var joystickGuid = Guid.Empty;

            while (_shouldPoll)
            {
                var devList = _directInput.GetDevices(DeviceType.Gamepad,
                    DeviceEnumerationFlags.AllDevices);
                foreach (var deviceInstance in devList)
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                }

                if (joystickGuid != Guid.Empty)
                {
                    break;
                }

                devList = _directInput.GetDevices(DeviceType.Joystick,
                    DeviceEnumerationFlags.AllDevices);
                foreach (var deviceInstance in _directInput
                        .GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                }

                if (joystickGuid != Guid.Empty)
                {
                    break;
                }

                devList.Clear();
                devList = null;
                GC.Collect();

                Thread.Sleep(1000);
            }

            if (_directInput != null && !_directInput.IsDisposed)
            {
                _joystick = new Joystick(_directInput, joystickGuid);
                _joystick.Properties.BufferSize = 128;
                _joystick.Acquire();
            }
        }

        private void PollJoystick()
        {
            while (_shouldPoll)
            {
                _joystick.Poll();
                foreach (var update in _joystick.GetBufferedData())
                {
                    var command = ParseCommand(update);
                    if (command != DeviceCommand.Other)
                    {
                        CommandReceived.Invoke(this, command);
                    }
                }
            }
        }

        private DeviceCommand ParseCommand(JoystickUpdate update)
        {
            var command = DeviceCommand.Other;
            switch (update.Offset)
            {
                case JoystickOffset.X:
                    if (update.Value == 0)
                    {
                        command = DeviceCommand.MoveLeft;
                    }
                    else if (update.Value == 65535)
                    {
                        command = DeviceCommand.MoveRight;
                    }
                    break;

                case JoystickOffset.Y:
                    if (update.Value == 0)
                    {
                        command = DeviceCommand.MoveUp;
                    }
                    else if (update.Value == 65535)
                    {
                        command = DeviceCommand.MoveDown;
                    }
                    break;

                case JoystickOffset.Buttons0:
                    if (update.Value == 128)
                    {
                        command = DeviceCommand.RevealCell;
                    }
                    break;

                case JoystickOffset.Buttons1:
                    if (update.Value == 128)
                    {
                        command = DeviceCommand.MarkCell;
                    }
                    break;
            }
            return command;
        }

        public void Dispose()
        {
            _shouldPoll = false;
            if (_joystick != null && !_joystick.IsDisposed)
            {
                _joystick.Dispose();
            }
            _directInput.Dispose();
        }

        public event DeviceCommandReceived CommandReceived;
    }
}
