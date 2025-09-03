using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpOrNotSpPt2.EF;

namespace SpOrNotSpPt2.Tests;

public class ToQueryStringDemo
{
    [Test]
    public async Task Get_Query_String_Then_Output()
    {
        await using AppDbContext context = new(Configuration.ConnectionString);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var queryable = context.Nodes
            .Where(o => o.Id > 15)
            .Take(10)
            .Select(o => new { o.Id, o.Created1 });
        var queryString = queryable.ToQueryString();
        Console.WriteLine(queryString);
    }
}
