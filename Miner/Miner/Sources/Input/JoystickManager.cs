using System;
using System.Threading;
using SharpDX.DirectInput;

namespace Miner.Input
{
    public class JoystickManager
    {
        private Joystick joystick = null;
        private readonly DirectInput directInput = new DirectInput();

        private volatile bool shouldPoll = true;

        private static readonly JoystickManager
            instance = new JoystickManager();

        public static JoystickManager Instance
        {
            get
            {
                return Instance;
            }
        }

        public JoystickManager()
        {
            CommandReceived += (sender, c) => { };
            new Thread(() =>
            {
                while (shouldPoll)
                {
                    PollProcedure();
                }
            }).Start();
        }

        ~JoystickManager()
        {
            shouldPoll = false;
            if (joystick?.IsDisposed != true) joystick.Dispose();
            directInput.Dispose();
        }

        private void PollProcedure()
        {
            try
            {
                FindJoystick();
                PollJoystick();
            }
            catch (Exception)
            {
                if (joystick?.IsDisposed != true) joystick.Dispose();
            }
        }

        private void FindJoystick()
        {
            var joystickGuid = Guid.Empty;

            while (shouldPoll)
            {
                var devList = directInput.GetDevices(DeviceType.Gamepad,
                    DeviceEnumerationFlags.AllDevices);
                foreach (var deviceInstance in devList)
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                }

                if (joystickGuid != Guid.Empty)
                {
                    break;
                }

                devList = directInput.GetDevices(DeviceType.Joystick,
                    DeviceEnumerationFlags.AllDevices);
                foreach (var deviceInstance in directInput
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

            joystick = new Joystick(directInput, joystickGuid);
            joystick.Properties.BufferSize = 128;
            joystick.Acquire();
        }

        private void PollJoystick()
        {
            while (shouldPoll)
            {
                joystick.Poll();
                foreach (var update in joystick.GetBufferedData())
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

        public event DeviceCommandReceived CommandReceived;
    }
}
