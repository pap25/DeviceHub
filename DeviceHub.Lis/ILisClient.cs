using DeviceHub.Lis.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Lis
{
    /**
     * 0. 查询设备配置
     * 1. 检验结果上传（最核心）
     * 2. 医嘱/申请单下载
     * 3. 样本信息同步
     * 4. 检验项目字典同步
     * 5. 设备状态监控
     * 6. 质控数据上传
     * 7. 仪器报警上传
     * 8. 校准数据上传
     * 9. 结果回写确认
     * 10. 历史结果查询
     * **/
    public interface ILisClient
    {
        // 查询设备配置
        public SerialPortConfig querySerialPortConfigById(int id);

        // 上传结果
        //Task UploadResultAsync(TestResult result);

        //// 下载申请单
        //Task<OrderInfo?> GetOrderAsync(string sampleNo);

        //// 查询样本信息
        //Task<SampleInfo?> GetSampleAsync(string sampleNo);

        //// 上传设备状态
        //Task UploadDeviceStatusAsync(DeviceStatus status);

        //// 上传报警
        //Task UploadAlarmAsync(DeviceAlarm alarm);

        //// 上传质控
        //Task UploadQcAsync(QcResult qc);
    }
}
