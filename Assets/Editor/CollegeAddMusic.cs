using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeAddMusic
{
    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        string musicPath =
            "Assets/Audio/college_adventure.wav";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        AssetDatabase.Refresh();

        AudioClip music =
            AssetDatabase.LoadAssetAtPath<AudioClip>(
                musicPath
            );

        if (music == null)
        {
            Debug.LogError(
                "Impossible de charger la musique : "
                + musicPath
            );

            return;
        }

        GameObject old =
            GameObject.Find(
                "BackgroundMusic"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        GameObject musicObject =
            new GameObject(
                "BackgroundMusic"
            );

        AudioSource source =
            musicObject.AddComponent<AudioSource>();

        source.clip = music;

        // Musique en boucle
        source.loop = true;

        // Démarrage automatique
        source.playOnAwake = true;

        // Musique globale, pas spatialisée
        source.spatialBlend = 0f;

        // Volume raisonnable
        source.volume = 0.22f;

        source.priority = 128;

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "MUSIQUE AJOUTEE AU JEU"
        );
    }
}
