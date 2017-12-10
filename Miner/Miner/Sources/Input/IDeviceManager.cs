using System;

namespace Miner.Input
{
    public interface IDeviceManager
    {
        event DeviceCommandReceived CommandReceived;
    }

    public delegate void DeviceCommandReceived(object sender, DeviceCommand command);
}
