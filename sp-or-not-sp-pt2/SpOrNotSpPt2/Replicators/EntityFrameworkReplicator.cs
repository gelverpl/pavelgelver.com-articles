using Microsoft.EntityFrameworkCore;
using SpOrNotSpPt2.EF;
using Attribute = SpOrNotSpPt2.EF.Attribute;

namespace SpOrNotSpPt2.Replicators;

public class EntityFrameworkReplicator : IReplicator
{
    private readonly AppDbContext _dbContext;

    public EntityFrameworkReplicator(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task CopyStructureAsync(int sourceId, int targetId)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await CopyAsync<Node>(sourceId, targetId);
            await CopyAsync<Permission>(sourceId, targetId);
            await CopyAsync<Attribute>(sourceId, targetId);

            await transaction.CommitAsync();
        }
        catch
        {
            // handle
        }
    }

    private async Task CopyAsync<T>(int sourceId, int targetId) where T : StructureEntity, new()
    {
        IAsyncEnumerable<T> sourceObjects = _dbContext.Set<T>()
            .Where(node => node.StructureId == sourceId)
            .AsAsyncEnumerable();

        await foreach(var obj in sourceObjects)
        {
            var newNode = CreateCopy(obj, targetId);
            _dbContext.Set<T>().Add(newNode);
        }

        await _dbContext.SaveChangesAsync();
    }

    private static T CreateCopy<T>(T source, int targetId) where T : StructureEntity, new()
    {
        return new T
        {
            StructureId = targetId,
            SomeId1 = source.SomeId1,
            SomeId2 = source.SomeId2,
            SomeBool1 = source.SomeBool1,
            SomeString = source.SomeString,
            Created1 = source.Created1
        };
    }
}
