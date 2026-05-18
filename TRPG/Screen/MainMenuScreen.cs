using Game.Enums;
using Game.Input;

namespace Game.Screen
{
    public class MainMenuScreen
    {
        private int menuCount = Enum.GetNames(typeof(MainMenuOpt)).Length;
        private MainMenuOpt selectedOpt = MainMenuOpt.NewGame;

        public MainMenuOpt Show()
        {
            while (true)
            {
                MainMenuRender();

                KeyInput input = InputManager.ReadKeyboard();
                switch (input)
                {
                    case KeyInput.Up:
                        selectedOpt = (MainMenuOpt)(((int)selectedOpt - 1 + menuCount) % menuCount);
                        break;
                    case KeyInput.Down:
                        selectedOpt = (MainMenuOpt)(((int)selectedOpt + 1) % menuCount);
                        break;
                    case KeyInput.Confirm:
                        return selectedOpt;
                }
            }
        }

        public void MainMenuRender()
        {
            Console.Clear();
            Console.WriteLine("=== TRPG Main Menu ===");
            Console.WriteLine();

            RenderMenuItem(MainMenuOpt.NewGame, selectedOpt, "새로운 게임");
            Console.WriteLine();
            RenderMenuItem(MainMenuOpt.Continue, selectedOpt, "게임 불러오기");
            Console.WriteLine();
            RenderMenuItem(MainMenuOpt.Exit, selectedOpt, "게임 종료");

        }

        private void RenderMenuItem(MainMenuOpt opt, MainMenuOpt selectedOpt, string text)
        {

            string cursor = opt == selectedOpt ? ">" : " ";
            Console.ForegroundColor = opt == selectedOpt ? ConsoleColor.Green : ConsoleColor.White;
            Console.WriteLine($"{cursor} {text}");
            Console.ResetColor();
        }
    }
}
