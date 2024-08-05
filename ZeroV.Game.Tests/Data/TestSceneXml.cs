using System;
using System.IO;

using NUnit.Framework;

using ZeroV.Game.Data;

namespace ZeroV.Game.Tests.Data;

[TestFixture]
internal class TestSceneXml {

    [SetUp]
    public void SetUp() {
    }

    [Test]
    public void TestXml() {
        FileInfo file = new FileInfo("./Resources/Schema/ZeroVMap.xml");
        BeatmapWrapper wrapper = BeatmapWrapper.Create(file);
        Assert.IsNotNull(wrapper.ZeroVMap);
        wrapper.GetTrackInfo();
        wrapper.GetBeatmapByIndex(0);
        wrapper.GetBeatmapByIndex(1);
        try {
            wrapper.GetBeatmapByIndex(2);
            Assert.Fail("Expected an Exception");
        //} catch (IndexOutOfRangeException) {
        //    Console.WriteLine($"Caught {nameof(IndexOutOfRangeException)}");
        } catch (ArgumentOutOfRangeException) {
            Console.WriteLine($"Caught {nameof(ArgumentOutOfRangeException)}");
        } catch (Exception e) {
            Assert.Fail($"Expected wrong Exception type, got {e.GetType().Name} here");
        }
    }
}
