using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

/// <summary>
/// Enum extensions for ease of use.
/// </summary>
public static class EnumExtensions
{
    public static bool IsFlagSet(this Enum variable, Enum value)
    {
        if (variable == null)
        {
            return false;
        }
          
        if (value == null)
        {
            throw new ArgumentNullException("value");
        }
            
        if (!Enum.IsDefined(variable.GetType(), value))
        {
            throw new ArgumentException(string.Format("Enumeration type mismatch. The flag is of type '{0}', was expecting '{1}'.", value.GetType(), variable.GetType()));
        }

        ulong num = Convert.ToUInt64(value);
        return ((Convert.ToUInt64(variable) & num) == num);

    }

    public static IEnumerable<Enum> GetFlags(this Enum input)
    {
        foreach (Enum value in Enum.GetValues(input.GetType()))
        {
            if (input.HasFlag(value))
            {
                yield return value;
            }       
        }   
    }

    public static Enum SetFlags(this Enum value, Enum flags, bool enabled) 
    {
        if (enabled)
        {
            ulong longFlags = Convert.ToUInt64(flags);
            ulong longValue = Convert.ToUInt64(value);
            return (Enum)(object)(longValue | longFlags);
        }
        else
        {
            ulong longFlags = Convert.ToUInt64(flags);
            ulong longValue = Convert.ToUInt64(value);
            return (Enum)(object)(longValue & ~longFlags);
        }
    }

    public static Enum SetFlags(this Enum value, Enum flags)
    {
        return value.SetFlags(flags, true);
    }

    public static Enum ClearFlags(this Enum value, Enum flags)
    {
        return value.SetFlags(flags, false);
    }
}
