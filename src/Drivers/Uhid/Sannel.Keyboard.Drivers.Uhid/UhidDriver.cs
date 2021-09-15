using System;
using System.Runtime.InteropServices;

namespace Sannel.Keyboard.Drivers.Uhid
{
	/// <summary>
	/// Represents a keyboard driver using /dev/uhid on linux
	/// </summary>
	public class UhidDriver : IKeyboardDriver
	{
		private readonly int fileId;
		private UhidDriver(string keyboardName)
		{
			if(string.IsNullOrWhiteSpace(nameof(keyboardName)))
			{
				throw new ArgumentNullException(nameof(keyboardName));
			}

			if (!System.IO.File.Exists(UhidFile))
			{
				fileId = -1;
				return;
			}

			fileId = uhiddriver.OpenUHID();
			if (fileId > 0)
			{
				var status = uhiddriver.CreateDevice(fileId, keyboardName);
				if (status != 0)
				{
					uhiddriver.CloseUHID(fileId);
					fileId = 0;
				}
			}
		}

		public static IKeyboardDriver Create()
			=> Create("sannel-virtual-uhid-keyboard");

		/// <summary>
		/// Creates a new UhidDriver instance if its supported on the running architecture.
		/// </summary>
		/// <param name="keyboardName">The name of the keyboard to create in uhid</param>
		/// <returns></returns>
		public static IKeyboardDriver Create(string keyboardName)
		{
			if(string.IsNullOrWhiteSpace(nameof(keyboardName)))
			{
				throw new ArgumentNullException(nameof(keyboardName));
			}

			if(!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				throw new PlatformNotSupportedException("Only Linux is currently supported by the Uhid driver");
			}

			return RuntimeInformation.OSArchitecture switch
			{
				Architecture.Arm => new UhidDriver(keyboardName),
				Architecture.Arm64 => new UhidDriver(keyboardName),
				Architecture.X64 => new UhidDriver(keyboardName),
				_ => throw new PlatformNotSupportedException("Only x64, arm and arm64 are supported by the Uhid driver")
			};
		}


		private string uhidFile;
		public string UhidFile
			=> uhidFile ??= Marshal.PtrToStringAnsi(uhiddriver.GetUhidFile());

		/// <summary>
		/// Is the device created and ready for sending events
		/// </summary>
		public bool IsAvalable
			=> fileId > 0;

		private byte toKeyboardCode(char key)
			=> char.ToLower(key) switch
			{
				'a' => 0x04,
				'b' => 0x05,
				'c' => 0x06,
				'd' => 0x07,
				'e' => 0x08,
				'f' => 0x09,
				'g' => 0x0a,
				'h' => 0x0b,
				'i' => 0x0c,
				'j' => 0x0d,
				'k' => 0x0e,
				'l' => 0x0f,
				'm' => 0x10,
				'n' => 0x11,
				'o' => 0x12,
				'p' => 0x13,
				'q' => 0x14,
				'r' => 0x15,
				's' => 0x16,
				't' => 0x17,
				'u' => 0x18,
				'v' => 0x19,
				'w' => 0x1a,
				'x' => 0x1b,
				'y' => 0x1c,
				'z' => 0x1d,
				'1' => 0x1e,
				'2' => 0x1f,
				'3' => 0x20,
				'4' => 0x21,
				'5' => 0x22,
				'6' => 0x23,
				'7' => 0x24,
				'8' => 0x25,
				'9' => 0x26,
				'0' => 0x27,
				'\n' => 0x28,
				'\t' => 0x2b,
				' ' => 0x2c,
				'-' => 0x2d,
				'=' => 0x2e,
				'{' => 0x2f,
				'}' => 0x30,
				'\\' => 0x31,
				';' => 0x33,
				'\'' => 0x36,
				'.' => 0x37,
				'/' => 0x38,
				SpecialKeys.Esc => 0x29,
				SpecialKeys.Backspace => 0x2a,
				SpecialKeys.F1 => 0x3a,
				SpecialKeys.F2 => 0x3b,
				SpecialKeys.F3 => 0x3c,
				SpecialKeys.F4 => 0x3d,
				SpecialKeys.F5 => 0x3e,
				SpecialKeys.F6 => 0x3f,
				SpecialKeys.F7 => 0x40,
				SpecialKeys.F8 => 0x41,
				SpecialKeys.F9 => 0x42,
				SpecialKeys.F10 => 0x43,
				SpecialKeys.F11 => 0x44,
				SpecialKeys.F12 => 0x45,
				SpecialKeys.F13 => 0x68,
				SpecialKeys.F14 => 0x69,
				SpecialKeys.F15 => 0x6a,
				SpecialKeys.F16 => 0x6b,
				SpecialKeys.F17 => 0x6c,
				SpecialKeys.F18 => 0x6d,
				SpecialKeys.F19 => 0x6e,
				SpecialKeys.F20 => 0x6f,
				SpecialKeys.F21 => 0x70,
				SpecialKeys.F22 => 0x71,
				SpecialKeys.F23 => 0x72,
				SpecialKeys.F24 => 0x73,
				SpecialKeys.Control => 0xe0,
				SpecialKeys.Shift => 0xe1,
				SpecialKeys.Alt => 0xe2,
				SpecialKeys.Insert => 0x49,
				SpecialKeys.Home => 0x4A,
				SpecialKeys.PageUp => 0x4B,
				SpecialKeys.Delete => 0x4C,
				SpecialKeys.End => 0x4D,
				SpecialKeys.PageDown => 0x4E,
				SpecialKeys.Right => 0x4F,
				SpecialKeys.Left => 0x50,
				SpecialKeys.Down => 0x51,
				SpecialKeys.Up => 0x52,
				SpecialKeys.Mute => 0x7F,
				SpecialKeys.VolumeUp => 0x80,
				SpecialKeys.VolumeDown => 0x81,
				_ => 0
			};

		/// <summary>
		/// Sends a keypress through the virtual keyboard
		/// </summary>
		/// <param name="key"></param>
		/// <param name="includeShift"></param>
		/// <returns></returns>
		public bool PressKey(char key, bool includeShift = false)
		{
			if(!IsAvalable)
			{
				return false;
			}

			if(includeShift)
			{
				var r2 = uhiddriver.SendEventKey(fileId, toKeyboardCode(SpecialKeys.Shift));
				if(r2 != 0)
				{
					return false;
				}
			}

			var result = uhiddriver.SendEventKey(fileId, toKeyboardCode(key));
			if (result != 0)
			{
				return false;
			}

			// reset keyboard so key is not stuck on
			result = uhiddriver.SendEventKey(fileId, 0);

			return result == 0;
		}

		/// <summary>
		/// Lets clean up this keyboard
		/// </summary>
		public void Dispose()
		{
			if (fileId > 0)
			{
				uhiddriver.DestroyDevice(fileId);
				uhiddriver.CloseUHID(fileId);
			}
			GC.SuppressFinalize(this);
		}

		private class uhiddriver
		{
			private const string lib = "uhid";
			[DllImport(lib)]
			public static extern IntPtr GetUhidFile();

			[DllImport(lib)]
			public static extern int OpenUHID();

			[DllImport(lib, CharSet = CharSet.Ansi)]
			public static extern int CreateDevice(int fileId, string deviceName);

			[DllImport(lib)]
			public static extern void DestroyDevice(int fileId);

			[DllImport(lib)]
			public static extern void CloseUHID(int fileId);

			[DllImport(lib)]
			public static extern int SendEventKey(int fileId, byte key);
		}
	}
}