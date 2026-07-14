using DeviceHub.Abstractions.Dto;
using DeviceHub.Template.Template.Tcp;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Handler;
using DeviceHub.YhloTestTcpServer.Protocol;

namespace DeviceHub.YhloTestTcpServer
{
    public class TcpServerSession : TcpServerSessionBase
    {
        private readonly string logType = nameof(TcpServerSession);
        public void Start(long instrumentId, TcpConfig config)
        {
            base.Start(instrumentId, config, new ReceiveHandler(instrumentId));
        }

        public override byte[]? GetReplyAckMessage(byte[] rawMessage)
        {
            Hl7MessageEntity.MshSegment? msh = Hl7MessageDecode.ParseMsh(rawMessage);
            if (msh is null)
            {
                Logger.Warn(logType, "无法构建ACK: 报文缺少MSH段");
                return null;
            }

            return Hl7MessageEncoder.EncoderAck(msh);
        }
    }
}
