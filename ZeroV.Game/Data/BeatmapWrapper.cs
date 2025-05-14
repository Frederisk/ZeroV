using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq; // Just to use the Sum() method.
using System.Net.Security;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using osu.Framework.Graphics;

using ZeroV.Game.Data.Schema.ZeroVMap;
using ZeroV.Game.Elements.Orbits;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Data;

/// <summary>
/// A wrapper for XML beatmap files to make it easier to read and use the data.
/// </summary>
public class BeatmapWrapper {

    // FIXME: Determine if there is a better solution.
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode", Justification = "the constructor will keep all necessary members of the target type.")]
    private class MyXmlSerializer : XmlSerializer {

        public MyXmlSerializer([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] Type type) : base(type) {
        }

        public new Object? Deserialize(XmlReader reader) => base.Deserialize(reader);
    }

    private static readonly MyXmlSerializer zero_v_map_serializer = new(typeof(ZeroVMapXml));
    private static readonly XmlReaderSettings xml_reader_settings = createXmlReaderSettings();

    #region createXmlReaderSettings

    private static XmlReaderSettings createXmlReaderSettings() {
        XmlReaderSettings settings = new() {
            ValidationType = ValidationType.Schema,
            ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints
                            | XmlSchemaValidationFlags.AllowXmlAttributes
                            | XmlSchemaValidationFlags.ReportValidationWarnings,
        };
        settings.ValidationEventHandler += validationEventHandler;
        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream("ZeroV.Game.Data.Schema.ZeroVMap.ZeroVMapXml.xsd") ?? throw new InvalidOperationException("The schema file is missing.");
        using XmlReader xmlReader = XmlReader.Create(stream);
        settings.Schemas.Add("http://schemas.zerov.net/2024/ZeroVMap", xmlReader);
        //settings.Schemas.Add("http://schemas.zerov.net/2024/ZeroVMap", "./Data/Schema/ZeroVMap/ZeroVMapXml.xsd");
        // XmlReader xmlReader = XmlReader.Create(new StreamReader("./Data/Schema/ZeroVMap/ZeroVMapXml.xsd"));
        // settings.Schemas.Add("http://schemas.zerov.net/2024/ZeroVMap", xmlReader);

        return settings;
    }

    private static void validationEventHandler(Object? sender, ValidationEventArgs args) {
        if (args.Severity is XmlSeverityType.Error) {
            throw new InvalidOperationException(args.Message);
        }
    }

    #endregion createXmlReaderSettings

    /// <summary>
    /// The serialized ZeroVMap object from the XML file.
    /// </summary>
    public ZeroVMapXml ZeroVMap { get; }

    /// <summary>
    /// The file that the beatmap was read from.
    /// </summary>
    public FileInfo BeatmapFile { get; }

    private BeatmapWrapper(ZeroVMapXml zeroVMap, FileInfo file) {
        this.ZeroVMap = zeroVMap;
        this.BeatmapFile = file;
    }

    /// <summary>
    /// Create a new instance of <see cref="BeatmapWrapper"/> from a file.
    /// </summary>
    /// <param name="file">The file to read the beatmap from.</param>
    /// <returns>The new instance of <see cref="BeatmapWrapper"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is null.</exception>
    /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the <see cref="Exception.InnerException"/> property.</exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="IOException"></exception>
    public static BeatmapWrapper Create(FileInfo file) {
        //ArgumentNullException.ThrowIfNull(file);
        using FileStream xmlStream = file.OpenRead();
        ZeroVMapXml map = DeserializeZeroVMapXml(xmlStream);
        return new BeatmapWrapper(map, file);
    }

    public static ZeroVMapXml DeserializeZeroVMapXml(Stream stream) {
        using XmlReader reader = XmlReader.Create(stream, xml_reader_settings);
        // Deserialize the XML
        ZeroVMapXml map = (ZeroVMapXml)zero_v_map_serializer.Deserialize(reader)!;
        // No longer used as xsd can now be used to verify validity.
        // var result = AnnotationsValidator.TryValidateObjectRecursive(map, null, null);
        // if (!result) {
        //     throw new InvalidOperationException("The XML file is invalid.");
        // }
        return map;
    }

