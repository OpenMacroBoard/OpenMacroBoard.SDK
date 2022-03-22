using System;

#nullable enable

namespace OpenMacroBoard.SDK.Internals
{
    /// <summary>
    /// Create an observer from an action.
    /// </summary>
    internal class DeviceStateObserver : IObserver<DeviceStateReport>
    {
        private readonly Action<DeviceStateReport> eventHandler;

        public DeviceStateObserver(Action<DeviceStateReport> eventHandler)
        {
            this.eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        }

        public void OnCompleted()
        {
            // intentionally left empty.
        }

        public void OnError(Exception error)
        {
            // intentionally left empty.
        }

        public void OnNext(DeviceStateReport value)
        {
            eventHandler(value);
        }
    }
}
