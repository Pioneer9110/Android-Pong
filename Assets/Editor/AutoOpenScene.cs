using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class AutoOpenScene
{
    static AutoOpenScene()
    {
        // Add a one-time callback to the next editor update
        EditorApplication.update += OpenSceneOnStartup;
    }

    private static void OpenSceneOnStartup()
    {
        EditorApplication.update -= OpenSceneOnStartup; // Remove self to avoid running every frame

        string scenePath = "Assets/Scenes/Main_Scene.unity"; // Change to your actual scene path

        if (EditorSceneManager.GetActiveScene().path != scenePath)
        {
            EditorSceneManager.OpenScene(scenePath);
            
        }
    }
}
