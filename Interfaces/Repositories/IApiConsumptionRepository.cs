using JobPortal_New.Modeles.Entites;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface IApiConsumptionRepository
    {

        Task<List<User>> getCandidatesByApi();
        Task<List<User>> getCandidatesByMvc();
    }
}
