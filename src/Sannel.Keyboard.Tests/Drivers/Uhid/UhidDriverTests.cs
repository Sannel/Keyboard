using Sannel.Keyboard.Drivers.Uhid;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Sannel.Keyboard.Tests.Drivers.Uhid
{
	public class UhidDriverTests
	{
		[Fact]
		public void IsAvalableTest()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				using var driver = UhidDriver.Create();
				Assert.True(driver.IsAvalable);
			}
			else
			{
				Assert.Throws<PlatformNotSupportedException>(() => UhidDriver.Create());
			}
		}

		[Fact]
		public void KeyPressTest()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				using var driver = UhidDriver.Create();
				Assert.True(driver.PressKey('a'));
			}
			else
			{
				Assert.Throws<PlatformNotSupportedException>(() => UhidDriver.Create());
			}
		}
	}
}