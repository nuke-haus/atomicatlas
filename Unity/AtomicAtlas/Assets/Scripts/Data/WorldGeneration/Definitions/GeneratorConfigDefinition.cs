using System;

[Flags]
public enum ProvinceType // Dom 5 terrain flags, subject to change when Dom 6 is released
{
    PLAINS = 0,
    SMALLER = 1,
    LARGER = 2,
    SEA = 4,
    FRESHWATER = 8,
    HIGHLAND = 16,
    SWAMP = 32,
    WASTE = 64,
    FOREST = 128,
    FARM = 256,
    NOSTART = 512,
    MANYSITES = 1024,
    DEEPSEA = 2048,
    CAVE = 4096,
    MOUNTAIN = 4194304,
    THRONE = 16777216,
    START = 33554432,
    NOTHRONE = 67108864,
    WARMER = 536870912,
    COLDER = 1073741824
}

public class GeneratorConfigDefinition
{
    // TODO: Copy over all generator settings stuff from mapnuke
}
