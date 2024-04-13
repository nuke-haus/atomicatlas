using System;

namespace Atlas.WorldGen
{
    [Flags]
    public enum ConnectionType
    {
        STANDARD = 0,
        MOUNTAIN_PASS = 1,
        RIVER = 2,
        MOUNTAIN = 4
    } 
}