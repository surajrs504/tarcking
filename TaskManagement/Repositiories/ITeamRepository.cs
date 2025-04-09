using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public interface ITeamRepository
    {
        Task<TeamDomain?> GetTeam(Guid teamId);
        Task<TeamDomain> Add(TeamDomain team);
        Task<TeamDomain?> Delete(Guid teamId);
        Task<TeamDomain> Update(Guid teamId, TeamDomain team);
    }
}
