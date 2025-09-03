CREATE PROCEDURE CopyStructure
    @SourceStructureId INT,
    @TargetStructureId INT
AS
BEGIN
    BEGIN TRAN
        INSERT INTO Nodes (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
        SELECT @TargetStructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1
        FROM Nodes
        WHERE StructureId = @SourceStructureId

        INSERT INTO Permissions (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
        SELECT @TargetStructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1
        FROM Permissions
        WHERE StructureId = @SourceStructureId

        INSERT INTO Attributes (StructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1)
        SELECT @TargetStructureId, SomeId1, SomeId2, SomeBool1, SomeString, Created1
        FROM Attributes
        WHERE StructureId = @SourceStructureId

    COMMIT
END;
