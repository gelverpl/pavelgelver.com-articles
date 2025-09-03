namespace SpOrNotSpPt2.Replicators;

public interface IReplicator
{
    Task CopyStructureAsync(int sourceId, int targetId);
}