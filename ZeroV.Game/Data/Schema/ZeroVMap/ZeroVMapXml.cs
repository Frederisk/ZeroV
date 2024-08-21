using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ZeroV.Game.Data.Schema.ZeroVMap;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(DirectionXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap")]
public enum DirectionXml {
    Up,
    Right,
    Down,
    Left,
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(ZeroVMapXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
[XmlRoot("ZeroVMap", Namespace = "http://schemas.zerov.net/2024/ZeroVMap")]
public partial class ZeroVMapXml {

    [Required]
    [XmlElement("TrackInfo")]
    public TrackInfoXml TrackInfo { get; set; }

    [Required]
    [XmlElement("GameInfo")]
    public GameInfoXml GameInfo { get; set; }

    [Required]
    [XmlArray("BeatmapList")]
    [XmlArrayItem("Map", Namespace = "http://schemas.zerov.net/2024/ZeroVMap")]
    public List<MapXml> BeatmapList { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: \d+\.\d+\.\d+\.\d+.</para>
    /// </summary>
    [RegularExpression(@"\d+\.\d+\.\d+\.\d+")]
    [Required]
    [XmlAttribute("MapVersion")]
    public String MapVersion { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: [0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}</para>
    /// </summary>
    [RegularExpression(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}")]
    [Required]
    [XmlAttribute("UUID")]
    public String UUID { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(TrackInfoXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class TrackInfoXml {

    [Required]
    [XmlElement("Title")]
    public String Title { get; set; }

    [AllowNull]
    [MaybeNull]
    [XmlElement("Album")]
    public String? Album { get; set; }

    [AllowNull]
    [MaybeNull]
    [XmlElement("TrackOrder")]
    public String? TrackOrder { get; set; }

    [AllowNull]
    [MaybeNull]
    [XmlElement("Artists")]
    public String? Artists { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlElement("Length")]
    public String Length { get; set; }

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    [XmlElementAttribute("BPM")]
    public Single BpmValue { get; set; }

    /// <summary>
    /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Bpm-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
    /// <para xml:lang="en">Gets or sets a value indicating whether the Bpm property is specified.</para>
    /// </summary>
    [XmlIgnoreAttribute()]
    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public bool BpmValueSpecified { get; set; }

    [XmlIgnoreAttribute()]
    public Nullable<Single> Bpm {
        get => this.BpmValueSpecified ? this.BpmValue : null;
        set {
            this.BpmValue = value.GetValueOrDefault();
            this.BpmValueSpecified = value.HasValue;
        }
    }

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlElement("FileOffset")]
    public String FileOffset { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(GameInfoXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class GameInfoXml {

    [Required]
    [XmlElement("Author")]
    public String Author { get; set; }

    [AllowNull]
    [MaybeNull]
    [XmlElement("Description")]
    public String? Description { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: \d+\.\d+\.\d+\.\d+.</para>
    /// </summary>
    [RegularExpression(@"\d+\.\d+\.\d+\.\d+")]
    [Required]
    [XmlElement("GameVersion")]
    public String GameVersion { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(BeatmapListXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class BeatmapListXml {

    [XmlElement("Map")]
    public List<MapXml> Map { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(MapXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class MapXml {

    [XmlElement("Orbit")]
    public List<OrbitXml> Orbit { get; set; }

    ///// <summary>
    ///// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    ///// </summary>
    //[RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    //[AllowNull]
    //[MaybeNull]
    //[XmlAttribute("MapOffset")]
    //public String? MapOffset { get; set; }

    [Required]
    [XmlAttribute("Index")]
    public String Index { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(OrbitXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class OrbitXml {

    [Required]
    [XmlElement("Frames")]
    public FramesXml Frames { get; set; }

    [Required]
    [XmlElement("Particles")]
    public ParticlesXml Particles { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(FramesXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class FramesXml {

    [Required]
    [XmlElement("Key")]
    public List<KeyXml> Key { get; set; }

    //[XmlElement("Fuck")]
    //public List<Object> Fuck { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(KeyXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class KeyXml {

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("Time")]
    public String Time { get; set; }

    [Required]
    [XmlAttribute("Position")]
    public Single Position { get; set; }

    [Required]
    [XmlAttribute("Width")]
    public Single Width { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: #([0-Fa-f]{6}|[0-Fa-f]{8}).</para>
    /// </summary>
    [RegularExpression("#([0-Fa-f]{6}|[0-Fa-f]{8})")]
    [Required]
    [XmlAttribute("Colour")]
    public String Colour { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(ParticlesXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class ParticlesXml {

    [XmlElement("Blink")]
    public List<BlinkXml> Blink { get; set; }

    [XmlElement("Press")]
    public List<PressXml> Press { get; set; }

    [XmlElement("Slide")]
    public List<SlideXml> Slide { get; set; }

    [XmlElement("Stroke")]
    public List<StrokeXml> Stroke { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(BlinkXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class BlinkXml {

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("Time")]
    public String Time { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(PressXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class PressXml {

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("StartTime")]
    public String StartTime { get; set; }

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("EndTime")]
    public String EndTime { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(SlideXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class SlideXml {

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("Time")]
    public String Time { get; set; }

    [Required]
    [XmlAttribute("Direction")]
    public DirectionXml Direction { get; set; }
}

[GeneratedCode("XmlSchemaClassGenerator-Modified", "2.1.1094.0")]
[Serializable]
[XmlType(nameof(StrokeXml), Namespace = "http://schemas.zerov.net/2024/ZeroVMap", AnonymousType = true)]
[DesignerCategory("code")]
public partial class StrokeXml {

    /// <summary>
    /// <para xml:lang="en">Pattern: -?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?.</para>
    /// </summary>
    [RegularExpression(@"-?([01]\d|2[0-3]):\d{2}:\d{2}(\.\d{1,7})?")]
    [Required]
    [XmlAttribute("Time")]
    public String Time { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
