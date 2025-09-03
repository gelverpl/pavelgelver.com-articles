using Microsoft.EntityFrameworkCore;

namespace SpOrNotSpPt2.EF;

[PrimaryKey(nameof(Id))]
[Index(nameof(StructureId))]
public abstract class StructureEntity
{
    public int Id { get; set; }

    public int StructureId { get; set; }

    public int SomeId1 { get; set; }

    public int SomeId2 { get; set; }

    public bool SomeBool1 { get; set; }

    public string SomeString { get; set; } = Guid.NewGuid().ToString();

    public DateTimeOffset Created1 { get; set; } = DateTimeOffset.UtcNow;
}

public class Node : StructureEntity;

public class Permission : StructureEntity;

public class Attribute : StructureEntity;
