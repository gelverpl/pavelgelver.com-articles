using NUnit.Framework;
using SpOrNotSpPt2.EF;

namespace SpOrNotSpPt2.Tests;

public class LogToDemo
{
    [Test]
    public async Task Log_Writes_And_Reads()
    {
        await using AppDbContext context = new(Configuration.ConnectionString, enableLogging: true);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Nodes.Add(new Node());
        context.Permissions.Add(new Permission());

        await context.SaveChangesAsync();
    }
}
