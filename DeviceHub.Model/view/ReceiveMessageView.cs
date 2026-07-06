using DeviceHub.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Model.view
{
    public class ReceiveMessageView
    {
        public ReceiveMessage.StatusEnum Status { get; set; }
        public byte[] RawMessage { get; set; }
        public string ResultJson { get; set; }
        public ReceiveMessageDecode.TypeEnum? Type { get; set; }
        public string SampleNo { get; set; }
        public string Barcode { get; set; }
        public long CreateTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
