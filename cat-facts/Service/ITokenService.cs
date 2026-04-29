namespace cat_facts.Service
{
    public interface ITokenService
    {
        string GenerateToken(string user);
    }
}
