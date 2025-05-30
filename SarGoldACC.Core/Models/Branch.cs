﻿using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Models;

public class Branch
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Counterparty> Counterparties { get; set; }
    public ICollection<Box> Boxes { get; set; }
}