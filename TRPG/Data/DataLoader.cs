using System.Text.Json;
using Game.Data.Models;
using Game.Interfaces;
using System.Reflection;

namespace Game.Data
{
    public static class DataLoader
    {
        public static Dictionary<string, JobData> jobs = new Dictionary<string, JobData>();
        public static Dictionary<string, MonsterData> monsters = new Dictionary<string, MonsterData>();


        // 직업, 몬스터, 아이템, 스킬 등 고유의 string 형식의 ID를 가진 데이터들을 JSON 파일에서 읽어와 Dictionary 형태로 반환
        public static Dictionary<string, T> LoadData<T>(string filePath) where T : IHasId
        {

            string json = File.ReadAllText(filePath);
            List<T>? dataList = JsonSerializer.Deserialize<List<T>>(json);

            Dictionary<string, T> dataDict = new Dictionary<string, T>();

            if (dataList == null)
            {
                Console.WriteLine($"{filePath}로 부터 데이터를 읽어오지 못했습니다.");
                return new Dictionary<string, T>();
            }

            foreach (T data in dataList)
            {
                dataDict.Add(data.Id, data);
            }

            return dataDict;
        }
    }
}
