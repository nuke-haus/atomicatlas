using System.Collections.Generic;
using System.Linq;

public enum Fort
{
    NONE = 0,
    PALISADE = 1,
    ROCKWALLS = 5,
    KELP = 9,
    BRAMBLES = 10,
    CITYPALISADE = 11,
    ICEWALLS = 20,
    WOODENFORT = 28
}

public static class FortHelper
{
    public static Fort GetFort(Node node)
    {
        var terrain = node.Terrain;

        if (terrain.HasFlag(Terrain.SEA))
        {
            return Fort.KELP;
        }

        List<Fort> forts = new List<Fort> { Fort.PALISADE };

        if (terrain.HasFlag(Terrain.FOREST))
        {
            forts.Add(Fort.BRAMBLES);
            forts.Add(Fort.WOODENFORT);
        }
        if (terrain.HasFlag(Terrain.HIGHLAND) || terrain.HasFlag(Terrain.WASTE))
        {
            forts.Add(Fort.ROCKWALLS);
        }
        if (terrain.HasFlag(Terrain.COLDER))
        {
            forts.Add(Fort.ICEWALLS);
        }
        if (terrain.HasFlag(Terrain.SWAMP))
        {
            forts.Add(Fort.WOODENFORT);
        }
        if (terrain.HasFlag(Terrain.FARM) || terrain.HasFlag(Terrain.LARGER) || terrain == Terrain.PLAINS)
        {
            forts.Add(Fort.CITYPALISADE);
            forts.Add(Fort.PALISADE);
        }
        if (node.Connections.Any(connection => connection.ConnectionType == ConnectionType.MOUNTAIN_PASS || connection.ConnectionType == ConnectionType.MOUNTAIN)) 
        {
            forts.Add(Fort.ROCKWALLS);
        }

        return forts.GetRandom();
    }
}