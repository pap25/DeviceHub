using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Abstractions
{
    public interface IDeviceDriver
    {
        void NotifyLisIssueApplication();

        void NotifyReceiveTask();
    }
}
