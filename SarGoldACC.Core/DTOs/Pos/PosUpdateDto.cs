﻿namespace SarGoldACC.Core.DTOs.Pos;

public class PosUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
}