using Microsoft.EntityFrameworkCore;
using SpOrNotSpPt2.EF;

namespace SpOrNotSpPt2.Replicators;

public class StoredProcedureReplicator : IReplicator
{
    private readonly AppDbContext _dbContext;

    public StoredProcedureReplicator(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task CopyStructureAsync(int sourceId, int targetId)
    {
        try
        {
            await _dbContext.Database.ExecuteSqlAsync($"EXEC CopyStructure {sourceId}, {targetId}");
        }
        catch
        {
            // handle
        }
    }
}
