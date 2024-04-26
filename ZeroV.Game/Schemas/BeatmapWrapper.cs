using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ZeroV.Game.Schemas;

public class BeatmapWrapper {
    private static readonly XmlSerializer zero_v_map_serializer = new(typeof(ZeroVMap));
    private static readonly XmlReaderSettings xml_reader_settings = createXmlReaderSettings();

    public ZeroVMap ZeroVMap { get; }

    public BeatmapWrapper(Stream xmlStream) {
        ArgumentNullException.ThrowIfNull(xmlStream);

        using var reader = XmlReader.Create(xmlStream, xml_reader_settings);

        // Deserialize the XML
        this.ZeroVMap = (ZeroVMap)zero_v_map_serializer.Deserialize(reader)!;
    }

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
}
