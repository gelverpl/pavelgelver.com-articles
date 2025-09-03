using System.Collections;
using System.Text;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using SpOrNotSpPt2.EF;
using SpOrNotSpPt2.Replicators;

namespace SpOrNotSpPt2.Tests;

public class ConnectionStatisticsDemo
{
    [Test]
    public async Task Collect_Connection_Statistics_For_Ef_And_Sp_Replicators()
    {
        await PrepareDatabase();

        Dictionary<object, List<object?>> statistics = new();
        await using SqlConnection connection = new(Configuration.ConnectionString);
        connection.StatisticsEnabled = true;
        await using AppDbContext context = new(connection);

        await new EntityFrameworkReplicator(context).CopyStructureAsync(0, 1);
        AddToGeneral(connection, statistics);
        connection.ResetStatistics();

        await new StoredProcedureReplicator(context).CopyStructureAsync(0, 1);
        AddToGeneral(connection, statistics);

        ShowComparison(statistics);
    }

    private static async Task PrepareDatabase()
    {
        await using AppDbContext context = new(Configuration.ConnectionString);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    private static void ShowComparison(Dictionary<object, List<object?>> statistics)
    {
        StringBuilder sb = new();
        foreach (var (key, list) in statistics)
        {
            sb.AppendLine();
            sb.Append($"{key,-20}");
            foreach (object? value in list)
            {
                sb.Append($"{value,-8}");
            }
        }
        Console.WriteLine(sb.ToString());
    }

    private static void AddToGeneral(SqlConnection connection, Dictionary<object, List<object?>> statistics)
    {
        foreach (DictionaryEntry stat in connection.RetrieveStatistics())
        {
            if (!statistics.ContainsKey(stat.Key))
            {
                statistics.Add(stat.Key, []);
            }
            statistics[stat.Key].Add(stat.Value);
        }
    }
}
