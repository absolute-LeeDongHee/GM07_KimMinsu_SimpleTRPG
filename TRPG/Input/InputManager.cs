using Game.Enums;

namespace Game.Input
{
    public static class InputManager
    {
        public static KeyInput ReadKeyboard()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    return KeyInput.Up;
                case ConsoleKey.DownArrow:
                    return KeyInput.Down;
                case ConsoleKey.LeftArrow:
                    return KeyInput.Left;
                case ConsoleKey.RightArrow:
                    return KeyInput.Right;
                case ConsoleKey.Enter:
                    return KeyInput.Confirm;
                case ConsoleKey.Escape:
                    return KeyInput.Cancel;
                default:
                    return KeyInput.None;
            }
        }
    }
}
