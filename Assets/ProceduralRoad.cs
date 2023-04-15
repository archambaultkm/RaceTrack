using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/

//to randomly generate a road for each new race, a grid of vertices will need to be drawn and filled with triangles to form the mesh.
//There should be a method to randomize the shape of this grid to give new road directions every run.
//The road should have inclines/declines, corners, and a border all the way around to prevent the car from falling off (or not?)

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))] //both of these are required to visualize the mesh we generate here
public class ProceduralRoad : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector3[] _normals;
    private int[] _triangles;
    
    //for generating grid: to start just make a rectangle
    private const int RoadLength = 40;
    private const int RoadWidth = 25;
    
    void Start()
    {
        _mesh = new Mesh {name = "Road Mesh"};
        CreateRoad();
        UpdateMesh();
        
        GetComponentInParent<MeshFilter>().mesh = _mesh; //take the meshfilter that's been assigned in unity
        GetComponentInParent<MeshCollider>().sharedMesh = _mesh; 
    }

    private void CreateRoad()
    {
        //total number of vertices will be lenght+1 * width+1
        _vertices = new Vector3[(RoadLength + 1) * (RoadWidth + 1)];

        for (int currentVertex = 0, z = 0; z <= RoadLength; z++) //the road's length will be represented on the z axis
        {
            for (int x = 0; x <= RoadWidth; x++) //the road;s width will be represented on the x axis
            {
                _vertices[currentVertex] = new Vector3(z, 0, x);
                currentVertex++;
            }
        }

        //this loop is done in steps of 6 because that's the number of vertices needed for a square
        //the index values in the triangle array have to be in order, because 3 consecutive index values form one triangle.
        //This is why you can't "cut corners" on duplicate vertices in the case of quads. 
        _triangles = new int[RoadLength * RoadWidth * 6];

        var vertexIndex = 0;
        var triangleIndex = 0;

        for (int z = 0; z < RoadLength; z++)
        {
            for (int x = 0; x < RoadWidth; x++)
            {
                _triangles[triangleIndex] = vertexIndex;
                _triangles[triangleIndex+1] = vertexIndex+RoadWidth+1;
                _triangles[triangleIndex+2] = vertexIndex+1;
                _triangles[triangleIndex+3] = vertexIndex+1;
                _triangles[triangleIndex+4] = vertexIndex+RoadWidth+1;
                _triangles[triangleIndex+5] = vertexIndex+RoadWidth+2;

                vertexIndex++;
                triangleIndex += 6;
            } 
        }
    }

    private void UpdateMesh()
    {
        _mesh.vertices = _vertices;
        //_mesh.normals = _normals; //normals are used by shader to calculate lighting
        _mesh.triangles = _triangles;
    }
    
}
