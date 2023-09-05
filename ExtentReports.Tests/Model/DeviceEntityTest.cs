using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    class DeviceEntityTest
    {
        [Test]
        public void DeviceName()
        {
            var name = "DeviceName";
            var device = new Device(name);
            Assert.AreEqual(name, device.Name);
        }
    }
}
