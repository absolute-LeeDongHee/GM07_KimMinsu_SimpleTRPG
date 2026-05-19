using Game.Data.Models;
using Game.Enums;
using Game.Input;
namespace Game.Screen
{
    public class JobSelectScreen
    {
        private int selectedIndex = 0;
        public JobData? Show(Dictionary<string, JobData> jobs)
        {
            List<JobData> jobList = jobs.Values.ToList();

            while (true)
            {
                Render(jobList);

                KeyInput input = InputManager.ReadKeyboard();
                switch (input)
                {
                    case KeyInput.Up:
                        selectedIndex = (selectedIndex - 1 + jobList.Count) % jobList.Count;
                        break;
                    case KeyInput.Down:
                        selectedIndex = (selectedIndex + 1) % jobList.Count;
                        break;
                    case KeyInput.Confirm:
                        return jobList[selectedIndex];
                    case KeyInput.Cancel:
                        return null;
                }
            }
        }

        private void Render(List<JobData> jobList)
        {
            Console.Clear();
            
            Console.WriteLine("=== 직업 선택 ===");
            for (int i = 0; i < jobList.Count; i++)
            {
                string cursor = (i == selectedIndex) ? ">" : " ";
                Console.WriteLine($"{cursor} {jobList[i].Id}");
            }
            Console.WriteLine();
            Console.WriteLine("===캐릭터 기본 능력치===");
            JobData selectedJob = jobList[selectedIndex];
            Console.WriteLine($"직업: {selectedJob.Id}");
            Console.WriteLine($"체력: {selectedJob.MaxHpBonus}");
            Console.WriteLine($"마나: {selectedJob.MaxMpBonus}");
            Console.WriteLine($"공격력: {selectedJob.AtkBonus}");
            Console.WriteLine($"방어력: {selectedJob.DefBonus}");
            Console.WriteLine($"속도: {selectedJob.SpdBonus}");
            Console.WriteLine($"치명타 확률: {selectedJob.CritBonus}%");

            Console.WriteLine();
            Console.WriteLine("방향키: 이동 / Enter: 선택 확정 / ESC: 메인 메뉴로 돌아가기");
        }
    }
}
