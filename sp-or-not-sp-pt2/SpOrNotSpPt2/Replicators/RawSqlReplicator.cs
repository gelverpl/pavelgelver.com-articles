

using Microsoft.EntityFrameworkCore;
using SpOrNotSpPt2.EF;

namespace SpOrNotSpPt2.Replicators;

public class RawSqlReplicator : IReplicator
{
    private readonly AppDbContext _dbContext;

    public RawSqlReplicator(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task CopyStructureAsync(int sourceId, int targetId)
    {
        try
        {
            await _dbContext.Database.ExecuteSqlAsync($"""
                BEGIN TRAN
                    INSERT INTO Nodes (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
                    SELECT {targetId}, SomeId1, SomeId2, SomeBool1, SomeString, Created1
                    FROM Nodes
                    WHERE StructureId = {sourceId}

                    INSERT INTO Permissions (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
                    SELECT {targetId}, SomeId1, SomeId2, SomeBool1, SomeString, Created1
                    FROM Permissions
                    WHERE StructureId = {sourceId}

                    INSERT INTO Attributes (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
                    SELECT {targetId}, SomeId1, SomeId2, SomeBool1, SomeString, Created1
                    FROM Attributes
                    WHERE StructureId = {sourceId}

                COMMIT
                """);
        }
        catch
        {
            // handle
        }
    }
}
