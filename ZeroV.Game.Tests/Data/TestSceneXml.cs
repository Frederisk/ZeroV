using System;
using System.IO;
using System.Text;

using NUnit.Framework;

using osu.Framework.IO.Stores;
using ZeroV.Resources;
using System.Xml;
using System.Xml.Schema;
using ZeroV.Game.Data;

namespace ZeroV.Game.Tests.Data;

[TestFixture]
internal class TestSceneXml {
    private String? xmlSource;

    private static MemoryStream getMemoryStream(String source) {
        return new MemoryStream(Encoding.UTF8.GetBytes(source));
    }

    [SetUp]
    public void SetUp() {
        this.xmlSource = """
            <?xml version="1.0" encoding="utf-8"?>
            <ZeroVMap
              xmlns="http://zerov.games/ZeroVMap"
              xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
              MapVersion="1.0.0.0">
              <TrackInfo>
                <Title>Brilliant World(Vocal Main Mix)</Title>
                <Length>00:04:24.7370000</Length>
                <BPM>127</BPM>
                <FileOffset>00:00:00</FileOffset><!-- Offset is editable. -->
              </TrackInfo>
              <GameInfo>
                <Author>Rowe Wilson Frederisk Holme</Author>
                <GameVersion>0.0.0.0</GameVersion>
              </GameInfo>
              <BeatmapList>
                <Map MapOffset="-00:00:00.4900000">
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:00.9450000" Position="0" Width="1000" Colour="#abcdef" /><!-- Real time is:
                      0.945 - 0.49 = 0.896-->
                      <Key Time="00:00:20.1230000" Position="3000" Width="13" Colour="#123345"/><!-- Colour="Azure", it's the same as above
                      one. -->
                      <Key Time="00:00:00.9450000" Position="0" Width="1000" Colour="#FFFF9999"></Key>
                    </Frames>
                    <Particles>
                      <Blink Time="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Blink Time="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Slide Time="00:00:00" Direction="Left" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Slide Time="00:00:00" Direction="Right" />
                      <Blink Time="00:00:00" />
                      <Blink Time="00:00:00" />
                      <Blink Time="00:00:00" />
                    </Particles>
                  </Orbit>
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:01.8900000" Position="-700" Width="1000" Colour="#FFFFFFFF" />
                    </Frames>
                    <Particles />
                  </Orbit>
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:02.8350000" Position="700" Width="1000" Colour="#FFFFFF" />
                    </Frames>
                    <Particles />
                  </Orbit>
                </Map>
                <Map MapOffset="-00:00:00.4900000">
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:00.9450000" Position="0" Width="1000" Colour="#abcdef" /><!-- Real time is:
                      0.945 - 0.49 = 0.896-->
                      <Key Time="00:00:20.1230000" Position="3000" Width="13" Colour="#123345"/><!-- Colour="Azure", it's the same as above
                      one. -->
                      <Key Time="00:00:00.9450000" Position="0" Width="1000" Colour="#FFFF9999"></Key>
                    </Frames>
                    <Particles>
                      <Blink Time="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Blink Time="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Slide Time="00:00:00" Direction="Left" />
                      <Press StartTime="00:00:00" EndTime="00:00:00" />
                      <Slide Time="00:00:00" Direction="Right" />
                      <Blink Time="00:00:00" />
                      <Blink Time="00:00:00" />
                      <Blink Time="00:00:00" />
                    </Particles>
                  </Orbit>
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:01.8900000" Position="-700" Width="1000" Colour="#FFFFFFFF" />
                    </Frames>
                    <Particles />
                  </Orbit>
                  <Orbit>
                    <Frames>
                      <Key Time="00:00:02.8350000" Position="700" Width="1000" Colour="#FFFFFF" />
                    </Frames>
                    <Particles />
                  </Orbit>
                </Map>
              </BeatmapList>
            </ZeroVMap>
            """;
    }

    [Test]
    public void TestXml() {
        BeatmapWrapper wrapper = new(getMemoryStream(this.xmlSource!));
        Assert.IsNotNull(wrapper.ZeroVMap);
    }
}
