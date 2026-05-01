using cat_facts.Model;
using System.Text.Json;

namespace cat_facts.Service
{
    
    public class CatFactService : ICatFactService
    {
       // private readonly string _filePath = "Data/catfacts.json";

        private readonly string _filePath;

        public CatFactService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "catfact.json");
        }


        public List<CatFact> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<CatFact>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<CatFact>>(json) ?? new List<CatFact>();
        }

        public CatFact GetRandom()
        {
            var facts = GetAll();
            if (!facts.Any()) return null;

            var rnd = new Random();
            return facts[rnd.Next(facts.Count)];
        }

        public CatFact Add(string factText)
        {
            var facts = GetAll();

            var newFact = new CatFact
            {
                Id = facts.Any() ? facts.Max(x => x.Id) + 1 : 1,
                Fact = factText
            };

            facts.Add(newFact);

            var json = JsonSerializer.Serialize(facts, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);

            return newFact;
        }
    }
}
