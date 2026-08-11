using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text;
using System.Linq;

public static class CollegeReleaseValidator
{
    static int pass = 0;
    static int fail = 0;

    static StringBuilder report =
        new StringBuilder();

    static readonly string[] clues =
    {
        "Indice_1_Vie_Scolaire",
        "Indice_2_CDI",
        "Indice_3_Auditorium",
        "Indice_4_Gymnase",
        "Indice_5_Arts_Musique",
        "Indice_6_Self"
    };

    public static void Run()
    {
        pass = 0;
        fail = 0;
        report.Clear();

        Log("================================================");
        Log(" COLLEGE TREASURE 3D - VALIDATION RELEASE V1.0");
        Log("================================================");
        Log("");

        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        if (!File.Exists(scenePath))
        {
            Fail("Scene principale absente.");
            Finish();
            return;
        }

        EditorSceneManager.OpenScene(
            scenePath
        );

        Pass("Scene principale presente.");

        // =================================================
        // BUILD SETTINGS
        // =================================================

        bool sceneInBuild =
            EditorBuildSettings.scenes
            .Any(
                s =>
                s.enabled
                &&
                s.path == scenePath
            );

        Check(
            sceneInBuild,
            "Scene principale dans Build Settings."
        );

        // =================================================
        // JOUEUR
        // =================================================

        GameObject player =
            GameObject.Find("Player");

        Check(
            player != null,
            "Player present."
        );

        if (player != null)
        {
            Check(
                player.GetComponent<CharacterController>()
                != null,
                "CharacterController present."
            );

            Check(
                player.GetComponent<FPSController>()
                != null,
                "FPSController present."
            );

            Check(
                player.GetComponent<PlayerInteraction3D>()
                != null,
                "PlayerInteraction3D present."
            );

            Check(
                player.GetComponentInChildren<Camera>()
                != null,
                "Camera joueur presente."
            );
        }

        // =================================================
        // GAME MANAGER
        // =================================================

        TreasureGameManager game =
            Object.FindFirstObjectByType<
                TreasureGameManager
            >();

        Check(
            game != null,
            "TreasureGameManager present."
        );

        if (game != null)
        {
            Check(
                game.totalClues == 6,
                "Nombre total d'indices = 6."
            );
        }

        // =================================================
        // INDICES
        // =================================================

        for (
            int i = 0;
            i < clues.Length;
            i++
        )
        {
            GameObject obj =
                GameObject.Find(
                    clues[i]
                );

            Check(
                obj != null,
                "Indice "
                + (i + 1)
                + " present."
            );

            if (obj == null)
                continue;

            ClueObject clue =
                obj.GetComponent<
                    ClueObject
                >();

            Check(
                clue != null,
                "Indice "
                + (i + 1)
                + " : ClueObject present."
            );

            if (clue != null)
            {
                Check(
                    clue.order == i + 1,
                    "Indice "
                    + (i + 1)
                    + " : ordre correct."
                );

                Check(
                    obj.GetComponent<ClueGlowPulse>()
                    != null,
                    "Indice "
                    + (i + 1)
                    + " : animation/halo present."
                );
            }
        }

        // =================================================
        // TRESOR
        // =================================================

        GameObject treasure =
            GameObject.Find(
                "Tresor_Final"
            );

        Check(
            treasure != null,
            "Tresor final present."
        );

        if (treasure != null)
        {
            Check(
                treasure.GetComponent<
                    TreasureObject
                >() != null,
                "TreasureObject present."
            );
        }

        Check(
            Object.FindFirstObjectByType<
                TreasureCelebrationFX
            >() != null,
            "Effet de victoire present."
        );

        // =================================================
        // MUSIQUE ET SONS
        // =================================================

        GameObject musicObject =
            GameObject.Find(
                "BackgroundMusic"
            );

        Check(
            musicObject != null,
            "Objet BackgroundMusic present."
        );

        if (musicObject != null)
        {
            AudioSource source =
                musicObject.GetComponent<
                    AudioSource
                >();

            Check(
                source != null,
                "AudioSource musique present."
            );

            if (source != null)
            {
                Check(
                    source.clip != null,
                    "Fichier musique assigne."
                );

                Check(
                    source.loop,
                    "Musique configuree en boucle."
                );
            }
        }

        CheckFile(
            "Assets/Audio/college_adventure.wav",
            "Musique principale"
        );

        CheckFile(
            "Assets/Audio/fx_clue.wav",
            "Son indice"
        );

        CheckFile(
            "Assets/Audio/fx_footstep.wav",
            "Son pas"
        );

        CheckFile(
            "Assets/Audio/fx_door.wav",
            "Son porte"
        );

        CheckFile(
            "Assets/Audio/fx_victory.wav",
            "Son victoire"
        );

        Check(
            Object.FindFirstObjectByType<
                CollegeAudioFX
            >() != null,
            "CollegeAudioFX present."
        );

        DoorController[] doors =
            Object.FindObjectsByType<
                DoorController
            >(
                FindObjectsSortMode.None
            );

        DoorSoundFX[] doorSounds =
            Object.FindObjectsByType<
                DoorSoundFX
            >(
                FindObjectsSortMode.None
            );

        Check(
            doors.Length > 0,
            "Portes interactives presentes."
        );

        Check(
            doorSounds.Length
            == doors.Length,
            "Toutes les portes sont sonorisees."
        );

        // =================================================
        // INTERFACE
        // =================================================

        Check(
            Object.FindFirstObjectByType<
                MiniMapHUD
            >() != null,
            "Mini-carte presente."
        );

        Check(
            Object.FindFirstObjectByType<
                CollegeModernUI
            >() != null,
            "Interface moderne presente."
        );

        Check(
            Object.FindFirstObjectByType<
                CollegeDemoController
            >() != null,
            "Demo F2 presente."
        );

        Check(
            Object.FindFirstObjectByType<
                CollegeTrailerController
            >() != null,
            "Bande-annonce F3 presente."
        );

        // =================================================
        // QUITTER
        // =================================================

        string uiPath =
            "Assets/Scripts/CollegeModernUI.cs";

        if (File.Exists(uiPath))
        {
            string ui =
                File.ReadAllText(
                    uiPath
                );

            Check(
                ui.Contains(
                    "KeyCode.F10"
                ),
                "Raccourci F10 present."
            );

            Check(
                ui.Contains(
                    "Application.Quit"
                ),
                "Application.Quit present."
            );

            Check(
                ui.Contains(
                    "QUITTER LE JEU"
                ),
                "Bouton QUITTER LE JEU present."
            );
        }
        else
        {
            Fail(
                "CollegeModernUI.cs absent."
            );
        }

        // =================================================
        // NPC
        // =================================================

        GameObject npcRoot =
            GameObject.Find(
                "NPC_Step19"
            );

        Check(
            npcRoot != null,
            "Groupe NPC present."
        );

        if (npcRoot != null)
        {
            CollegeNPC[] npcs =
                npcRoot.GetComponentsInChildren<
                    CollegeNPC
                >(true);

            Collider[] npcColliders =
                npcRoot.GetComponentsInChildren<
                    Collider
                >(true);

            Check(
                npcs.Length >= 10,
                "Au moins 10 personnages presents."
            );

            Check(
                npcColliders.Length == 0,
                "NPC sans collider bloquant."
            );

            Log(
                "NPC detectes : "
                + npcs.Length
            );
        }

        // =================================================
        // DECORS
        // =================================================

        Check(
            GameObject.Find(
                "Decor_Step13"
            ) != null,
            "Decor exterieur Step13 present."
        );

        Check(
            GameObject.Find(
                "Decor_Step14_Interieurs"
            ) != null,
            "Decor interieur Step14 present."
        );

        GameObject architecture =
            GameObject.Find(
                "Decor_Step18_Architecture"
            );

        Check(
            architecture != null,
            "Architecture Step18 presente."
        );

        if (architecture != null)
        {
            Collider[] colliders =
                architecture
                .GetComponentsInChildren<
                    Collider
                >(true);

            Check(
                colliders.Length == 0,
                "Decor Step18 sans collider."
            );
        }

        // =================================================
        // TEXTURES
        // =================================================

        CheckDirectory(
            "Assets/Textures/ambientCG/Concrete010",
            "Texture beton"
        );

        CheckDirectory(
            "Assets/Textures/ambientCG/WoodFloor041",
            "Texture bois"
        );

        CheckDirectory(
            "Assets/Textures/ambientCG/Tiles040",
            "Texture carrelage"
        );

        CheckDirectory(
            "Assets/Textures/ambientCG/Grass008",
            "Texture herbe"
        );

        CheckDirectory(
            "Assets/Textures/ambientCG/Metal036",
            "Texture metal"
        );

        // =================================================
        // BUILD
        // =================================================

        CheckFile(
            "Assets/Editor/CollegeDesktopBuild.cs",
            "Script build multiplateforme"
        );

        CheckFile(
            "build-mac.command",
            "Script build macOS"
        );

        // =================================================
        // PERFORMANCE
        // =================================================

        Check(
            Object.FindFirstObjectByType<
                PlatformPerformanceProfile
            >() != null,
            "Profil performance multiplateforme present."
        );

        Finish();
    }

