using System;

namespace Sannel.Keyboard
{
    public interface IKeyboardDriver : IDisposable
    {
        bool IsAvalable {get;}

        bool PressKey(char key, bool includeShift=false);
    }
}