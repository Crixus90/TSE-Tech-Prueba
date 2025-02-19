using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class SceneManagerWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private SceneAsset draggedScene;

    [MenuItem("Tools/Scene Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneManagerWindow>("Scene Manager");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scenes in Build Settings", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        var scenes = EditorBuildSettings.scenes
            .Where(scene => !string.IsNullOrEmpty(scene.path))
            .Select(scene => AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path))
            .Where(scene => scene != null)
            .ToList();

        foreach (var scene in scenes)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(scene.name, GUILayout.Width(200));

            if (GUILayout.Button("Load", GUILayout.Width(60)))
            {
                OpenScene(scene);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        if (GUILayout.Button("Reload Current Scene"))
        {
            ReloadScene();
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Drag a Scene to Add to Build Settings", EditorStyles.boldLabel);
        var rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "Drop Scene Here", EditorStyles.helpBox);
        HandleDragAndDrop(rect);
    }

    private void OpenScene(SceneAsset scene)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            string scenePath = AssetDatabase.GetAssetPath(scene);
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    private void ReloadScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            string scenePath = EditorSceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        if (!dropArea.Contains(evt.mousePosition)) return;

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var draggedObj in DragAndDrop.objectReferences)
                    {
                        if (draggedObj is SceneAsset scene)
                        {
                            AddSceneToBuildSettings(scene);
                        }
                    }
                }
                Event.current.Use();
                break;
        }
    }

    private void AddSceneToBuildSettings(SceneAsset scene)
    {
        string scenePath = AssetDatabase.GetAssetPath(scene);
        if (EditorBuildSettings.scenes.Any(s => s.path == scenePath)) return;

        var scenes = EditorBuildSettings.scenes.ToList();
        scenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log($"Added scene to Build Settings: {scene.name}");
    }
}
