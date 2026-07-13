using static DeviceHub.YhloTestTcpServer.Protocol.Hl7MessageEntity;

namespace DeviceHub.YhloTestTcpServer.Protocol;

/// <summary>
/// HL7 应用层消息段解析（MSH/PID/OBR/OBX/QRD/QRF）
/// </summary>
public static class Hl7MessageDecode
{
    public static ParseResult Parse(List<string> segmentList)
    {
        var result = new ParseResult();

        if (segmentList is null || segmentList.Count == 0)
            return result;

        foreach (string segment in segmentList)
        {
            if (string.IsNullOrWhiteSpace(segment))
                continue;

            ParseSegment(segment, result);
        }

        return result;
    }

    private static void ParseSegment(string segment, ParseResult result)
    {
        if (segment.Length < 3 || segment[3] != '|')
            return;

        string[] fields = segment.Split('|');
        if (fields.Length == 0)
            return;

        switch (fields[0])
        {
            case "MSH":
                result.MshSegment = ParseMshSegment(fields);
                break;
            case "PID":
                result.PidSegment = ParsePidSegment(fields);
                break;
            case "OBR":
                result.ObrSegmentList.Add(ParseObrSegment(fields));
                break;
            case "OBX":
                result.ObxSegmentList.Add(ParseObxSegment(fields));
                break;
            case "QRD":
                result.QrdSegment = ParseQrdSegment(fields);
                break;
            case "QRF":
                result.QrfSegment = ParseQrfSegment(fields);
                break;
        }
    }

    private static MshSegment ParseMshSegment(string[] fields) => new()
    {
        SendingApplication = GetMshField(fields, 3),
        SendingFacility = GetMshField(fields, 4),
        DateTimeOfMessage = GetMshField(fields, 7),
        MessageType = GetMshField(fields, 9),
        MessageControlId = GetMshField(fields, 10),
        ProcessingId = GetMshField(fields, 11),
        VersionId = GetMshField(fields, 12),
        ApplicationAcknowledgmentType = GetMshField(fields, 16),
        CharacterSet = GetMshField(fields, 18)
    };

    private static PidSegment ParsePidSegment(string[] fields) => new()
    {
        PatientId = GetField(fields, 2),
        PatientIdentifierList = GetField(fields, 3),
        PatientName = GetField(fields, 5),
        Sex = GetField(fields, 8)
    };

    private static ObrSegment ParseObrSegment(string[] fields) => new()
    {
        SetId = GetField(fields, 1),
        PlacerOrderNumber = GetField(fields, 2),
        FillerOrderNumber = GetField(fields, 3),
        UniversalServiceId = GetField(fields, 4),
        RequestedDateTime = GetField(fields, 6),
        ObservationDateTime = GetField(fields, 7),
        SpecimenSource = GetField(fields, 15)
    };

    private static ObxSegment ParseObxSegment(string[] fields) => new()
    {
        SetId = GetField(fields, 1),
        ValueType = GetField(fields, 2),
        ObservationIdentifier = GetField(fields, 3),
        ObservationSubId = GetField(fields, 4),
        ObservationValue = GetField(fields, 5),
        Units = GetField(fields, 6),
        AbnormalFlags = GetField(fields, 8),
        Probability = GetField(fields, 9),
        ResultStatus = GetField(fields, 11),
        ObservationDateTime = GetField(fields, 14)
    };

    private static QrdSegment ParseQrdSegment(string[] fields) => new()
    {
        QueryDateTime = GetField(fields, 1),
        SubjectFilter = GetField(fields, 8)
    };

    private static QrfSegment ParseQrfSegment(string[] fields) => new()
    {
        WhereSubjectFilter = GetField(fields, 1),
        WhenDataStartDateTime = GetField(fields, 2),
        WhenDataEndDateTime = GetField(fields, 3),
        SampleStartNo = GetField(fields, 4),
        SampleEndNo = GetField(fields, 5)
    };

    private static string GetMshField(string[] fields, int fieldNumber)
    {
        if (fieldNumber <= 0 || fieldNumber > fields.Length)
            return string.Empty;

        return fields[fieldNumber - 1];
    }

    private static string GetField(string[] fields, int fieldNumber)
    {
        if (fieldNumber <= 0 || fieldNumber >= fields.Length)
            return string.Empty;

        return fields[fieldNumber];
    }

    public sealed class ParseResult
    {
        public MshSegment MshSegment { get; set; } = new();
        public PidSegment? PidSegment { get; set; }
        public List<ObrSegment> ObrSegmentList { get; set; } = [];
        public List<ObxSegment> ObxSegmentList { get; set; } = [];
        public QrdSegment? QrdSegment { get; set; }
        public QrfSegment? QrfSegment { get; set; }

        public ObrSegment? FirstObrSegment => ObrSegmentList.Count > 0 ? ObrSegmentList[0] : null;

        public bool IsQcResult => MshSegment.ApplicationAcknowledgmentType == "2";
    }
}
