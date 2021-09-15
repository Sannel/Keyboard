using System;

namespace Sannel.Keyboard
{
	/// <summary>
	/// The interface representing a virtual keyboard driver
	/// </summary>
	public interface IKeyboardDriver : IDisposable
	{
		/// <summary>
		/// Is the underlying device available to send events to
		/// </summary>
		bool IsAvalable { get; }

		/// <summary>
		/// fires a key press of a key if <paramref name="includeShift"/> is true the shift key will be pressed also
		/// </summary>
		/// <param name="key"></param>
		/// <param name="includeShift"></param>
		/// <returns></returns>
		bool PressKey(char key, bool includeShift = false);
	}
}