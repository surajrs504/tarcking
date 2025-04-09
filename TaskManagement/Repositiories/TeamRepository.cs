using Microsoft.EntityFrameworkCore;
using TaskManagement.DataContext;
using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TaskManagmentDbContext taskManagmentDbContext;

        public TeamRepository(TaskManagmentDbContext taskManagmentDbContext)
        {
            this.taskManagmentDbContext = taskManagmentDbContext;
        }

        public async Task<TeamDomain?> GetTeam(Guid teamId)
        {
            var team = await taskManagmentDbContext.Team.FirstOrDefaultAsync(x => x.Id == teamId);

            if (team == null)
            {
                return null;
            }

            return team;

        }

        public async Task<TeamDomain> Add(TeamDomain team)
        {
            await taskManagmentDbContext.Team.AddAsync(team);
            await taskManagmentDbContext.SaveChangesAsync();
            return team;
        }

        public async Task<TeamDomain?> Delete(Guid teamId)
        {
            var team = await taskManagmentDbContext.Team.FirstOrDefaultAsync(x => x.Id == teamId);

            if (team == null)
            {
                return null;
            }

            taskManagmentDbContext.Team.Remove(team);
            await taskManagmentDbContext.SaveChangesAsync();
            return team;

        }

        public async Task<TeamDomain> Update(Guid Id, TeamDomain team)
        {
            var teamDomain = await taskManagmentDbContext.Team.FirstOrDefaultAsync(x => x.Id == Id);

            if (teamDomain == null)
            {
                return null;
            }
            teamDomain.teamName = team.teamName;
            teamDomain.teamDescription = team.teamDescription;
            teamDomain.teamAdmin = team.teamAdmin;

            await taskManagmentDbContext.SaveChangesAsync();

            return teamDomain;

        }
    }
}
