using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalGenerator : MonoBehaviour {

    public string seed;
    [Range(0.1f, 1.0f)]
    public float roughness = 0.5f;

    System.Random RandomValues;
    float[,] heightmap;
    TerrainData td;
    private int hash;
    public int size;
    int max;

    void Awake()
    {

        td = GetComponent<Terrain>().terrainData;
        size = td.heightmapResolution;
        max = size - 1;
    }

    //garthering the x and y values of the heightmap
    void Start()
    {
        heightmap = new float[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                heightmap[x, y] = 0.5f;
            }
        }
        //calling GenerateTerrain
        GenerateTerrain();
    }

    //generating the terrain and storing it in the hash key
    void GenerateTerrain()
    {
        if (seed != "")
        {
            hash = seed.GetHashCode();
            RandomValues = new System.Random(hash);
        }
        else
        {
            RandomValues = new System.Random();
        }
        FractalCreation(max);
        td.SetHeights(0, 0, heightmap);
    }

    //FractalCreation get the two methods diamonds and squares and applies them to the fractal equation to create a realistic map
    void FractalCreation(int size)
    {
        int half = size / 2;
        if (half < 1)
        {
            return;
        }
        for (int y = half; y < max; y += size)
        {
            for (int x = half; x < max; x += size)
            {
                //making sure the floating point numbers of the x and y values are random everytime they're generated
                Square(x, y, half, ((float)RandomValues.NextDouble() - 0.5f) / (max / size));
            }
        }
        for (int y = 0; y <= max; y += half)
        {
            for (int x = (y + half) % size; x <= max; x += size)
            {
                //making sure the floating point numbers of the x and y values are random everytime they're generated
                Diamond(x, y, half, ((float)RandomValues.NextDouble() - 0.5f) / (max / size));
            }
        }
        FractalCreation(half);
    }

    //diamonds are created on the terrain to make the terrain itself seem rougher example a rocky terrain
    void Diamond(int x, int y, int size, float offset)
    {
        float ave = Average(new float[4]{
            Get(x, y - size),
            Get(x + size, y),
            Get(x, y + size),
            Get(x - size, y)});

        heightmap[x, y] = ave + offset * roughness;
    }

    //Squares are created on the terrain to make the terrain itself seem rougher example a rocky terrain
    void Square(int x, int y, int size, float offset)
    {
        float ave = Average(new float[4]{
        Get (x - size, y - size),
        Get (x + size, y - size),
        Get (x + size, y + size),
        Get (x - size, y + size)});

        heightmap[x, y] = ave + offset * roughness;
    }

    //making sure the x and y vlaues of the heightmap are above 0 but not bigger than max value
    float Get(int x, int y)
    {
        if (x < 0 || x > max || y < 0 || y > max)
        {
            return 0;
        }
        return heightmap[x, y];
    }

    //finds the average of the float values and divides them by total
    float Average(float[] values)
    {
        float total = 0;
        foreach (float f in values)
        {
            total += f;
        }
        return total / values.Length;
    }


}
