namespace DeviceHub.Lis.Dto;

public class UploadSpecimenTestResultInput
{
    public long InstrumentId { get; set; }
    public string SampleNo { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public List<TestResultItem> Items { get; set; } = [];
}
