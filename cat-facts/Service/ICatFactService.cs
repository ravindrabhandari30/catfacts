using cat_facts.Model;

namespace cat_facts.Service
{
    public interface ICatFactService
    {
        List<CatFact> GetAll();
        CatFact GetRandom();
        CatFact Add(string factText);
    }
}
