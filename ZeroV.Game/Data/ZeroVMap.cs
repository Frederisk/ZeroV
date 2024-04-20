using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Objects;

[XmlRoot(ElementName = "ZeroVMap")]
public class ZeroVMap {

    [XmlElement(ElementName = "TrackInfo")]
    public TrackInfo TrackInfo { get; set; }

    [XmlElement(ElementName = "GameInfo")]
    public GameInfo GameInfo { get; set; }

    [XmlElement(ElementName = "BeatMapList")]
    public BeatMapList BeatMapList { get; set; }
}

public class TrackInfo {
    [XmlElement(ElementName = "Title")]
    public String Title { get; set; }

    [XmlElement(ElementName = "Artist")]
    public String Artist { get; set; }

    [XmlElement(ElementName = "Length")]
    public TimeSpan Length { get; set; }

    [XmlElement(ElementName = "BPM")]
    public Double BPM { get; set; }

    [XmlElement(ElementName = "FileOffset")]
    public TimeSpan FileOffset { get; set; }
}

public class GameInfo {
    [XmlElement(ElementName = "Author")]
    public String Author { get; set; }
    [XmlElement(ElementName = "Author")]
    public String Description { get; set; }
    [XmlElement(ElementName = "Author")]
    public Version Version { get; set; }
}

public class BeatMapList {
    [XmlElement(ElementName = "Map")]
    public List<Map> MapList { get; set; }
}

public class Map {
    public List<Oribit> OribitList { get; set; }

    [XmlAttribute(AttributeName = "Offset")]
    public TimeSpan Offset { get; set; }
}

public class Oribit {
    [XmlElement(ElementName = "Frames")]
    public Frames Frames { get; set; }

    [XmlElement(ElementName = "Particles")]
    public Particles Particles { get; set; }
}

public class Frames {

    [XmlElement(ElementName = "Key")]
    public List<Key> Key { get; set; }
}

public class Key {
    [XmlAttribute(AttributeName = "Time")]
    public TimeSpan Time { get; set; }

    [XmlAttribute(AttributeName = "Position")]
    public Double Position { get; set; }

    [XmlAttribute(AttributeName = "Color")]
    public String Color { get; set; }
}

public class Particles {
    [XmlElement(ElementName = "Blink")]
    public List<Blink> Blink { get; set; }

    [XmlElement(ElementName = "Press")]
    public List<Press> Press { get; set; }

    [XmlElement(ElementName = "Slide")]
    public List<Slide> Slide { get; set; }

    [XmlElement(ElementName = "Stroke")]
    public List<Stroke> Stroke { get; set; }
}

public class Blink {
    [XmlAttribute(AttributeName = "Time")]
    public TimeSpan Time { get; set; }
}

public class Press {
    [XmlAttribute(AttributeName = "StartTime")]
    public TimeSpan StartTime { get; set; }

    [XmlAttribute(AttributeName = "EndTime")]
    public TimeSpan EndTime { get; set; }
}

public class Slide {
    [XmlAttribute(AttributeName = "Time")]
    public TimeSpan Time { get; set; }

    [XmlAttribute(AttributeName = "Direction")]
    public SlidingDirection Direction { get; set; }
}

public class Stroke {
    [XmlAttribute(AttributeName = "Time")]
    public TimeSpan Time { get; set; }
}
