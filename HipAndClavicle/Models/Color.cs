﻿using NuGet.Packaging.Signing;
using System;
namespace HipAndClavicle.Models;

public class Color
{
    public int ColorId { get; set; }
    public string? ColorName { get; set; }
    public string? HexValue { get; set; }
    [NotMapped]
    public (int, int, int) RGB
    {

        get => (Red, Green, Blue);
        set => (Red, Green, Blue) = value;
    }

    [Range(0, 255)]
    public int Red { get; set; } = 0;
    [Range(0, 255)]
    public int Blue { get; set; } = 0;
    [Range(0, 255)]
    public int Green { get; set; } = 0;

}
