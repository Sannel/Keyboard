using System;
using Xunit;

namespace Sannel.Keyboard.Tests.Drivers.Uhid
{
    public class UhidDriverTests
    {
        [Fact]
        public void IsAvalableTest()
        {
            using var driver = new Sannel.Keyboard.Driver.Uhid.UhidDriver();
            Assert.True(driver.IsAvalable);
        }

        [Fact]
        public void KeyPressTest()
        {
            using var driver = new Sannel.Keyboard.Driver.Uhid.UhidDriver();
            Assert.True(driver.PressKey(ConsoleKey.A));
        }
    }
}