using UnityEngine;

//https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/

//to randomly generate a road for each new race, a grid of vertices will need to be drawn and filled with triangles to form the mesh.

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))] //both of these are required to visualize the mesh we generate here
public class ProceduralRoad : MonoBehaviour
{
    private Mesh _mesh;
    public Vector3 _startPoint;
    public Vector3 _endPoint;

    public const int RoadStep = 10; //how often to create a new point in the road
    public const int RoadWidth = 20; //used to control how wide the road is
    public const int RoadRange = 100; //used to control how long the road can go. For now it will only ever be a straight road (z axis increases)

    void Start()
    {
        _mesh = CreateMesh(GeneratePath());

        GetComponent<MeshFilter>().mesh = _mesh;
        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }

    Vector3[] GeneratePath()
    {
        Vector3[] path = new Vector3[RoadRange/RoadStep]; 

        for (int i = 0, zVal = 0, xVal = 0; i < path.Length; i++)
        {
            if (i > 0) //TODO you can get rid of the conditional once you have the car spawning at the first point of the road
                xVal = Random.Range(0, RoadRange);
            
            path[i] = new Vector3(xVal, 0, zVal);
            zVal += RoadStep; //I'm using the road width just for a constant value, might change to move by a different step
        }

        _startPoint = path[0];
        _endPoint = path[path.Length];

        return path;
    }

    Mesh CreateMesh(Vector3[] pathPoints)
    {
        Vector3[] vertices = new Vector3[pathPoints.Length * 2];
        int[] triangles = new int[(pathPoints.Length - 1) * 6];

        //build quads based on the given line's points
        //ty for the equations to find vertices to the side of points along a path: https://www.youtube.com/watch?v=Q12sb-sOhdI&ab_channel=SebastianLague
        for (int i = 0, currentPathPoint = 0, triangleIndex = 0; i < pathPoints.Length; i++) 
        {
            Vector3 forward = Vector3.zero;
            
            //if the point isn't the last on the road
            if (i < pathPoints.Length - 1)
                forward += pathPoints[i + 1] - pathPoints[i];
            
            //if the point isn't the first one on the road
            if (i > 0)
                forward += pathPoints[i] - pathPoints[i - 1];

            //find the average to show the direction between consecutive points
            forward.Normalize();
            //find a point to the side of the center of the road
            Vector3 leftOfPath = new Vector3(-forward.z, 0, forward.x);

            //find the points to the left and right of the path
            vertices[currentPathPoint] = pathPoints[i] + leftOfPath * (RoadWidth * .5f);
            vertices[currentPathPoint + 1] = pathPoints[i] - leftOfPath * (RoadWidth * .5f);
            
            //make triangles based on those points to show the line in 3d space with the mesh
            if (i < pathPoints.Length - 1)
            {
                //the order you draw the triangles in matters for them to show up/have a collider on the right side of the y axis
                triangles[triangleIndex] = currentPathPoint;
                triangles[triangleIndex+1] = currentPathPoint+2;
                triangles[triangleIndex+2] = currentPathPoint+1;

                triangles[triangleIndex+3] = currentPathPoint+1;
                triangles[triangleIndex+4] = currentPathPoint+2;
                triangles[triangleIndex+5] = currentPathPoint+3;
            }
            
            currentPathPoint+=2;
            triangleIndex += 6;//this loop is done in steps of 6 because that's the number of vertices needed for a square
        }

        Mesh mesh = new Mesh {name = "Road Mesh"};
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}