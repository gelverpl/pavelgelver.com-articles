using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SpOrNotSpPt2.EF;

public class AppDbContext : DbContext
{
    private readonly int _seedingAmount;
    private readonly string? _connectionString;
    private readonly SqlConnection? _connection;
    private readonly bool _enableLogging;

    public AppDbContext(SqlConnection connection, bool enableLogging = false)
        => (_connection, _enableLogging) = (connection, enableLogging);

    public AppDbContext(string connectionString, int seedingAmount = 0, bool enableLogging = false)
    {
        _connectionString = connectionString;
        _seedingAmount = seedingAmount;
        _enableLogging = enableLogging;
    }

    public DbSet<Node> Nodes { get; set; } = null!;

    public DbSet<Permission> Permissions { get; set; } = null!;

    public DbSet<Attribute> Attributes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder = _connection is not null
            ? optionsBuilder.UseSqlServer(connection: _connection)
            : optionsBuilder.UseSqlServer(connectionString: _connectionString);

        if (_enableLogging)
        {
            optionsBuilder = optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Debug)
                .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
        }

        optionsBuilder.UseAsyncSeeding(async (context, _, ct) =>
        {
            int nodes = await context.Set<Node>().CountAsync(ct);
            if (nodes == 0)
            {
                context.Set<Node>().AddRange(Enumerable.Range(0, _seedingAmount).Select(_ => new Node()));
            }

            int permissions = await context.Set<Permission>().CountAsync(ct);
            if (permissions == 0)
            {
                context.Set<Permission>().AddRange(Enumerable.Range(0, _seedingAmount).Select(_ => new Permission()));
            }

            int attributes = await context.Set<Attribute>().CountAsync(ct);
            if (attributes == 0)
            {
                context.Set<Attribute>().AddRange(Enumerable.Range(0, _seedingAmount).Select(_ => new Attribute()));
            }

            await context.SaveChangesAsync(ct);

            await context.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync(@".\SQL\CopyStructure.sql", ct), cancellationToken: ct);
        });
    }
}
