namespace SarGoldACC.Core.DTOs;

public class ResultDto
{
    public required bool Success { get; set; }
    public string Message { get; set; }
    public Object Data { get; set; }
}