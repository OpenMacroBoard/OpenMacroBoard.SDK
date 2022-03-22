using System;

namespace OpenMacroBoard.SDK.Internals
{
    internal sealed class KnownDeviceInternal : IKnownDevice
    {
        public KnownDeviceInternal(IDeviceReference deviceReferenceHandle, bool connected)
        {
            DeviceReference = deviceReferenceHandle ?? throw new ArgumentNullException(nameof(deviceReferenceHandle));
            Connected = connected;
        }

        public bool Connected { get; set; }
        public IDeviceReference DeviceReference { get; }
        public IKeyLayout Keys => DeviceReference.Keys;

        public string DeviceName
        {
            get => DeviceReference.DeviceName;
            set => DeviceReference.DeviceName = value;
        }

        public override bool Equals(object obj)
        {
            return DeviceReference.Equals(obj);
        }

        public override int GetHashCode()
        {
            return DeviceReference.GetHashCode();
        }

        public IMacroBoard Open()
        {
            return DeviceReference.Open();
        }
    }
}
