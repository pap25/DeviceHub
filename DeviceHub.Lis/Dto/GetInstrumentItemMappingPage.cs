namespace DeviceHub.Lis.Dto
{
    public class GetInstrumentItemMappingPage
    {
        /// <summary>
        /// 仪器项目编号
        /// </summary>
        public string InstrumentItemCode { get; set; }

        /// <summary>
        /// 仪器项目名
        /// </summary>
        public string InstrumentItemName { get; set; }

        /// <summary>
        /// LIS项目编号
        /// </summary>
        public string LisItemCode { get; set; }

        /// <summary>
        /// LIS项目名
        /// </summary>
        public string LisItemName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}
