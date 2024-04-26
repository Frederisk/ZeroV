using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

using ZeroV.Game.Objects;
using ZeroV.Game.Data;
using osu.Framework.IO.Stores;
using ZeroV.Resources;
using osu.Framework.Audio.Track;
using System.Xml;
using System.Xml.Schema;

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
              </BeatmapList>
            </ZeroVMap>
            """;
    }

    [Test]
    public void TestXml() {
        // xsd
        using DllResourceStore dllStore = new(ZeroVResources.ResourceAssembly);
        using Stream xsdStream = dllStore.GetStream(@"Data/ZeroVMap.xsd");
        using XmlReader xsdReader = XmlReader.Create(xsdStream);
        // xml
        using MemoryStream xmlStream = GetMemoryStream(this.XmlSource!);
        //using XmlReader xmlReader = XmlReader.Create(xmlStream);

        // setting
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add("http://zerov.games/ZeroVMap", xsdReader);
        settings.ValidationEventHandler += new ValidationEventHandler((sender, args) => { if (args.Severity is XmlSeverityType.Error) { throw new Exception(); } });
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationType = ValidationType.Schema;

        // validate
        using XmlReader reader = XmlReader.Create(xmlStream, settings);
        XmlDocument document = new XmlDocument();
        document.Load(reader);

        //Assert.IsNotNull(xsdStream);
        //using MemoryStream stream = GetMemoryStream(this.XmlSource!);
        //var serializer = new XmlSerializer(typeof(ZeroVMapXml));
        //ZeroVMapXml? map = (ZeroVMapXml?)serializer.Deserialize(stream)!;
        //var result = AnnotationsValidator.TryValidateObjectRecursive(map, null, null);

        //Assert.IsFalse(result);

        //// Assert: The map can safely be converted to string.
        //map.ToString();
        //Assert.AreEqual("Left",
        //    map.BeatmapList!.MapList![0].OrbitList![0].Particles!.Slide![0].Direction.ToString());

        //// deserialize to string
        //using MemoryStream stream2 = new MemoryStream();
        //serializer.Serialize(stream2, map);
        //stream2.Position = 0;
        //using StreamReader reader = new StreamReader(stream2);
        //String text = reader.ReadToEnd();
    }

    [Test]
    public void TestXml2() {
        // xsd
        using XmlReader xsdReader = XmlReader.Create("./Schemas/ZeroVMap.xsd");
        // xml
        using MemoryStream stream = GetMemoryStream(this.XmlSource!);

        // setting
        var settings = new XmlReaderSettings();
        settings.Schemas.Add("http://zerov.games/ZeroVMap", xsdReader);
        settings.ValidationEventHandler += new ValidationEventHandler((sender, args) => { if (args.Severity is XmlSeverityType.Error) { throw new Exception(); } });
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationType = ValidationType.Schema;

        // deserialize
        var serializer = new XmlSerializer(typeof(Schemas.ZeroVMap.ZeroVMap));
        Schemas.ZeroVMap.ZeroVMap? map = (Schemas.ZeroVMap.ZeroVMap?)serializer.Deserialize(stream)!;
    }
}
