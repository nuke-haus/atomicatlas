using System;

[Flags]
public enum Terrain : ulong
{
    PLAINS = 0,
    SMALLER = 1,
    LARGER = 2,
    SEA = 4,
    FRESH_WATER = 8,
    HIGHLAND = 16,
    SWAMP = 32,
    WASTE = 64,
    FOREST = 128,
    FARM = 256,
    NO_START = 512,
    MANY_SITES = 1024,
    DEEP_SEA = 2048,
    CAVE = 4096,
    MOUNTAINS = 8388608,
    THRONE = 33554432,
    GENERIC_START = 67108864,
    WARMER = 1073741824,
    COLDER = 2147483648,
    CAVE_WALL = 68719476736
}