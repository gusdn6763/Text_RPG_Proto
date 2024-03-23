using System.Collections.Generic;
using System;

public class Graph
{
    public List<Tuple<int, Status>>[] list;
    public List<Location> locations;

    public Graph(int vertexCount)
    {
        list = new List<Tuple<int, Status>>[vertexCount];
        locations = new List<Location>(vertexCount);

        for (int i = 0; i < vertexCount; i++)
            list[i] = new List<Tuple<int, Status>>();
    }

    public void AddEdge(Location from, Location to)
    {
        if (locations.Contains(from) == false)
            locations.Add(from);

        if (locations.Contains(to) == false)
            locations.Add(to);

        list[from.vertex].Add(new Tuple<int, Status>(to.vertex, to.status));
        list[to.vertex].Add(new Tuple<int, Status>(from.vertex, to.status));
    }

    public int GetVertexFromName(string locationName)
    {
        for (int i = 0; i < locations.Count; i++)
        {
            if (locations[i].locationName == locationName)
                return locations[i].vertex;
        }

        return -1;
    }
}