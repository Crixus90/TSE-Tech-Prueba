using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField, Range(1, 50)] private int cubeCount = 1;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Vector3 spawnRange = new Vector3(5, 2, 5);

    private List<GameObject> spawnedCubes = new List<GameObject>();
    private static readonly int _colorID = Shader.PropertyToID("_Color");

    public int CubeCount => spawnedCubes.Count;

    public void SpawnCubes()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("[CubeSpawner] Cube prefab is missing!");
            return;
        }

        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),
                Random.Range(1, spawnRange.y),
                Random.Range(-spawnRange.z, spawnRange.z)
            );

            GameObject cube = Instantiate(cubePrefab, randomPos, Quaternion.identity, transform);
            AssignRandomMaterial(cube);
            spawnedCubes.Add(cube);
        }

        Debug.Log($"[CubeSpawner] Spawned {cubeCount} cubes. Total: {spawnedCubes.Count}");
    }

    public void DeleteAllCubes()
    {
        foreach (var cube in spawnedCubes)
        {
            DestroyImmediate(cube);
        }
        spawnedCubes.Clear();
        Debug.Log("[CubeSpawner] All spawned cubes deleted.");
    }

    private void AssignRandomMaterial(GameObject cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            Material newMaterial = new Material(Shader.Find("Standard"));
            newMaterial.SetColor(_colorID, Random.ColorHSV());
            renderer.material = newMaterial;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize spawn area in inspector

        Gizmos.color = new Color(0, 1, 0, 0.3f); 
        Vector3 center = transform.position;
        Gizmos.DrawCube(center, spawnRange * 2); 

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, spawnRange * 2); 
    }
}
