using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometricGenerator : MonoBehaviour
{
    public Vector3 dimensionsParallelipiped = new Vector3(1f, 1f, 1f); // Размеры параллелепипеда

    public float radiusSphere = 1f; // Радиус сферы
    public int latitudeSegmentsSphere = 20; // Количество сегментов по широте
    public int longitudeSegmentsSphere = 20; // Количество сегментов по долготе

    public float widthPrism = 1f; // Ширина призмы
    public float heightPrism = 1f; // Высота призмы
    public int numSidesPrism = 6; // Количество сторон призмы

    public float CapsuleRadius = 1f; // Радиус капсулы
    public float CapsuleHeight = 2f; // Высота капсулы
    public int CapsuleNumSegments = 12; // Количество сегментов капсулы

    bool SpawnMesh1 = false;
    bool SpawnMesh2 = false;
    bool SpawnMesh3 = false;
    bool SpawnMesh4 = false;


    void Update()
    {
        // Добавление компонента MeshFilter к игровому объекту
        MeshFilter m_f = GetComponent<MeshFilter>();
        // Создание массива вершин и треугольников
        Mesh mesh = new Mesh();
        m_f.mesh = mesh;

        if(SpawnMesh1 == true || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Parallelepiped(mesh);
            SpawnMesh1 = true;
        }
        if (SpawnMesh2 == true || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Sphere(mesh);
            SpawnMesh2 = true;
        }
        if (SpawnMesh3 == true || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Prism(mesh);
            SpawnMesh3 = true;
        }
        if (SpawnMesh4 == true || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Capsule(mesh);
            SpawnMesh4 = true;
        }

        // Расчет нормалей для освещения
        mesh.RecalculateNormals();

        // Создание материала синего цвета
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.blue;

        // Установка материала на компонент MeshRenderer
        MeshRenderer m_r = GetComponent<MeshRenderer>();
        m_r.material = material;
    }


    private void Parallelepiped(Mesh mesh)
    {
        //Вершины 
        Vector3[] vert = new Vector3[]
        {
            new Vector3(-dimensionsParallelipiped.x * 0.5f, -dimensionsParallelipiped.y * 0.5f, -dimensionsParallelipiped.z * 0.5f), // Вершина 0
            new Vector3(dimensionsParallelipiped.x * 0.5f, -dimensionsParallelipiped.y * 0.5f, -dimensionsParallelipiped.z * 0.5f),  // Вершина 1
            new Vector3(dimensionsParallelipiped.x * 0.5f, dimensionsParallelipiped.y * 0.5f, -dimensionsParallelipiped.z * 0.5f),   // Вершина 2
            new Vector3(-dimensionsParallelipiped.x * 0.5f, dimensionsParallelipiped.y * 0.5f, -dimensionsParallelipiped.z * 0.5f),  // Вершина 3
            new Vector3(-dimensionsParallelipiped.x * 0.5f, -dimensionsParallelipiped.y * 0.5f, dimensionsParallelipiped.z * 0.5f),  // Вершина 4
            new Vector3(dimensionsParallelipiped.x * 0.5f, -dimensionsParallelipiped.y * 0.5f, dimensionsParallelipiped.z * 0.5f),   // Вершина 5
            new Vector3(dimensionsParallelipiped.x * 0.5f, dimensionsParallelipiped.y * 0.5f, dimensionsParallelipiped.z * 0.5f),    // Вершина 6
            new Vector3(-dimensionsParallelipiped.x * 0.5f, dimensionsParallelipiped.y * 0.5f, dimensionsParallelipiped.z * 0.5f)    // Вершина 7
        };

        //треугольники
        int[] tri = new int[]
            {
            // Передняя грань
            0, 2, 1,  // Треугольник 1
            0, 3, 2,  // Треугольник 2

            // Задняя грань
            4, 6, 5,  // Треугольник 3
            4, 7, 6,  // Треугольник 4

            // Верхняя грань
            3, 6, 2,  // Треугольник 5
            3, 7, 6,  // Треугольник 6

            // Нижняя грань
            0, 1, 5,  // Треугольник 7
            0, 5, 4,  // Треугольник 8

            // Левая грань
            0, 7, 3,  // Треугольник 9
            0, 4, 7,  // Треугольник 10

            // Правая грань
            1, 2, 6,  // Треугольник 11
            1, 6, 5   // Треугольник 12
            };

        mesh.vertices = vert;
        mesh.triangles = tri;
    }


    private void Sphere(Mesh mesh)
    {
        int numVertices = (latitudeSegmentsSphere + 1) * (longitudeSegmentsSphere + 1);
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[latitudeSegmentsSphere * longitudeSegmentsSphere * 6];

        float latitudeStep = 2 * Mathf.PI / latitudeSegmentsSphere;
        float longitudeStep = Mathf.PI / longitudeSegmentsSphere;
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int lat = 0; lat <= latitudeSegmentsSphere; lat++)
        {
            for (int lon = 0; lon <= longitudeSegmentsSphere; lon++)
            {
                float theta = lat * latitudeStep;
                float phi = lon * longitudeStep;

                float x = radiusSphere * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radiusSphere * Mathf.Cos(phi);
                float z = radiusSphere * Mathf.Sin(phi) * Mathf.Sin(theta);

                vertices[vertexIndex] = new Vector3(x, y, z);

                if (lat < latitudeSegmentsSphere && lon < longitudeSegmentsSphere)
                {
                    int currentRow = lat * (longitudeSegmentsSphere + 1);
                    int nextRow = (lat + 1) * (longitudeSegmentsSphere + 1);

                    triangles[triangleIndex] = currentRow + lon;
                    triangles[triangleIndex + 1] = nextRow + lon;
                    triangles[triangleIndex + 2] = currentRow + lon + 1;

                    triangles[triangleIndex + 3] = currentRow + lon + 1;
                    triangles[triangleIndex + 4] = nextRow + lon;
                    triangles[triangleIndex + 5] = nextRow + lon + 1;

                    triangleIndex += 6;
                }

                vertexIndex++;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    void Prism(Mesh mesh)
    {
        int numVertices = numSidesPrism * 2;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numSidesPrism * 6];

        float angleStep = 2 * Mathf.PI / numSidesPrism;
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int i = 0; i < numSidesPrism; i++)
        {
            float angle = i * angleStep;

            // Вершины основания призмы
            float x = widthPrism * Mathf.Cos(angle);
            float z = widthPrism * Mathf.Sin(angle);
            vertices[vertexIndex] = new Vector3(x, -heightPrism * 0.5f, z); // Нижняя вершина
            vertices[vertexIndex + 1] = new Vector3(x, heightPrism * 0.5f, z); // Верхняя вершина

            // Треугольники основания призмы
            if (i < numSidesPrism - 1)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + 3;

                triangles[triangleIndex + 3] = vertexIndex;
                triangles[triangleIndex + 4] = vertexIndex + 3;
                triangles[triangleIndex + 5] = vertexIndex + 2;

                triangleIndex += 6;
            }
            else // Последний треугольник
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = 1;

                triangles[triangleIndex + 3] = vertexIndex;
                triangles[triangleIndex + 4] = 1;
                triangles[triangleIndex + 5] = 0;

                triangleIndex += 6;
            }

            vertexIndex += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    void Capsule(Mesh mesh)
    {
        int numVertices = (CapsuleNumSegments + 1) * (CapsuleNumSegments + 1) + 2;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[CapsuleNumSegments * CapsuleNumSegments * 6 + CapsuleNumSegments * 3 * 2];

        float segmentHeight = CapsuleHeight - 2 * CapsuleRadius;
        float halfHeight = segmentHeight * 0.5f;

        int vertexIndex = 0;
        int triangleIndex = 0;

        // Верхние и нижние полусферы
        GenerateHalfSphere(vertices, triangles, ref vertexIndex, ref triangleIndex, Vector3.up, halfHeight);
        GenerateHalfSphere(vertices, triangles, ref vertexIndex, ref triangleIndex, Vector3.down, halfHeight);

        // Цилиндр
        GenerateCylinder(vertices, triangles, ref vertexIndex, ref triangleIndex, segmentHeight);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    // Генерация вершин и треугольников полусферы
    void GenerateHalfSphere(Vector3[] vertices, int[] triangles, ref int vertexIndex, ref int triangleIndex, Vector3 hemisphereAxis, float halfHeight)
    {
        float angleStep = 2f * Mathf.PI / CapsuleNumSegments;
        float verticalAngleStep = Mathf.PI / (CapsuleNumSegments + 1);
        int startIndex = vertexIndex;

        for (int lat = 0; lat <= CapsuleNumSegments; lat++)
        {
            float verticalAngle = Mathf.PI * 0.5f - (lat + 1) * verticalAngleStep;
            float radiusAtLatitude = CapsuleRadius * Mathf.Cos(verticalAngle);
            float yOffset = halfHeight + CapsuleRadius * Mathf.Sin(verticalAngle) * hemisphereAxis.y;

            for (int lon = 0; lon <= CapsuleNumSegments; lon++)
            {
                float angle = lon * angleStep;
                float x = radiusAtLatitude * Mathf.Cos(angle);
                float z = radiusAtLatitude * Mathf.Sin(angle);
                vertices[vertexIndex] = new Vector3(x, yOffset, z);
                vertexIndex++;

                if (lat < CapsuleNumSegments && lon < CapsuleNumSegments)
                {
                    int currentRow = lat * (CapsuleNumSegments + 1);
                    int nextRow = (lat + 1) * (CapsuleNumSegments + 1);

                    triangles[triangleIndex] = startIndex + currentRow + lon;
                    triangles[triangleIndex + 1] = startIndex + nextRow + lon;
                    triangles[triangleIndex + 2] = startIndex + currentRow + lon + 1;

                    triangles[triangleIndex + 3] = startIndex + currentRow + lon + 1;
                    triangles[triangleIndex + 4] = startIndex + nextRow + lon;
                    triangles[triangleIndex + 5] = startIndex + nextRow + lon + 1;

                    triangleIndex += 6;
                }
            }
        }
    }

    // Генерация вершин и треугольников цилиндра
    void GenerateCylinder(Vector3[] vertices, int[] triangles, ref int vertexIndex, ref int triangleIndex, float segmentHeight)
    {
        float angleStep = 2f * Mathf.PI / CapsuleNumSegments;
        int startIndex = vertexIndex;

        for (int lat = 0; lat <= CapsuleNumSegments; lat++)
        {
            for (int lon = 0; lon <= CapsuleNumSegments; lon++)
            {
                float angle = lon * angleStep;
                float x = CapsuleRadius * Mathf.Cos(angle);
                float z = CapsuleRadius * Mathf.Sin(angle);
                float yOffset = -(CapsuleHeight * 0.5f - CapsuleRadius - segmentHeight);

                vertices[vertexIndex] = new Vector3(x, yOffset, z);
                vertexIndex++;

                if (lat < CapsuleNumSegments && lon < CapsuleNumSegments)
                {
                    int currentRow = lat * (CapsuleNumSegments + 1);
                    int nextRow = (lat + 1) * (CapsuleNumSegments + 1);

                    triangles[triangleIndex] = startIndex + currentRow + lon;
                    triangles[triangleIndex + 1] = startIndex + nextRow + lon;
                    triangles[triangleIndex + 2] = startIndex + currentRow + lon + 1;

                    triangles[triangleIndex + 3] = startIndex + currentRow + lon + 1;
                    triangles[triangleIndex + 4] = startIndex + nextRow + lon;
                    triangles[triangleIndex + 5] = startIndex + nextRow + lon + 1;

                    triangleIndex += 6;
                }
            }
        }
    }
}