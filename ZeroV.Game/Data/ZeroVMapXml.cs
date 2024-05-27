using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

using osu.Framework.Extensions;
using osu.Framework.Graphics;

using ZeroV.Game.Elements.Particles;

using static ZeroV.Game.Elements.OrbitSource;
using System.Diagnostics;

namespace ZeroV.Game.Objects;

[XmlRoot(ElementName = "ZeroVMap")]
public record class ZeroVMapXml {
    [XmlAttribute(AttributeName = "MapVersion")]
    public String? MapVersionString {
        get => this.MapVersion?.ToString();
        set => this.MapVersion = value is null ? null : Version.Parse(value);
    }

    [XmlIgnore]
    public Version? MapVersion { get; set; }

    [XmlElement(ElementName = "TrackInfo")]
    public TrackInfoXml? TrackInfo { get; set; }

    [XmlElement(ElementName = "GameInfo")]
    public GameInfoXml? GameInfo { get; set; }

    [XmlElement(ElementName = "BeatmapList")]
    public BeatmapListXml? BeatmapList { get; set; }
}

public record class TrackInfoXml {
    [XmlElement(ElementName = "Title")]
    //[Required]
    public String Title { get; set; } = null!;

    [XmlElement(ElementName = "Artist")]
    public ArtistsXml? Artists { get; set; }

    [XmlElement(ElementName = "Length")]
    public String? LengthString {
        get => this.Length?.ToString();
        set => this.Length = value is null ? null : TimeSpan.Parse(value);
    }

    [XmlIgnore]
    public TimeSpan? Length { get; set; }

    [XmlElement(ElementName = "BPM")]
    public Double BPM { get; set; }

    [XmlElement(ElementName = "FileOffset")]
    public String? FileOffsetString {
        get => this.FileOffset?.ToString();
        set => this.FileOffset = value is null ? null : TimeSpan.Parse(value);
    }

    [XmlIgnore]
    public TimeSpan? FileOffset { get; set; }
}

public record class ArtistsXml {
}

public record class GameInfoXml {
    [XmlElement(ElementName = "Author")]
    public String? Author { get; set; }

    [XmlElement(ElementName = "Description")]
    public String? Description { get; set; }

    [XmlElement(ElementName = "GameVersion")]
    public String? GameVersionString {
        get => this.GameVersion?.ToString();
        set => this.GameVersion = value is null ? null : Version.Parse(value);
    }

    [XmlIgnore]
    public Version? GameVersion { get; set; }
}

public record class BeatmapListXml {
    [XmlElement(ElementName = "Map")]
    public List<MapXml>? MapList { get; set; }
}

public record class MapXml {
    [XmlAttribute(AttributeName = "MapOffset")]
    public String? MapOffsetString {
        get => this.MapOffset?.ToString();
        set => this.MapOffset = value is null ? null : TimeSpan.Parse(value);
    }

    [XmlIgnore]
    public TimeSpan? MapOffset { get; set; }

    [XmlElement(ElementName = "Orbit")]
    public List<OrbitXml>? OrbitList { get; set; }
}

public record class OrbitXml {
    [XmlElement(ElementName = "Frames")]
    public FramesXml? Frames { get; set; }

    [XmlElement(ElementName = "Particles")]
    public ParticlesXml? Particles { get; set; }
}

public record class FramesXml {
    [XmlElement(ElementName = "Key")]
    public List<KeyXml>? Key { get; set; }

    public List<KeyFrame> GetKeyFrames() {
        //if (this.Key is null || this.Key.Count is 0) {
        //    throw new NullReferenceException("Key is null or empty.");
        //}

        return this.Key!.ConvertAll(key => {
            //if (key.Time is null || key.Colour is null) {
            //    throw new NullReferenceException("Key time or colour is null.");
            //}

            return new KeyFrame {
                Colour = Colour4.FromHex(key.Colour),
                Time = key.Time!.Value.TotalMilliseconds,
                Width = key.Width,
                XPosition = key.Position
            };
        });
    }
}

public record class KeyXml {
    [XmlAttribute(AttributeName = "Time")]
    public String? TimeString {
        get => this.Time?.ToString();
        set => this.Time = value is null ? null : TimeSpan.Parse(value);
    }

    [XmlIgnore]
    public TimeSpan? Time { get; set; }

    [XmlAttribute(AttributeName = "Position")]
    public Single Position { get; set; }

    [XmlAttribute(AttributeName = "Width")]
    public Single Width { get; set; }

    [XmlAttribute(AttributeName = "Colour")]
    public String? Colour { get; set; }
}

public record class ParticlesXml {
    [XmlElement(ElementName = "Blink")]
    public List<BlinkXml>? Blink { get; set; }

    [XmlElement(ElementName = "Press")]
    public List<PressXml>? Press { get; set; }

    [XmlElement(ElementName = "Slide")]
    public List<SlideXml>? Slide { get; set; }

    [XmlElement(ElementName = "Stroke")]
    public List<StrokeXml>? Stroke { get; set; }

    public List<ParticleSource> GetParticleSources() {
        List<ParticleSource> particles = [];
        particles.AddRange(this.Blink?.ConvertAll(item =>
            new BlinkParticleSource(item.Time!.Value.TotalMilliseconds)) ?? []);
        particles.AddRange(this.Press?.ConvertAll(item =>
            new PressParticleSource(item.StartTime!.Value.TotalMilliseconds,
                                    item.EndTime!.Value.TotalMilliseconds)) ?? []);
        particles.AddRange(this.Slide?.ConvertAll(item =>
            new SlideParticleSource(item.Time!.Value.TotalMilliseconds, item.Direction)) ?? []);
        particles.AddRange(this.Stroke?.ConvertAll(item =>
            new StrokeParticleSource(item.Time!.Value.TotalMilliseconds)) ?? []);
        particles.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

        return particles;
    }

    public record class BlinkXml {
        [XmlAttribute(AttributeName = "Time")]
        public String? TimeString {
            get => this.Time?.ToString();
            set => this.Time = value is null ? null : TimeSpan.Parse(value);
        }

        [XmlIgnore]
        public TimeSpan? Time { get; set; }
    }

    public record class PressXml {
        [XmlAttribute(AttributeName = "StartTime")]
        public String? StartTimeString {
            get => this.StartTime?.ToString();
            set => this.StartTime = value is null ? null : TimeSpan.Parse(value);
        }

        [XmlIgnore]
        public TimeSpan? StartTime { get; set; }

        [XmlAttribute(AttributeName = "EndTime")]
        public String? EndTimeString {
            get => this.EndTime?.ToString();
            set => this.EndTime = value is null ? null : TimeSpan.Parse(value);
        }

        [XmlIgnore]
        public TimeSpan? EndTime { get; set; }
    }

    public record class SlideXml {
        [XmlAttribute(AttributeName = "Time")]
        public String? TimeString {
            get => this.Time?.ToString();
            set => this.Time = value is null ? null : TimeSpan.Parse(value);
        }

        [XmlIgnore]
        public TimeSpan? Time { get; set; }

        [XmlAttribute(AttributeName = "Direction")]
        public SlidingDirection Direction { get; set; }
    }

    public record class StrokeXml {
        [XmlAttribute(AttributeName = "Time")]
        public String? TimeString {
            get => this.Time?.ToString();
            set => this.Time = value is null ? null : TimeSpan.Parse(value);
        }

        [XmlIgnore]
        public TimeSpan? Time { get; set; }
    }
}