    /// <summary>
    /// Get a list of all <see cref="Beatmap"/>s in the file.
    /// </summary>
    /// <returns>
    /// A list of all <see cref="Beatmap"/>s in the file.
    /// </returns>
    public List<Beatmap> GetAllBeatmaps() =>
        this.ZeroVMap.BeatmapList.ConvertAll(getBeatmapFromXml);

    /// <summary>
    /// Get the <see cref="Beatmap"/> at the specified index.
    /// </summary>
    /// <param name="index">The index of the <see cref="Beatmap"/> to get.</param>
    /// <returns>The <see cref="Beatmap"/> at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is not found in the <see cref="ZeroVMapXml.BeatmapList"/>.</exception>
    public Beatmap GetBeatmapByIndex(Int32 index) {
        foreach (MapXml map in this.ZeroVMap.BeatmapList) {
            if (Int32.TryParse(map.Index, out Int32 mapIndex) && mapIndex == index) {
                return getBeatmapFromXml(map);
            }
        }
        throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
    }

    /// <summary>
    /// Get the <see cref="TrackInfo"/> from the file.
    /// </summary>
    /// <returns>
    /// The <see cref="TrackInfo"/> from the file.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when the track file and it's folder layout is invalid.</exception>
    public TrackInfo GetTrackInfo() {
        DirectoryInfo? directory = this.BeatmapFile.Directory ?? throw new InvalidOperationException("The beatmap file is not in a valid directory.");
        // TODO: Need a better way to match music file
        FileInfo[] backgrounds = directory.GetFiles(ZeroVPath.TRACK_FILE_BACKGROUND_IMAGE_PATTERN);
        FileInfo? backgroud = backgrounds.Length >= 1 && backgrounds[0].Exists ? backgrounds[0] : null;
        FileInfo[] trackFiles = directory.GetFiles(ZeroVPath.TRACK_FILE_NAME_PATTERN);
        if (trackFiles.Length < 1 || !trackFiles[0].Exists) {
            throw new InvalidOperationException("Track file not found.");
        }
        //if (backgrounds.Length < 1 || !backgrounds[0].Exists) {
        //    throw new InvalidOperationException("Background file not found.");
        //}
        return new() {
            UUID = new Guid(this.ZeroVMap.UUID),
            Title = this.ZeroVMap.TrackInfo.Title,
            Album = this.ZeroVMap.TrackInfo.Album,
            TrackOrder = this.ZeroVMap.TrackInfo.TrackOrder is not null ? Int32.Parse(this.ZeroVMap.TrackInfo.TrackOrder) : null,
            Artists = this.ZeroVMap.TrackInfo.Artists,
            FileOffset = this.ZeroVMap.TrackInfo.FileOffset is not null ? TimeSpan.Parse(this.ZeroVMap.TrackInfo.FileOffset) : default,
            GameAuthor = this.ZeroVMap.GameInfo.Author,
            Description = this.ZeroVMap.GameInfo.Description,
            GameVersion = new Version(this.ZeroVMap.GameInfo.GameVersion),
            MapInfos = this.ZeroVMap.BeatmapList.ConvertAll(getMapInfoFromXml),
            TrackFile = trackFiles[0],
            BeatmapFile = this.BeatmapFile,
            BackgroundFile = backgroud,
        };
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
        };

    private static MapInfo getMapInfoFromXml(MapXml mapXml) => new() {
        //MapOffset = mapXml.MapOffset is not null ? TimeSpan.Parse(mapXml.MapOffset) : default,
        Index = Int32.Parse(mapXml.Index),
        Difficulty = default, // FIXME: calculate it
        BlinkCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Blink.Count).Sum(),
        PressCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Press.Count).Sum(),
        SlideCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Slide.Count).Sum(),
        StrokeCount = mapXml.Orbit.ConvertAll(orbit => orbit.Particles.Stroke.Count).Sum(),
    };

    #endregion Wrapping static methods
}
