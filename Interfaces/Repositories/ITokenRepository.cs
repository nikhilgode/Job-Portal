using JobPortal_New.Modeles.Entites;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        public string GenerateToken(User user);
        void InvalidateToken(string token);
        bool IsTokenValid(string token);
    }
}
