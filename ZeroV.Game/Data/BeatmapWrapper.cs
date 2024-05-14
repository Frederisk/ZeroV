using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq; // Just to use the Sum() method.
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
        settings.Schemas.Add("http://zerov.games/ZeroVMap", "./Data/Schema/ZeroVMap/ZeroVMapXml.xsd");
        // XmlReader xmlReader = XmlReader.Create(new StreamReader("./Data/Schema/ZeroVMap/ZeroVMapXml.xsd"));
        // settings.Schemas.Add("http://zerov.games/ZeroVMap", xmlReader);

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

    public FileInfo File { get; }

    private BeatmapWrapper(ZeroVMapXml zeroVMap, FileInfo file) {
        this.ZeroVMap = zeroVMap;
        this.File = file;
    }

    public static BeatmapWrapper Create(FileInfo file) {
        ArgumentNullException.ThrowIfNull(file);

        using FileStream xmlStream = file.OpenRead();
        using XmlReader reader = XmlReader.Create(xmlStream, xml_reader_settings);
        // Deserialize the XML
        var map = (ZeroVMapXml)zero_v_map_serializer.Deserialize(reader)!;
        return new BeatmapWrapper(map, file);
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

    public TrackInfo GetTrackInfo() {
        DirectoryInfo[]? directories = this.File.Directory?.GetDirectories();
        if (directories is null || directories.Length < 1 || !directories[0].Exists) {
            throw new InvalidOperationException("The beatmap file is not in a valid directory.");
        }
        // TODO: match music file
        FileInfo[] files = directories[0].GetFiles("Track.*");
        if (files.Length < 1 || !files[0].Exists) {
            throw new InvalidOperationException("Track file not found.");
        }
        return new() {
            Title = this.ZeroVMap.TrackInfo.Title,
            Album = this.ZeroVMap.TrackInfo.Album,
            TrackOrder = this.ZeroVMap.TrackInfo.TrackOrder is not null ? Int32.Parse(this.ZeroVMap.TrackInfo.TrackOrder) : null,
            Artists = this.ZeroVMap.TrackInfo.Artists,
            FileOffset = this.ZeroVMap.TrackInfo.FileOffset is not null ? TimeSpan.Parse(this.ZeroVMap.TrackInfo.FileOffset) : default,
            GameAuthor = this.ZeroVMap.GameInfo.Author,
            Description = this.ZeroVMap.GameInfo.Description,
            GameVersion = new Version(this.ZeroVMap.GameInfo.GameVersion),
            Maps = this.ZeroVMap.BeatmapList.ConvertAll(getMapInfoFromXml),
            File = files[0],
        };
    }

    #region Wrapping static methods

    private static MapInfo getMapInfoFromXml(MapXml mapXml) => new MapInfo() {
        MapOffset = mapXml.MapOffset is not null ? TimeSpan.Parse(mapXml.MapOffset) : default,
        Difficulty = default, // FIXME: calculate it
        BlinkCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Blink.Count).Sum(),
        PressCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Press.Count).Sum(),
        SlideCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Slide.Count).Sum(),
        StrokeCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Stroke.Count).Sum(),
    };

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
            Offset = mapXml.MapOffset is not null ? TimeSpan.Parse(mapXml.MapOffset).TotalMilliseconds : default,
        };

    #endregion Wrapping static methods
}
