using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class Map : MonoBehaviour {
    Mesh mesh;
     
    //vector3 called vertices used to create a mesh
    Vector3[] vertices;
    int[] triangles;

    //x and y values of the plane
    public int xSize = 20;
    public int zSize = 20;

	
	void Start ()
    {
        //creates a mesh
        mesh = new Mesh();
        //gets the mesh component and assigns it to mesh
        GetComponent<MeshFilter>().mesh = mesh;

        //starts the coroutine to create the triangles
        StartCoroutine(CreateShape());
        
	}

    private void Update()
    {
        //updates the mesh every frame as each vertice is populated with a value 
        UpdateMesh();
    }

    
    IEnumerator CreateShape()
    {
        //populates the vertices with the xSize value and the zSize values 
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                //manipulates the y float with the perlin noise equation timesing both the x and z values by random integers between 0 and 1
                float y = Mathf.PerlinNoise(x * Random.Range(0.0f, 1.0f), z * Random.Range(0.0f, 1.0f)) * 3f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        //fills tirangles with a new integer created by timesing xSize by zSize and the number 6
        //number 6 being the number of triangles created below 
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        
        //for loop to repeat the creation of the triangles until the grid is filled
        for(int z = 0; z < zSize; z++)
        {
            //second for loop to create the original siz triangles 
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

                yield return new WaitForSeconds(.01f);
            }
            vert++;
        }
        


     


    }

    //makes sure the mesh is clear to begin with before filling it with the vertices and triangles.
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    //draws a sphere used for testing the y value of the map
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
