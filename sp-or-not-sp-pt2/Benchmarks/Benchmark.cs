using BenchmarkDotNet.Attributes;
using SpOrNotSpPt2.EF;
using SpOrNotSpPt2.Replicators;

namespace SpOrNotSpPt2.Benchmarks;

[MemoryDiagnoser]
public class SimpleBenchmark
{
    private static readonly string s_connectionString = Configuration.ConnectionString;

    private const int SourceId = 0;
    private const int TargetId = 1;
    private AppDbContext _context = null!;
    private EntityFrameworkReplicator _entityFrameworkReplicator = null!;
    private StoredProcedureReplicator _storedProcedureReplicator = null!;
    private RawSqlReplicator _rawSqlReplicator = null!;

    [Params(0, 3, 6, 12, 24, 48, 96, 192)]
    public int NumberOfRecords;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _context = new AppDbContext(s_connectionString, NumberOfRecords);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        _entityFrameworkReplicator = new EntityFrameworkReplicator(_context);
        _storedProcedureReplicator = new StoredProcedureReplicator(_context);
        _rawSqlReplicator = new RawSqlReplicator(_context);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _context.Dispose();
    }

    [Benchmark(Baseline = true, Description = "EntityFramework")]
    public async Task CopyUsingEntityFramework()
    {
        await _entityFrameworkReplicator.CopyStructureAsync(SourceId, TargetId);
    }

    [Benchmark(Description = "StoredProcedure")]
    public async Task CopyUsingStoredProcedure()
    {
        await _storedProcedureReplicator.CopyStructureAsync(SourceId, TargetId);
    }

    [Benchmark(Description = "RawSQL")]
    public async Task CopyUsingRawSql()
    {
        await _rawSqlReplicator.CopyStructureAsync(SourceId, TargetId);
    }
}
