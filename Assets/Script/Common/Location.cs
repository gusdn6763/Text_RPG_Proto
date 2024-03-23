
[System.Serializable]
public class Location
{
    private static int nextVertexId = 0;

    public Status status;
    public string locationName;
    public int vertex;

    public Location()
    {
        vertex = nextVertexId++;
    }

    public Location(string locationName, Status status)
    {
        vertex = nextVertexId++;
        this.locationName = locationName;
        this.status = status;
    }
}