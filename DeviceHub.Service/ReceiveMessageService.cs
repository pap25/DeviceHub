using DeviceHub.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Service
{
    public class ReceiveMessageService
    {
        private static readonly ReceiveMessageService _instance = new();
        public static ReceiveMessageService Instance => _instance;
        private ReceiveMessageService()
        {
        }

        private ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;


    }
}
