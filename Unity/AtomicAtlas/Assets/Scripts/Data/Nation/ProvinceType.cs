using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Data
{
    [Flags]
    public enum ProvinceType // Dom 5 ProvinceType flags, subject to change when Dom 6 is released
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

    /// <summary>
    /// ProvinceType enum extensions for ease of use.
    /// </summary>
    public static class ProvinceTypeEnumExtensions
    {
        public static bool IsFlagSet(this ProvinceType value, ProvinceType flag)
        {
            var val_long = (long)value;
            var flag_long = (long)flag;
            return ((val_long & flag_long) != 0) || (flag_long == 0);
        }

        public static IEnumerable<ProvinceType> GetFlags(this ProvinceType value)
        {
            foreach (var flag in Enum.GetValues(typeof(ProvinceType)).Cast<ProvinceType>())
            {
                if (value.IsFlagSet(flag))
                    yield return flag;
            }
        }

        public static ProvinceType SetFlags(this ProvinceType value, ProvinceType flags, bool on)
        {
            var lValue = (long)value;
            var lFlag = (long)flags;
            if (on)
            {
                lValue |= lFlag;
            }
            else
            {
                lValue &= (~lFlag);
            }
            return (ProvinceType)Enum.ToObject(typeof(ProvinceType), lValue);
        }

        public static ProvinceType AddFlags(this ProvinceType value, ProvinceType flags)
        {
            return value.SetFlags(flags, true);
        }

        public static ProvinceType RemoveFlags(this ProvinceType value, ProvinceType flags)
        {
            return value.SetFlags(flags, false);
        }

        public static ProvinceType CombineFlags(this IEnumerable<ProvinceType> flags)
        {
            long lValue = 0;
            foreach (var flag in flags)
            {
                var lFlag = (long)(flag);
                lValue |= lFlag;
            }
            return (ProvinceType)Enum.ToObject(typeof(ProvinceType), lValue);
        }
    }
}