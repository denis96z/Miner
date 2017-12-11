using System;

namespace Miner.Input
{
    public interface IDeviceManager : IDisposable
    {
        event DeviceCommandReceived CommandReceived;
    }

    public delegate void DeviceCommandReceived(object sender, DeviceCommand command);
}
