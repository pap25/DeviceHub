using DeviceHub.Template.Template.Tcp;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Protocol;

namespace DeviceHub.YhloTestTcpServer
{
    public class TcpServerSession : TcpServerSessionBase
    {
        private readonly string logType = nameof(TcpServerSession);

        protected override byte[]? GetReplyAckMessage(byte[] rawMessage)
        {
            Hl7MessageEntity.MshSegment? msh = Hl7MessageDecode.ParseMsh(rawMessage, MessageEncoding);
            if (msh is null)
            {
                Logger.Warn(logType, "无法构建ACK: 报文缺少MSH段");
                return null;
            }

            return Hl7MessageEncoder.EncoderAck(msh, MessageEncoding);
        }
    }
}
