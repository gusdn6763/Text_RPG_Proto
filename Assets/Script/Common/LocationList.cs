using System;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    public static List<Tuple<int, Status>> DijkstraAlgorithm(int start, int V, Graph graph)
    {
        List<Tuple<int, Status>> distWithStatus = new List<Tuple<int, Status>>();
        for (int i = 0; i < V; i++)
        {
            distWithStatus.Add(new Tuple<int, Status>(int.MaxValue, new Status()));
        }
        distWithStatus[start] = new Tuple<int, Status>(0, new Status());

        PriorityQueue<Tuple<int, int>> q = new PriorityQueue<Tuple<int, int>>((x, y) => x.Item1.CompareTo(y.Item1));
        q.Enqueue(new Tuple<int, int>(0, start));

        while (q.Count != 0)
        {
            int cost = -q.Peek().Item1;
            int from = q.Peek().Item2;
            q.Dequeue();

            if (distWithStatus[from].Item1 < cost)
                continue;

            Status totalStatus = graph.locations[from].status;

            foreach (var tuple in graph.list[from])
            {
                int to = tuple.Item1;
                int distFromTo = cost + 1;
                if (distFromTo < distWithStatus[to].Item1)
                {
                    distWithStatus[to] = new Tuple<int, Status>(distFromTo, distWithStatus[from].Item2 + tuple.Item2);
                    q.Enqueue(new Tuple<int, int>(-distFromTo, to));
                }
            }
        }
        return distWithStatus;
    }
}

public class PriorityQueue<T>
{
    private List<T> data;
    private Comparison<T> comparison;

    public PriorityQueue(Comparison<T> comparison)
    {
        this.data = new List<T>();
        this.comparison = comparison;
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int ci = data.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (comparison(data[ci], data[pi]) >= 0) break;
            T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
            ci = pi;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0) 
            throw new InvalidOperationException("Queue is empty.");

        T frontItem = data[0];
        data[0] = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);

        int ci = 0;
        while (ci < data.Count)
        {
            int li = 2 * ci + 1, ri = 2 * ci + 2;
            if (li >= data.Count) break;
            int mi = ri >= data.Count || comparison(data[li], data[ri]) < 0 ? li : ri;
            if (comparison(data[ci], data[mi]) <= 0) break;
            T tmp = data[ci]; data[ci] = data[mi]; data[mi] = tmp;
            ci = mi;
        }
        return frontItem;
    }

    public T Peek()
    {
        if (data.Count == 0) throw new InvalidOperationException("Queue is empty.");
        return data[0];
    }

    public int Count
    {
        get { return data.Count; }
    }
}

public class LocationList : MonoBehaviour
{
    [SerializeField] private int locationCount;

    private Graph graph;

    public int LocationCount { get => locationCount; set => locationCount = value; }

    private void Awake()
    {
        graph = new Graph(locationCount);
        TestTmp();
    }

    public void TestTmp()
    {
        Location loc0 = new Location("����", new Status(0, 0, 0, 0));
        Location loc1 = new Location("����", new Status(0, -1, -1, 1));
        Location loc2 = new Location("����", new Status(0, -2, -2, 2));         
        Location loc3 = new Location("����", new Status(0, -3, -3, 3));
        Location loc4 = new Location("���� ����",new Status(0, -4, -4, 4));
        Location loc5 = new Location("��ȭ��", new Status(0, -5, -5, 5));
        Location loc6 = new Location("���", new Status(0, -6, -6, 6));
        Location loc7 = new Location("����", new Status(0, -7, -7, 7));
        Location loc8 = new Location("��", new Status(0, -8, -8, 8));
        Location loc9 = new Location("�Ĵ�", new Status(0, -9, -9, 9));

        graph.AddEdge(loc0, loc1);   //����, ����
        graph.AddEdge(loc1, loc2);   //����, ����
        graph.AddEdge(loc1, loc3);   //����, ����
        graph.AddEdge(loc2, loc4);   //����, ���� ����
        graph.AddEdge(loc2, loc5);   //����, ��ȭ��
        graph.AddEdge(loc3, loc6);   //����, ���
        graph.AddEdge(loc3, loc7);   //����, ����
        graph.AddEdge(loc7, loc8);   //����, �Ĵ�
        graph.AddEdge(loc7, loc9);   //����, ��
    }

    public List<Tuple<Location, Status>> CaculateAllPathsStatusFromName(string fromLocation)
    {
        int currentVertex = graph.GetVertexFromName(fromLocation);
        List<Tuple<int, Status>> shortestPaths = Dijkstra.DijkstraAlgorithm(currentVertex, 10, graph);

        List<Tuple<Location, Status>> result = new List<Tuple<Location, Status>>();
        for (int i = 0; i < shortestPaths.Count; i++)
        {
            if (shortestPaths[i].Item1 != int.MaxValue)
            {
                Location location = graph.locations[i];
                Status status = shortestPaths[i].Item2;
                result.Add(new Tuple<Location, Status>(location, status));
            }
            else
                Debug.LogError("ã�� ���ϴ� ��ΰ� �����ϴµ�?");
        }

        return result;
    }

    public List<Location> GetAllLocation()
    {
        List<Location> allLocations = new List<Location>();

        foreach (var location in graph.locations)
            allLocations.Add(location);

        return allLocations;
    }

    public List<string> SearchNearLocationFromName(string fromLocation)
    {
        int currentVertex = graph.GetVertexFromName(fromLocation);
        List<string> nearLocations = new List<string>();

        foreach (var edge in graph.list[currentVertex])
        {
            // ���� ��ġ���� �̿��� ��ġ�� ã�� �߰��մϴ�.
            nearLocations.Add(graph.locations[edge.Item1].locationName);
        }

        return nearLocations;
    }
}