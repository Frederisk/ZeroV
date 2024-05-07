using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using osu.Framework.Graphics;

using ZeroV.Game.Data.Schema.ZeroVMap;
using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Data;

public class BeatmapWrapper {
    private static readonly XmlSerializer zero_v_map_serializer = new(typeof(ZeroVMapXml));
    private static readonly XmlReaderSettings xml_reader_settings = createXmlReaderSettings();

    #region createXmlReaderSettings

    [SuppressMessage("Style", "IDE0017")]
    private static XmlReaderSettings createXmlReaderSettings() {
        XmlReaderSettings settings = new();
        settings.ValidationType = ValidationType.Schema;
        settings.Schemas.Add("http://zerov.games/ZeroVMap", "./Schemas/ZeroVMap.xsd");
        settings.ValidationEventHandler += validationEventHandler;
        settings.ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints
                                 | XmlSchemaValidationFlags.AllowXmlAttributes
                                 | XmlSchemaValidationFlags.ReportValidationWarnings;

        return settings;
    }

    private static void validationEventHandler(Object? sender, ValidationEventArgs args) {
        if (args.Severity is XmlSeverityType.Error) {
            throw new InvalidOperationException(args.Message);
        }
    }

    #endregion createXmlReaderSettings

    public ZeroVMapXml ZeroVMap { get; }

    public BeatmapWrapper(Stream xmlStream) {
        ArgumentNullException.ThrowIfNull(xmlStream);

        using var reader = XmlReader.Create(xmlStream, xml_reader_settings);

        // Deserialize the XML
        this.ZeroVMap = (ZeroVMapXml)zero_v_map_serializer.Deserialize(reader)!;
    }

    public List<Beatmap> GetAllBeatmaps() {
        List<Beatmap> beatmaps = [];
        beatmaps.AddRange(this.ZeroVMap.BeatmapList.ConvertAll(getBeatmapFromXml));
        if (this.ZeroVMap.TrackInfo.FileOffset is not null) {
            beatmaps.ForEach(beatmap => beatmap.Offset += TimeSpan.Parse(this.ZeroVMap.TrackInfo.FileOffset).TotalMilliseconds);
        }
        return beatmaps;
    }

    public Beatmap GetBeatmapAt(Int32 index) {
        Beatmap beatmap = getBeatmapFromXml(this.ZeroVMap.BeatmapList[index]);
        if (this.ZeroVMap.TrackInfo.FileOffset is not null) {
            beatmap.Offset += TimeSpan.Parse(this.ZeroVMap.TrackInfo.FileOffset).TotalMilliseconds;
        }
        return beatmap;
    }

    #region Wrapping static methods

    private static OrbitSource.KeyFrame getKeyFrameFromXml(KeyXml keyXml) =>
        new() {
            Time = TimeSpan.Parse(keyXml.Time).TotalMilliseconds,
            XPosition = keyXml.Position,
            Width = keyXml.Width,
            Colour = Colour4.FromHex(keyXml.Colour),
        };

    private static List<OrbitSource.KeyFrame> getKeyFrameListFromXml(FramesXml framesXml) {
        List<OrbitSource.KeyFrame> keyFrame = framesXml.Key.ConvertAll(getKeyFrameFromXml);
        keyFrame.Sort((a, b) => a.Time.CompareTo(b.Time));
        return keyFrame;
    }

    private static BlinkParticleSource getBlinkParticleSourceFromXml(BlinkXml blinkXml) =>
        new(TimeSpan.Parse(blinkXml.Time).TotalMilliseconds);

    private static PressParticleSource getPressParticleSourceFromXml(PressXml pressXml) =>
        new(TimeSpan.Parse(pressXml.StartTime).TotalMilliseconds, TimeSpan.Parse(pressXml.EndTime).TotalMilliseconds);

    private static SlidingDirection getSlidingDirectionFromXml(DirectionXml xmlDirection) => xmlDirection switch {
        DirectionXml.Left => SlidingDirection.Left,
        DirectionXml.Right => SlidingDirection.Right,
        DirectionXml.Up => SlidingDirection.Up,
        DirectionXml.Down => SlidingDirection.Down,
        _ => throw new ArgumentOutOfRangeException(nameof(xmlDirection), xmlDirection, null),
    };

    private static SlideParticleSource getSlideParticleSourceFromXml(SlideXml slideXml) =>
        new(TimeSpan.Parse(slideXml.Time).TotalMilliseconds, getSlidingDirectionFromXml(slideXml.Direction));

    private static StrokeParticleSource getStrokeParticleSourceFromXml(StrokeXml strokeXml) =>
        new(TimeSpan.Parse(strokeXml.Time).TotalMilliseconds);

    private static List<ParticleSource> getParticleSourceListFromXml(ParticlesXml particlesXml) {
        List<ParticleSource> particleSources = [];
        particleSources.AddRange(particlesXml.Blink.ConvertAll(getBlinkParticleSourceFromXml));
        particleSources.AddRange(particlesXml.Press.ConvertAll(getPressParticleSourceFromXml));
        particleSources.AddRange(particlesXml.Slide.ConvertAll(getSlideParticleSourceFromXml));
        particleSources.AddRange(particlesXml.Stroke.ConvertAll(getStrokeParticleSourceFromXml));
        particleSources.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
        return particleSources;
    }

    private static OrbitSource getOrbitSourceFromXml(OrbitXml orbitXml) =>
        new() {
            KeyFrames = getKeyFrameListFromXml(orbitXml.Frames),
            HitObjects = getParticleSourceListFromXml(orbitXml.Particles),
        };

    private static Beatmap getBeatmapFromXml(MapXml mapXml) =>
        new() {
            OrbitSources = mapXml.Orbit.ConvertAll(getOrbitSourceFromXml),
            Offset = TimeSpan.Parse(mapXml.MapOffset ?? "00:00:00.0000000").TotalMilliseconds,
        };

    #endregion Wrapping static methods
}
