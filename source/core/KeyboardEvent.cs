using System.Windows.Forms;

namespace SHVDN
{
    public struct KeyboardEvent
    {
        public KeyEventArgs Args;
        public bool IsDown;

        public KeyboardEvent(KeyEventArgs args)
        {
            Args = args;
            IsDown = false;
        }

        public KeyboardEvent(bool isKeyDown, KeyEventArgs args)
        {
            Args = args;
            IsDown = isKeyDown;
        }
    }
}
