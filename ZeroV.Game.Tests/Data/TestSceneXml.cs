using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using NUnit.Framework;

using Vortice;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Tests.Data;

[TestFixture]
internal class TestSceneXml {
    private String? XmlSource;

    private static MemoryStream GetMemoryStream(String source) {
        return new MemoryStream(Encoding.UTF8.GetBytes(source));
    }

    [SetUp]
    public void SetUp() {
        this.XmlSource = """
            <?xml version="1.0" encoding="utf-8"?>
            <ZeroVMap xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" MapVersion="1.0.0.0">
              <TrackInfo>
                <Title>Brilliant World(Vocal Main Mix)</Title>
                <Length>00:04:24.7370000</Length>
                <BPM>127</BPM>
                <FileOffset>00:00:00</FileOffset><!-- Offset is editable. -->
              </TrackInfo>
              <GameInfo>
                <Author>Rowe Wilson Frederisk Holme</Author>
              </GameInfo>
              <BeatmapList>
                <Map MapOffset="-00:00:00.4900000">
                  <Oribit>
                    <Frames>
                      <Key Time="00:00:00.9450000" Position="0" Color="Azure" /><!-- Real time is: 0.945 - 0.49 = 0.896-->
                      <Key Time="00:00:20.1230000" Position="3000" /><!-- Color="Azure", it's the same as above one. -->
                    </Frames>
                    <Particles>
                      <Blink Time="00:00:00" />
                      <Blink Time="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Slide Time="00:00:00" Direction="Left" />
                      <Slide Time="00:00:00" Direction="Right" />
                    </Particles>
                  </Oribit>
                  <Oribit>
                    <Frames>
                      <Key Time="00:00:01.8900000" Position="-700" Color="#FFFFFFFF" />
                    </Frames>
                    <Particles />
                  </Oribit>
                  <Oribit>
                    <Frames>
                      <Key Time="00:00:02.8350000" Position="700" Color="Azure" />
                    </Frames>
                    <Particles />
                  </Oribit>
                </Map>
              </BeatmapList>
            </ZeroVMap>
            """;
    }

    [Test]
    public void TestXml() {
        using MemoryStream stream = GetMemoryStream(this.XmlSource!);
        var serializer = new XmlSerializer(typeof(ZeroVMapXml));

        ZeroVMapXml? map = (ZeroVMapXml?)serializer.Deserialize(stream)!;
        // Assert: The map can safely be converted to string.
        map.ToString();
        Assert.AreEqual("Left",
            map.BeatmapList!.MapList![0].OribitList![0].Particles!.Slide![0].Direction.ToString());

        // deserialize to string
        using MemoryStream stream2 = new MemoryStream();
        serializer.Serialize(stream2, map);
        stream2.Position = 0;
        using StreamReader reader = new StreamReader(stream2);
        String text = reader.ReadToEnd();
    }
}
