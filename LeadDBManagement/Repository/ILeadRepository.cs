using LeadDBManagement.Model;

namespace LeadDBManagement.Repository
{
    public interface ILeadRepository
    {
        Task<Leaddata> CreateLeadAsync(Leaddata lead);
        Task<List<Leaddata>> GetAllLeadsAsync();
        Task<Leaddata> GetLeadByIdAsync(string id);
        Task<Leaddata> UpdateLeadAsync(Leaddata lead);
        Task DeleteLeadAsync(string id);
    }
}
