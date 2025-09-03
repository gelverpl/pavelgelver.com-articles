using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpOrNotSpPt2.EF;

namespace SpOrNotSpPt2.Tests;

public class AppDbContextTests
{
    [Test]
    public async Task Seeding_Should_Work_Correctly([Random(1,5,3)] int num)
    {
        await using AppDbContext context = new(Configuration.ConnectionString, num);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        Assert.That(context.Nodes.Count(), Is.EqualTo(num));
        Assert.That(context.Permissions.Count(), Is.EqualTo(num));
        Assert.That(context.Attributes.Count(), Is.EqualTo(num));
    }

    [Test]
    public async Task Write_Then_Read()
    {
        await using AppDbContext context = new(Configuration.ConnectionString);

        var newNode = new Node { StructureId = 2 };
        context.Nodes.Add(newNode);
        await context.SaveChangesAsync();

        var dbNode = await context.Nodes.FirstOrDefaultAsync(o => o.Id == newNode.Id);
        Assert.That(dbNode, Is.Not.Null);
        Assert.That(dbNode, Is.EqualTo(newNode));
    }
}
