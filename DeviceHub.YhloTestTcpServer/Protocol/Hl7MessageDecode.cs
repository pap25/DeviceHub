using DeviceHub.Template.Constant;
using DeviceHub.Utils;
using System.Text;
using static DeviceHub.YhloTestTcpServer.Protocol.Hl7MessageEntity;

namespace DeviceHub.YhloTestTcpServer.Protocol;

/// <summary>
/// HL7 应用层消息段解析（MSH/PID/OBR/OBX/QRD/QRF）
/// </summary>
public static class Hl7MessageDecode
{
    /// <summary>
    /// 仅解析报文中的 MSH 段（用于即时 ACK）。
    /// </summary>
    public static MshSegment? ParseMsh(byte[] rawMessage, Encoding encoding)
    {
        if (rawMessage is null || rawMessage.Length == 0)
            return null;

        int start = 0;
        int end = rawMessage.Length;

        if (rawMessage[start] == HL7Protocols.VT)
            start++;

        while (end > start && rawMessage[end - 1] is HL7Protocols.CR or HL7Protocols.EB)
            end--;

        if (end <= start)
            return null;

        string text = encoding.GetString(rawMessage, start, end - start);
        foreach (string line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            string segment = line.Trim();
            if (!segment.StartsWith("MSH|", StringComparison.Ordinal))
                continue;

            string[] fields = segment.Split('|');
            return fields.Length == 0 ? null : ParseMshSegment(fields);
        }

        return null;
    }

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
                // OBR 可重复；其后 OBX 挂到当前（最后一个）OBR 下
                result.ObrSegmentList.Add(ParseObrSegment(fields));
                break;
            case "OBX":
                if (result.ObrSegmentList.Count == 0)
                    break;
                result.ObrSegmentList[^1].ObxSegmentList.Add(ParseObxSegment(fields));
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
        ObservationEndDateTime = GetField(fields, 8),
        CollectionVolume = GetField(fields, 9),
        CollectorIdentifier = GetField(fields, 10),
        SpecimenActionCode = GetField(fields, 11),
        DangerCode = GetField(fields, 12),
        RelevantClinicalInfo = GetField(fields, 13),
        SpecimenReceivedDateTime = GetField(fields, 14),
        SpecimenSource = GetField(fields, 15),
        OrderingProvider = GetField(fields, 16),
        OrderCallbackPhoneNumber = GetField(fields, 17),
        PlacerField1 = GetField(fields, 18),
        PlacerField2 = GetField(fields, 19),
        FillerField1 = GetField(fields, 20),
        QcBarCode = GetField(fields, 21)
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

        /// <summary>检验申请/检验组列表（OBR 可重复，每个下挂各自的 OBX）</summary>
        public List<ObrSegment> ObrSegmentList { get; set; } = [];

        public QrdSegment? QrdSegment { get; set; }
        public QrfSegment? QrfSegment { get; set; }

        //public ObrSegment? FirstObrSegment => ObrSegmentList.Count > 0 ? ObrSegmentList[0] : null;

        /// <summary>所有 OBR 下的 OBX（扁平视图）</summary>
        public IEnumerable<ObxSegment> AllObxSegments => ObrSegmentList.SelectMany(obr => obr.ObxSegmentList);

        public bool IsQcResult => MshSegment.ApplicationAcknowledgmentType == "2";
    }
}
