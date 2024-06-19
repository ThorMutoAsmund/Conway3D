using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3Int With(this Vector3Int self, int? x = null, int? y = null, int? z = null)
    {
        return new Vector3Int(x.HasValue ? x.Value : self.x, y.HasValue ? y.Value : self.y, z.HasValue ? z.Value : self.z);
    }

    public static Vector3Int Add(this Vector3Int self, int? x = null, int? y = null, int? z = null)
    {
        return new Vector3Int(self.x + (x.HasValue ? x.Value : 0), self.y + (y.HasValue ? y.Value : 0), self.z + (z.HasValue ? z.Value : 0));
    }

    public static Vector3Int[] Surrounding2D(this Vector3Int location)
    {
        return new Vector3Int[] { location.Add(x: -1), location.Add(x: 1), location.Add(y: -1), location.Add(y: 1),
            location.Add(x: -1, y: -1), location.Add(x: 1, y:1), location.Add(x: 1, y: -1), location.Add(x: -1, y: 1) };
    }
}
