using System.Text;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpOrNotSpPt2.EF;
using SpOrNotSpPt2.Replicators;

namespace SpOrNotSpPt2.Tests;

public class ReplicatorsTests
{
    private const int SourceStructureId = 0;
    private const int SeedNum = 3;

    private AppDbContext _context;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = new(Configuration.ConnectionString, SeedNum);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
        => await _context.DisposeAsync();

    [Test]
    public async Task EF_Replicator_Works_Correctly()
    {
        await Test_Replicator(new EntityFrameworkReplicator(_context), 10);
    }

    [Test]
    public async Task SP_Replicator_Works_Correctly()
    {
        await Test_Replicator(new StoredProcedureReplicator(_context), 20);
    }

    [Test]
    public async Task RawSQL_Replicator_Works_Correctly()
    {
        await Test_Replicator(new RawSqlReplicator(_context), 30);
    }

    private async Task Test_Replicator(IReplicator replicator, int targetId)
    {
        var expected = GetCounts(SourceStructureId);
        Assert.That(expected, Is.EqualTo((SeedNum, SeedNum, SeedNum)));

        await replicator.CopyStructureAsync(SourceStructureId, targetId);

        var sourceAfter = GetCounts(SourceStructureId);
        var target = GetCounts(targetId);

        Assert.That(sourceAfter, Is.EqualTo(expected));
        Assert.That(target, Is.EqualTo(expected));

        await ShowDbState();
    }

    private (int nodes, int permissions, int attributes) GetCounts(int structureId)
    {
        var nodesCount = _context.Nodes.Count(o => o.StructureId == structureId);
        var permissionsCount = _context.Permissions.Count(o => o.StructureId == structureId);
        var attributesCount = _context.Attributes.Count(o => o.StructureId == structureId);
        return (nodesCount, permissionsCount, attributesCount);
    }

    private async Task ShowDbState()
    {
        StringBuilder sb = new();

        foreach (var node in await _context.Nodes.ToArrayAsync())
        {
            sb.AppendLine($"Node: {node.Id}, StructureId: {node.StructureId}");
        }
        foreach (var permission in await _context.Permissions.ToArrayAsync())
        {
            sb.AppendLine($"Node: {permission.Id}, StructureId: {permission.StructureId}");
        }
        foreach (var attribute in await _context.Attributes.ToArrayAsync())
        {
            sb.AppendLine($"Node: {attribute.Id}, StructureId: {attribute.StructureId}");
        }

        Console.WriteLine("State: ");
        Console.WriteLine(sb.ToString());
    }
}
