using static DeviceHub.YhloTest2SerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.YhloTest2SerialPort.Protocol;

/// <summary>
/// ASTM 应用层消息记录解析（H/P/O/R/C/Q/L）
/// </summary>
public static class AstmMessageDecode
{
    public static ParseResult Parse(List<string> frameDataList)
    {
        var result = new ParseResult();

        if (frameDataList is null || frameDataList.Count == 0)
            return result;

        foreach (string record in frameDataList)
        {
            if (string.IsNullOrWhiteSpace(record))
                continue;

            ParseRecord(record, result);
        }

        return result;
    }

    private static void ParseRecord(string record, ParseResult result)
    {
        if (record.Length < 2 || record[1] != '|')
            return;

        string[] fields = SplitFields(record);
        if (fields.Length == 0)
            return;

        switch (fields[0])
        {
            case "H":
                result.HeaderRecord = ParseHeaderRecord(fields);
                break;
            case "P":
                result.PatientRecord = ParsePatientRecord(fields);
                break;
            case "O":
                result.TestOrderRecord = ParseTestOrderRecord(fields);
                break;
            case "R":
                result.ResultRecordList.Add(ParseResultRecord(fields));
                break;
            case "C":
                result.ResultsCommentRecordList.Add(ParseCommentRecord(fields));
                break;
            case "Q":
                result.RequestInformationRecord = ParseRequestInformationRecord(fields);
                break;
            case "L":
                result.MessageTerminatorRecord = ParseTerminatorRecord(fields);
                break;
        }
    }

    private static HeaderRecord ParseHeaderRecord(string[] fields)
    {
        string sender = GetField(fields, 5);
        if (string.IsNullOrEmpty(sender))
            sender = GetField(fields, 6);

        return new HeaderRecord
        {
            RecordTypeId = GetField(fields, 1),
            DelimiterDefinition = GetField(fields, 2),
            MessageControlId = GetField(fields, 3),
            Password = GetField(fields, 4),
            SenderNameOrId = sender,
            InstrumentModel = GetComponent(sender, 0),
            SoftwareVersion = GetComponent(sender, 1),
            SerialNumber = GetComponent(sender, 2),
            SenderStreetAddress = GetField(fields, 6),
            ReservedField7 = GetField(fields, 7),
            SenderTelephoneNumber = GetField(fields, 8),
            CharacteristicsOfSender = GetField(fields, 9),
            ReceiverId = GetField(fields, 10),
            CharacterCodingSet = GetField(fields, 11),
            CharacterSet = GetField(fields, 12),
            ProcessingId = GetField(fields, 13),
            VersionNumber = GetField(fields, 14),
            DateAndTime = GetField(fields, 15)
        };
    }

    private static PatientRecord ParsePatientRecord(string[] fields) => new()
    {
        RecordTypeId = GetField(fields, 1),
        SequenceNumber = GetField(fields, 2),
        PracticeAssignedPatientId = GetField(fields, 3),
        PatientId = GetField(fields, 4),
        PatientId3 = GetField(fields, 5),
        PatientName = GetField(fields, 6),
        ReservedField7 = GetField(fields, 7),
        BirthDateOrAge = GetField(fields, 8),
        PatientSex = GetField(fields, 9),
        PatientRace = GetField(fields, 10),
        PatientAddress = GetField(fields, 11),
        ReservedField12 = GetField(fields, 12),
        PatientTelephone = GetField(fields, 13),
        AttendingPhysicianName = GetField(fields, 14),
        SpecialField1 = GetField(fields, 15),
        BodySurfaceArea = GetField(fields, 16),
        PatientHeight = GetField(fields, 17),
        PatientWeight = GetField(fields, 18),
        PatientDiagnosis = GetField(fields, 19),
        PatientMedications = GetField(fields, 20),
        PatientDiet = GetField(fields, 21),
        PracticeField1 = GetField(fields, 22),
        PracticeField2 = GetField(fields, 23),
        AdmissionStatus = GetField(fields, 25),
        Location = GetField(fields, 26)
    };

    private static TestOrderRecord ParseTestOrderRecord(string[] fields)
    {
        string sampleId = GetField(fields, 3);
        return new TestOrderRecord
        {
            RecordTypeId = GetField(fields, 1),
            SequenceNumber = GetField(fields, 2),
            SampleId = sampleId,
            SampleTrayNo = GetComponent(sampleId, 0),
            SamplePosNo = GetComponent(sampleId, 1),
            InstrumentSpecimenId = GetField(fields, 4),
            AssayNo = GetField(fields, 5),
            Priority = GetField(fields, 6),
            RequestedDateTime = GetField(fields, 7),
            SpecimenCollectionDateTime = GetField(fields, 8),
            CollectionEndTime = GetField(fields, 9),
            CollectionVolume = GetField(fields, 10),
            CollectedBy = GetField(fields, 11),
            ActionCode = GetField(fields, 12),
            DangerCode = GetField(fields, 13),
            RelevantClinicalInformation = GetField(fields, 14),
            SpecimenReceivedDateTime = GetField(fields, 15),
            SpecimenType = GetField(fields, 16),
            OrderingPhysician = GetField(fields, 17),
            PhysicianPhoneNumber = GetField(fields, 18),
            OfflineDilutionFactor = GetField(fields, 19),
            UserField2 = GetField(fields, 20),
            LaboratoryField1 = GetField(fields, 21),
            LaboratoryField2 = GetField(fields, 22),
            ResultsReportedDateTime = GetField(fields, 23),
            InstrumentChargeToComputer = GetField(fields, 24),
            InstrumentSectionId = GetField(fields, 25),
            ReportType = GetField(fields, 26),
            ReservedField27 = GetField(fields, 27),
            Location = GetField(fields, 28),
            NosocomialInfectionFlag = GetField(fields, 29),
            SpecimenService = GetField(fields, 30),
            SpecimenInstitution = GetField(fields, 31)
        };
    }

