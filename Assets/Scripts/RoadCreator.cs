using System;
using UnityEngine;
using Random = UnityEngine.Random;

//https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/

//to randomly generate a road for each new race, a line will be drawn within given bounds and filled with the custom mesh

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))] //both of these are required to visualize the mesh we generate here
public class RoadCreator : MonoBehaviour
{
    private Mesh _mesh;
    public GameObject finishLinePrefab;
    public GameObject carPrefab;

    public Vector3 startPoint;
    public Vector3 startDirection; //used to calculate player starting direction
    public Vector3 endPoint;

    private const int RoadStep = 5; //smaller number creates more frequent points
    private const int RoadVarianceX = RoadStep*5; //limit how far points can spawn from each other to get less jagged roads
    private const int RoadWidth = 10; //used to control how wide the road is
    private const int RoadRange = 150; //used to control the road's length

    void Start()
    {
        _mesh = CreateMesh(GeneratePath());
        Instantiate(finishLinePrefab, endPoint, Quaternion.identity);
        //TODO fix this, the quaternion isn't working 
        //spawning them at y-1 to avoid spawning inside the road
        Instantiate(carPrefab, new Vector3(startPoint.x, 1, startPoint.z), Quaternion.FromToRotation(startPoint, startDirection)); 

        GetComponent<MeshFilter>().mesh = _mesh;
        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }

    Vector3[] GeneratePath()
    {
        //for now the path will only ever progress linearly along the z axis
        Vector3[] path = new Vector3[RoadRange/RoadStep];
        
        for (int i = 0, zVal = 0, xVal = 0; i < path.Length; i++)
        {
            //start the road at any random x point within the boundaries of the race
            if (i == 0)
                xVal = Random.Range(0, RoadRange);
            else
                xVal = (int)Random.Range(path[i - 1].x - RoadVarianceX, path[i - 1].x + RoadVarianceX);
                    
            path[i] = new Vector3(xVal, 0, zVal);
            zVal += RoadStep; 
        }

        startPoint = path[1]; //start them a little into the path so they don't spawn right on the edge and fall off
        endPoint = path[path.Length-1];

        return path;
    }

    Mesh CreateMesh(Vector3[] path)
    {
        Vector3[] vertices = new Vector3[path.Length * 2];
        int[] triangles = new int[(path.Length - 1) * 6];

        //build quads based on the given line's points
        //ty for the equations to find vertices to the side of points along a path: https://www.youtube.com/watch?v=Q12sb-sOhdI&ab_channel=SebastianLague
        for (int i = 0, currentPoint = 0, triangleIndex = 0; i < path.Length; i++) 
        {
            Vector3 forward = Vector3.zero;

            //if the point isn't the last on the road
            if (i < path.Length - 1)
                forward += path[i + 1] - path[i];
            
            //if the point isn't the first one on the road
            if (i > 0)
                forward += path[i] - path[i - 1];

            //find the average to show the direction between consecutive points
            forward.Normalize();
            
            //get a starting direction for the car to spawn with
            if (currentPoint == 0)
            {
                startDirection = forward;
            }
            
            //find a point to the side of the center of the road
            Vector3 leftOfPath = new Vector3(-forward.z, 0, forward.x);

            //find the points to the left and right of the path
            vertices[currentPoint] = path[i] + leftOfPath * (RoadWidth * .5f);
            vertices[currentPoint + 1] = path[i] - leftOfPath * (RoadWidth * .5f);
            
            //make triangles based on those points to show the line in 3d space with the mesh
            if (i < path.Length - 1)
            {
                //the order you draw the triangles in matters for them to show up/have a collider on the right side of the y axis
                triangles[triangleIndex] = currentPoint;
                triangles[triangleIndex+1] = currentPoint+2;
                triangles[triangleIndex+2] = currentPoint+1;

                triangles[triangleIndex+3] = currentPoint+1;
                triangles[triangleIndex+4] = currentPoint+2;
                triangles[triangleIndex+5] = currentPoint+3;
            }
            
            currentPoint+=2;
            triangleIndex += 6;//this loop is done in steps of 6 because that's the number of vertices needed for a square
        }

        Mesh mesh = new Mesh {name = "Road Mesh"};
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}