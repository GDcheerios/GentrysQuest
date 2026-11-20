using System;
using System.IO;
using GentrysQuest.Editor;
using NUnit.Framework;

namespace GentrysQuest.Game.Tests;

[TestFixture]
public class MapEditorDocumentTests
{
    [Test]
    public void JsonRoundTripPreservesVectorObjectsAndSpawn()
    {
        MapEditorDocument document = MapEditorDocument.CreateNew("Round Trip");
        Assert.That(document.AddObject(new MapEditorObject { X = 1250, Y = 2300, Width = 850, Height = 620, HasCollider = true }), Is.True);
        Assert.That(document.SetSpawn(new MapEditorPoint(4500, 5500)), Is.True);

        MapEditorDocument loaded = MapEditorDocument.FromJson(document.ToJson());

        Assert.That(loaded.Name, Is.EqualTo("Round Trip"));
        Assert.That(loaded.Objects, Has.Count.EqualTo(1));
        Assert.That(loaded.Objects[0].X, Is.EqualTo(1250));
        Assert.That(loaded.Objects[0].Width, Is.EqualTo(850));
        Assert.That(loaded.Objects[0].HasCollider, Is.True);
        Assert.That(loaded.Spawn, Is.EqualTo(document.Spawn));
    }

    [Test]
    public void InvalidJsonAndOutOfBoundsGeometryAreRejected()
    {
        Assert.Catch<System.Text.Json.JsonException>(() => MapEditorDocument.FromJson("{"));
        Assert.Throws<InvalidDataException>(() => MapEditorDocument.FromJson("{\"Columns\": 19}"));

        MapEditorDocument document = MapEditorDocument.CreateNew();
        Assert.Throws<InvalidDataException>(() => document.AddObject(new MapEditorObject { X = -1, Y = 0, Width = 100, Height = 100 }));
        Assert.Throws<InvalidDataException>(() => document.SetSpawn(new MapEditorPoint(document.WorldWidth, 0)));
        Assert.Throws<InvalidDataException>(() => MapEditorDocument.FromJson("{\"Version\":2,\"Name\":\"Bad\",\"Spawn\":{\"X\":1,\"Y\":1},\"Objects\":[{\"X\":0,\"Y\":0,\"Width\":0,\"Height\":10}]}"));
    }

    [Test]
    public void StorageRoundTripWritesAndReopensDocument()
    {
        string directory = Path.Combine(Path.GetTempPath(), $"gentrys-quest-map-editor-{Guid.NewGuid():N}");
        string path = Path.Combine(directory, "Storage Test.json");

        try
        {
            MapEditorDocument document = MapEditorDocument.CreateNew("Storage Test");
            document.AddObject(new MapEditorObject { X = 3200, Y = 4100, Width = 500, Height = 600 });

            MapEditorStorage.Save(path, document);
            MapEditorDocument loaded = MapEditorStorage.Load(path);

            Assert.That(loaded.Name, Is.EqualTo(document.Name));
            Assert.That(loaded.Objects[0].X, Is.EqualTo(document.Objects[0].X));
            Assert.That(loaded.Objects[0].Height, Is.EqualTo(document.Objects[0].Height));
        }
        finally
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }
    }

    [Test]
    public void SpawnCannotOverlapSolidObjectAndInvalidObjectListsAreRejected()
    {
        MapEditorDocument document = MapEditorDocument.CreateNew();
        var solid = new MapEditorObject { X = 100, Y = 100, Width = 500, Height = 500, HasCollider = true };
        Assert.That(document.AddObject(solid), Is.True);
        Assert.That(document.AddObject(solid.Copy()), Is.True);
        Assert.That(document.SetSpawn(new MapEditorPoint(200, 200)), Is.False);
        Assert.That(document.AddObject(new MapEditorObject { X = 9900, Y = 6900, Width = 200, Height = 200, HasCollider = true }), Is.False);

        document.Objects.Add(new MapEditorObject { X = float.NaN, Y = 0, Width = 100, Height = 100 });
        Assert.Throws<InvalidDataException>(() => document.Validate());
    }

    [Test]
    public void ExportCreatesGameMapWithBoundariesSpawnAndWalls()
    {
        MapEditorDocument document = MapEditorDocument.CreateNew("My Test Map");
        document.AddObject(new MapEditorObject { X = 1234, Y = 2345, Width = 876, Height = 654, HasCollider = true, Filled = false });
        document.SetSpawn(new MapEditorPoint(4500, 5500));

        string source = MapSourceExporter.Generate(document);

        Assert.That(source, Does.Contain("public class My_Test_Map : Map"));
        Assert.That(source, Does.Contain("base.Load();"));
        Assert.That(source, Does.Contain("AllowRandomSpawning = false;"));
        Assert.That(source, Does.Contain("SpawnPoint = new Vector2(4500f, 5500f);"));
        Assert.That(source, Does.Contain("Position = new Vector2(1234f, 2345f),"));
        Assert.That(source, Does.Contain("Size = new Vector2(876f, 654f),"));
        Assert.That(source, Does.Contain("HasCollider = true"));
        Assert.That(source, Does.Contain("Filled = false"));
    }

    [Test]
    public void VersionOneTileMapsAreMigratedToWorldRectangles()
    {
        const string oldDocument = "{\"Version\":1,\"Name\":\"Old Map\",\"Columns\":20,\"Rows\":14,\"TileWorldSize\":1000,\"Spawn\":{\"X\":4,\"Y\":5},\"Walls\":[{\"X\":2,\"Y\":3}]}";

        MapEditorDocument migrated = MapEditorDocument.FromJson(oldDocument);

        Assert.That(migrated.Version, Is.EqualTo(MapEditorDocument.CurrentVersion));
        Assert.That(migrated.Spawn, Is.EqualTo(new MapEditorPoint(4500, 5500)));
        Assert.That(migrated.Objects, Has.Count.EqualTo(1));
        Assert.That(migrated.Objects[0].X, Is.EqualTo(2000));
        Assert.That(migrated.Objects[0].Y, Is.EqualTo(3000));
        Assert.That(migrated.Objects[0].Width, Is.EqualTo(1000));
        Assert.That(migrated.Objects[0].HasCollider, Is.True);
    }
}