    static void Check(
        bool condition,
        string text)
    {
        if (condition)
            Pass(text);
        else
            Fail(text);
    }

    static void CheckFile(
        string path,
        string name)
    {
        Check(
            File.Exists(path),
            name + " present."
        );
    }

    static void CheckDirectory(
        string path,
        string name)
    {
        bool ok =
            Directory.Exists(path)
            &&
            Directory.GetFiles(
                path,
                "*",
                SearchOption.AllDirectories
            ).Length > 0;

        Check(
            ok,
            name + " presente."
        );
    }

    static void Pass(
        string text)
    {
        pass++;

        string line =
            "[PASS] " + text;

        Debug.Log(line);
        report.AppendLine(line);
    }

    static void Fail(
        string text)
    {
        fail++;

        string line =
            "[FAIL] " + text;

        Debug.LogError(line);
        report.AppendLine(line);
    }

    static void Log(
        string text)
    {
        Debug.Log(text);
        report.AppendLine(text);
    }

    static void Finish()
    {
        Log("");
        Log("================================================");
        Log(" RESULTAT RELEASE V1.0");
        Log("================================================");

        Log(
            "PASS : " + pass
        );

        Log(
            "FAIL : " + fail
        );

        Log("");

        if (fail == 0)
        {
            Log(
                ">>> RELEASE V1.0 VALIDEE <<<"
            );
        }
        else
        {
            Log(
                ">>> RELEASE V1.0 NON VALIDEE <<<"
            );
        }

        Directory.CreateDirectory(
            "Logs"
        );

        File.WriteAllText(
            "Logs/release-v1-tests.txt",
            report.ToString()
        );
    }
}
