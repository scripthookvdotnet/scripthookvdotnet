using System.Windows.Forms;

namespace SHVDN
{
    public readonly struct KeyboardEvent
    {
        public readonly KeyEventArgs Args;
        public readonly bool IsDown;

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
