using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementSearchItemTable;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementSearchItem
{
    public PlacementParameters Parameters
    {
        get { if (Field02.Count != 1) throw new ArgumentException($"Invalid {nameof(Field02)}"); return Field02[0]; }
        set { if (Field02.Count != 1) throw new ArgumentException($"Invalid {nameof(Field02)}"); Field02[0] = value; }
    }

    public override string ToString() => $"SearchItem(\"{Identifier}\", 0x{Hash01:X16}, {Parameters}, {Rate}, {Field03}, {Field04})";

    public IEnumerable<PlacementLocation> GetContainingLocations(IList<PlacementLocation> locations)
    {
        var result = new List<PlacementLocation>();
        foreach (var loc in locations)
        {
            if (!loc.IsNamedPlace)
                continue;
            if (loc.Contains(Parameters.Coordinates))
                result.Add(loc);
        }
        if (result.Count == 0)
            result.Add(locations.First(l => l.IsNamedPlace)); // base area name
        return result;
    }
}
