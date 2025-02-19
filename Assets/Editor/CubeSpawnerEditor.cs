using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CubeSpawner spawner = (CubeSpawner)target;

        DrawDefaultInspector(); 

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cube Spawner Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Spawn Cubes"))
        {
            spawner.SpawnCubes();
        }

        if (GUILayout.Button("Delete All Cubes"))
        {
            spawner.DeleteAllCubes();
        }

        EditorGUILayout.HelpBox($"Cubes Spawned: {spawner.CubeCount}", MessageType.Info);
    }
}
