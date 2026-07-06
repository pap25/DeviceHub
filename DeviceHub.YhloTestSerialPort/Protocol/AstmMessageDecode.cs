using DeviceHub.Model.Entities;
using System.Reflection.PortableExecutable;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.Yhlo.Protocol;

public static class AstmMessageDecode
{
    public static ParseResult Parse(List<string> frameDataList)
    {
        return null;
    }
    public sealed class ParseResult
    {
        public HeaderRecord HeaderRecord { get; set; }
        public PatientRecord PatientRecord { get; set; }
        public TestOrderRecord TestOrderRecord { get; set; }

        public List<ResultRecord> ResultRecordList { get; set; }
        public List<ResultsCommentRecord> ResultsCommentRecordList { get; set; }

        public MessageTerminatorRecord MessageTerminatorRecord { get; set; }
    }
}