using DeviceHub.Model.Entities;

namespace DeviceHub.Yhlo.Protocol;

/// <summary>
/// ASTM 记录解析（H/P/O/R/Q/L 等）
/// </summary>
public static class AstmRecordParser
{
  public sealed class ParseResult
  {
    public string SampleNo { get; set; } = string.Empty;

    public string Barcode { get; set; } = string.Empty;

    public ReceiveMessageDecode.TypeEnum Type { get; set; } = ReceiveMessageDecode.TypeEnum.TestResult;

    public List<ResultItem> Results { get; } = [];
  }

  public sealed class ResultItem
  {
    public string InstrumentItemCode { get; set; } = string.Empty;

    public string InstrumentItemName { get; set; } = string.Empty;

    public string ResultValue { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public string AbnormalFlag { get; set; } = string.Empty;

    public string TestTime { get; set; } = string.Empty;
  }

  public static ParseResult Parse(IReadOnlyList<string> frameDataList)
  {
    var result = new ParseResult();

    foreach (string frameData in frameDataList)
    {
      if (string.IsNullOrWhiteSpace(frameData))
        continue;

      foreach (string record in frameData.Split('\r', StringSplitOptions.RemoveEmptyEntries))
        ParseRecord(record.Trim(), result);
    }

    return result;
  }

  private static void ParseRecord(string record, ParseResult result)
  {
    if (record.Length < 2 || record[1] != '|')
      return;

    string[] fields = record.Split('|');
    switch (record[0])
    {
      case 'O':
        if (fields.Length > 3)
          result.SampleNo = GetFieldValue(fields[3]);
        if (fields.Length > 4)
          result.Barcode = GetFieldValue(fields[4]);
        break;

      case 'Q':
        result.Type = ReceiveMessageDecode.TypeEnum.SampleQuery;
        if (fields.Length > 2)
          result.Barcode = GetFieldValue(fields[2]);
        if (fields.Length > 3 && string.IsNullOrEmpty(result.SampleNo))
          result.SampleNo = GetFieldValue(fields[3]);
        break;

      case 'R':
        var item = new ResultItem();
        if (fields.Length > 3)
        {
          item.InstrumentItemCode = GetComponent(fields[2], 0);
          item.InstrumentItemName = GetComponent(fields[2], 1);
        }

        if (fields.Length > 4)
          item.ResultValue = fields[3];
        if (fields.Length > 5)
          item.Unit = fields[4];
        if (fields.Length > 7)
          item.AbnormalFlag = fields[6];
        if (fields.Length > 13)
          item.TestTime = fields[12];

        result.Results.Add(item);
        break;
    }
  }

  private static string GetFieldValue(string field) => GetComponent(field, 0);

  private static string GetComponent(string field, int index)
  {
    if (string.IsNullOrEmpty(field))
      return string.Empty;

    string[] parts = field.Split('^');
    return index < parts.Length ? parts[index] : string.Empty;
  }
}