    private static ResultRecord ParseResultRecord(string[] fields)
    {
        string assay = GetField(fields, 3);
        string resultType = GetField(fields, 4);
        bool hasExplicitResultType = resultType is "I" or "F" or "B";
        int offset = hasExplicitResultType ? 1 : 0;

        return new ResultRecord
        {
            RecordTypeId = GetField(fields, 1),
            SequenceNumber = GetField(fields, 2),
            AssayNo = GetComponent(assay, 0),
            AssayName = GetComponent(assay, 1),
            ReplicateNumber = GetComponent(assay, 2),
            ResultType = hasExplicitResultType ? resultType : string.Empty,
            MeasurementValue = GetField(fields, 4 + offset),
            Unit = GetField(fields, 5 + offset),
            ReferenceRange = GetField(fields, 7 + offset),
            AbnormalFlag = GetField(fields, 8 + offset),
            ResultStatus = GetField(fields, 9 + offset),
            TestCompletedDateTime = GetField(fields, 13 + offset)
        };
    }

    private static ResultsCommentRecord ParseCommentRecord(string[] fields) => new()
    {
        RecordTypeId = GetField(fields, 1),
        SequenceNumber = GetField(fields, 2),
        CommentSource = GetField(fields, 3),
        CommentText = GetField(fields, 4),
        CommentType = GetField(fields, 5)
    };

    private static RequestInformationRecord ParseRequestInformationRecord(string[] fields)
    {
        string patientIdAndSpecimenId = GetField(fields, 3);
        return new RequestInformationRecord
        {
            RecordTypeId = GetField(fields, 1),
            SequenceNumber = GetField(fields, 2),
            PatientIdAndSpecimenId = patientIdAndSpecimenId,
            PatientId = GetComponent(patientIdAndSpecimenId, 0),
            SpecimenId = GetComponent(patientIdAndSpecimenId, 1),
            EndingRangeId = GetField(fields, 4),
            UniversalTestId = GetField(fields, 5),
            NatureOfRequestTimeLimits = GetField(fields, 6),
            BeginningRequestResultsDateTime = GetField(fields, 7),
            EndingRequestResultsDateTime = GetField(fields, 8),
            RequestingPhysicianName = GetField(fields, 9),
            RequestingPhysicianTelephoneNumber = GetField(fields, 10),
            UserField1 = GetField(fields, 11),
            UserField2 = GetField(fields, 12),
            RequestInformationStatusCode = GetField(fields, 13)
        };
    }

    private static MessageTerminatorRecord ParseTerminatorRecord(string[] fields) => new()
    {
        RecordTypeId = GetField(fields, 1),
        SequenceNumber = GetField(fields, 2),
        TerminationCode = GetField(fields, 3)
    };

    private static string[] SplitFields(string record)
    {
        string[] fields = record.Split('|');
        for (int i = 0; i < fields.Length; i++)
            fields[i] = Unescape(fields[i]);
        return fields;
    }

    private static string GetField(string[] fields, int fieldNumber)
    {
        if (fieldNumber <= 0 || fieldNumber > fields.Length)
            return string.Empty;

        return fields[fieldNumber - 1];
    }

    private static string GetComponent(string field, int componentIndex)
    {
        if (string.IsNullOrEmpty(field))
            return string.Empty;

        string[] parts = field.Split('^');
        return componentIndex < parts.Length ? Unescape(parts[componentIndex]) : string.Empty;
    }

    private static string Unescape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value
            .Replace("&F&", "|", StringComparison.Ordinal)
            .Replace("&S&", "^", StringComparison.Ordinal)
            .Replace("&R&", "\\", StringComparison.Ordinal)
            .Replace("&E&", "&", StringComparison.Ordinal);
    }

    public sealed class ParseResult
    {
        public HeaderRecord HeaderRecord { get; set; } = new HeaderRecord();
        public PatientRecord? PatientRecord { get; set; }
        public TestOrderRecord? TestOrderRecord { get; set; }
        public List<ResultRecord> ResultRecordList { get; set; } = [];
        public List<ResultsCommentRecord> ResultsCommentRecordList { get; set; } = [];
        public RequestInformationRecord? RequestInformationRecord { get; set; }
        public MessageTerminatorRecord? MessageTerminatorRecord { get; set; }
    }
}
