namespace Game.Screen
{
    public class NameInputScreen
    {
        public string InputName()
        {
            Console.Clear();
            Console.WriteLine("캐릭터 이름을 입력하세요:");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Clear();
                Console.WriteLine("이름은 공백이 될 수 없습니다. 다시 입력해주세요:");
                name = Console.ReadLine();
            }

            return name;
        }
    }
}
