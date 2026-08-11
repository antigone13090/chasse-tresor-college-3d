using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class CollegeGameValidator
{
    static int passed = 0;
    static int failed = 0;

    static StringBuilder report =
        new StringBuilder();

    static readonly string[] clueNames =
    {
        "Indice_1_Vie_Scolaire",
        "Indice_2_CDI",
        "Indice_3_Auditorium",
        "Indice_4_Gymnase",
        "Indice_5_Arts_Musique",
        "Indice_6_Self"
    };

    static readonly string[] expectedTitles =
    {
        "Indice 1 - Vie scolaire",
        "Indice 2 - CDI",
        "Indice 3 - Auditorium",
        "Indice 4 - Gymnase",
        "Indice 5 - Arts et Musique",
        "Indice 6 - Self élèves"
    };

    [MenuItem("College Treasure/Tester tous les indices")]
    public static void Run()
    {
        passed = 0;
        failed = 0;
        report.Clear();

        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        Log("");
        Log("==============================================");
        Log(" COLLEGE TREASURE 3D");
        Log(" TEST AUTOMATIQUE DES INDICES");
        Log("==============================================");
        Log("");

        // --------------------------------------------------
        // SCENE
        // --------------------------------------------------

        if (!File.Exists(scenePath))
        {
            Fail("Scene CollegePrototype introuvable.");
            Finish();
            return;
        }

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Pass("Scene CollegePrototype chargee.");

        // --------------------------------------------------
        // GAME MANAGER
        // --------------------------------------------------

        TreasureGameManager manager =
            UnityEngine.Object.FindFirstObjectByType<
                TreasureGameManager
            >();

        if (manager == null)
        {
            Fail("TreasureGameManager absent.");
            Finish();
            return;
        }

        Pass("TreasureGameManager present.");

        TreasureGameManager.Instance =
            manager;

        // --------------------------------------------------
        // JOUEUR
        // --------------------------------------------------

        GameObject player =
            GameObject.Find("Player");

        if (player == null)
        {
            Fail("Player absent.");
        }
        else
        {
            Pass("Player present.");
        }

        PlayerInteraction3D interaction =
            player != null
            ? player.GetComponent<PlayerInteraction3D>()
            : null;

        if (interaction == null)
        {
            Fail(
                "PlayerInteraction3D absent sur Player."
            );
        }
        else
        {
            Pass(
                "Systeme d'interaction E present."
            );
        }

        // --------------------------------------------------
        // CHARGEMENT DES 6 INDICES
        // --------------------------------------------------

        List<ClueObject> clues =
            new List<ClueObject>();

        for (int i = 0; i < clueNames.Length; i++)
        {
            GameObject obj =
                GameObject.Find(clueNames[i]);

            Log("");
            Log(
                "----------------------------------------------"
            );

            Log(
                "TEST INDICE " + (i + 1)
            );

            if (obj == null)
            {
                Fail(
                    clueNames[i]
                    + " : objet introuvable."
                );

                clues.Add(null);
                continue;
            }

            Pass(
                clueNames[i]
                + " : objet present."
            );

            ClueObject clue =
                obj.GetComponent<ClueObject>();

            clues.Add(clue);

            if (clue == null)
            {
                Fail(
                    clueNames[i]
                    + " : composant ClueObject absent."
                );

                continue;
            }

            Pass("Composant ClueObject present.");

            // Ordre
            if (clue.order == i + 1)
            {
                Pass(
                    "Ordre correct : "
                    + clue.order
                );
            }
            else
            {
                Fail(
                    "Ordre incorrect : "
                    + clue.order
                    + " au lieu de "
                    + (i + 1)
                );
            }

            // Titre
            if (
                clue.clueTitle
                == expectedTitles[i]
            )
            {
                Pass("Titre correct.");
            }
            else
            {
                Fail(
                    "Titre inattendu : "
                    + clue.clueTitle
                );
            }

            // Collider
            Collider collider =
                obj.GetComponent<Collider>();

            if (
                collider != null
                && collider.enabled
            )
            {
                Pass(
                    "Collider actif."
                );
            }
            else
            {
                Fail(
                    "Collider absent ou desactive."
                );
            }

            // Renderer
            Renderer renderer =
                obj.GetComponent<Renderer>();

            if (
                renderer != null
                && renderer.enabled
            )
            {
                Pass(
                    "Indice visible."
                );
            }
            else
            {
                Fail(
                    "Renderer absent ou desactive."
                );
            }

            // Position
            Log(
                "Position Unity : "
                + obj.transform.position
            );

            // ------------------------------------------------
            // TEST DETECTION A PROXIMITE
            // ------------------------------------------------

            if (
                player != null
                && interaction != null
                && collider != null
            )
            {
                Vector3 oldPlayerPosition =
                    player.transform.position;

                bool controllerState = false;

                CharacterController cc =
                    player.GetComponent<
                        CharacterController
                    >();

                if (cc != null)
                {
                    controllerState =
                        cc.enabled;

                    cc.enabled = false;
                }

                player.transform.position =
                    obj.transform.position
                    + new Vector3(
                        0f,
                        -0.7f,
                        -1.3f
                    );

                Physics.SyncTransforms();

                Collider[] detected =
                    Physics.OverlapSphere(
                        player.transform.position,
                        interaction.cluePickupDistance
                    );

                bool found = false;

                foreach (
                    Collider detectedCollider
                    in detected
                )
                {
                    ClueObject detectedClue =
                        detectedCollider
                        .GetComponentInParent<
                            ClueObject
                        >();

                    if (detectedClue == clue)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    Pass(
                        "Detectable avec E a proximite."
                    );
                }
                else
                {
                    Fail(
                        "Non detectable dans la zone "
                        + "d'interaction E."
                    );
                }

                player.transform.position =
                    oldPlayerPosition;

                if (cc != null)
                    cc.enabled =
                        controllerState;

                Physics.SyncTransforms();
            }

            // ------------------------------------------------
            // TEST : REFUS SI MAUVAIS ORDRE
            // ------------------------------------------------

            if (i > 0)
            {
                SaveState state =
                    Save(clue);

                manager.foundClues =
                    i - 1;

                clue.collected = false;

                Enable(clue);

                int before =
                    manager.foundClues;

                clue.Interact();

                if (
                    manager.foundClues == before
                    &&
                    !clue.collected
                )
                {
                    Pass(
                        "Refus correct si indice "
                        + "pris trop tot."
                    );
                }
                else
                {
                    Fail(
                        "L'indice peut etre pris "
                        + "dans le mauvais ordre."
                    );
                }

                Restore(clue, state);
            }

            // ------------------------------------------------
            // TEST : RECUPERATION CORRECTE
            // ------------------------------------------------

            {
                SaveState state =
                    Save(clue);

                manager.foundClues = i;

                clue.collected = false;

                Enable(clue);

                clue.Interact();

                if (
                    manager.foundClues
                    == i + 1
                    &&
                    clue.collected
                )
                {
                    Pass(
                        "Recuperation fonctionnelle."
                    );
                }
                else
                {
                    Fail(
                        "Echec de recuperation."
                    );
                }

                Restore(clue, state);
            }
        }

        // ==================================================
        // TEST SEQUENCE COMPLETE 1 -> 6
        // ==================================================

        Log("");
        Log("==============================================");
        Log(" TEST SEQUENCE COMPLETE 1 -> 6");
        Log("==============================================");

        bool allCluesExist = true;

        foreach (ClueObject clue in clues)
        {
            if (clue == null)
                allCluesExist = false;
        }

        if (allCluesExist)
        {
            List<SaveState> states =
                new List<SaveState>();

            foreach (ClueObject clue in clues)
            {
                states.Add(
                    Save(clue)
                );

                clue.collected = false;
                Enable(clue);
            }

            manager.foundClues = 0;
            manager.victory = false;

            bool sequenceOK = true;

            for (
                int i = 0;
                i < clues.Count;
                i++
            )
            {
                clues[i].Interact();

                if (
                    manager.foundClues
                    != i + 1
                )
                {
                    sequenceOK = false;

                    Fail(
                        "Sequence bloquee a l'indice "
                        + (i + 1)
                    );

                    break;
                }

                Log(
                    "Indice "
                    + (i + 1)
                    + " OK -> compteur "
                    + manager.foundClues
                    + "/6"
                );
            }

            if (
                sequenceOK
                &&
                manager.foundClues == 6
            )
            {
                Pass(
                    "Sequence complete 1 -> 6 fonctionnelle."
                );
            }

            for (
                int i = 0;
                i < clues.Count;
                i++
            )
            {
                Restore(
                    clues[i],
                    states[i]
                );
            }
        }
        else
        {
            Fail(
                "Impossible de tester la sequence : "
                + "un ou plusieurs indices manquent."
            );
        }

        // ==================================================
        // TEST TRESOR FINAL
        // ==================================================

        Log("");
        Log("==============================================");
        Log(" TEST TRESOR FINAL");
        Log("==============================================");

        GameObject treasureObject =
            GameObject.Find("Tresor_Final");

        if (treasureObject == null)
        {
            Fail("Tresor_Final introuvable.");
        }
        else
        {
            Pass("Tresor_Final present.");

            TreasureObject treasure =
                treasureObject
                .GetComponent<TreasureObject>();

            if (treasure != null)
            {
                Pass(
                    "Composant TreasureObject present."
                );
            }
            else
            {
                Fail(
                    "TreasureObject absent."
                );
            }
        }

        Finish();
    }

    // ======================================================
    // SAUVEGARDE ET RESTAURATION DES ETATS DES INDICES
    // ======================================================

    class SaveState
    {
        public bool collected;
        public bool[] colliders;
        public bool[] renderers;
    }

    static SaveState Save(
        ClueObject clue)
    {
        SaveState state =
            new SaveState();

        state.collected =
            clue.collected;

        Collider[] cols =
            clue.GetComponentsInChildren<
                Collider
            >(true);

        Renderer[] rends =
            clue.GetComponentsInChildren<
                Renderer
            >(true);

        state.colliders =
            new bool[cols.Length];

        state.renderers =
            new bool[rends.Length];

        for (
            int i = 0;
            i < cols.Length;
            i++
        )
        {
            state.colliders[i] =
                cols[i].enabled;
        }

        for (
            int i = 0;
            i < rends.Length;
            i++
        )
        {
            state.renderers[i] =
                rends[i].enabled;
        }

        return state;
    }

    static void Restore(
        ClueObject clue,
        SaveState state)
    {
        clue.collected =
            state.collected;

        Collider[] cols =
            clue.GetComponentsInChildren<
                Collider
            >(true);

        Renderer[] rends =
            clue.GetComponentsInChildren<
                Renderer
            >(true);

        for (
            int i = 0;
            i < cols.Length
            && i < state.colliders.Length;
            i++
        )
        {
            cols[i].enabled =
                state.colliders[i];
        }

        for (
            int i = 0;
            i < rends.Length
            && i < state.renderers.Length;
            i++
        )
        {
            rends[i].enabled =
                state.renderers[i];
        }
    }

    static void Enable(
        ClueObject clue)
    {
        foreach (
            Collider col
            in clue.GetComponentsInChildren<
                Collider
            >(true))
        {
            col.enabled = true;
        }

        foreach (
            Renderer renderer
            in clue.GetComponentsInChildren<
                Renderer
            >(true))
        {
            renderer.enabled = true;
        }
    }

    // ======================================================
    // RAPPORT
    // ======================================================

    static void Pass(string text)
    {
        passed++;

        string line =
            "[PASS] " + text;

        Debug.Log(line);
        report.AppendLine(line);
    }

    static void Fail(string text)
    {
        failed++;

        string line =
            "[FAIL] " + text;

        Debug.LogError(line);
        report.AppendLine(line);
    }

    static void Log(string text)
    {
        Debug.Log(text);
        report.AppendLine(text);
    }

    static void Finish()
    {
        Log("");
        Log("==============================================");
        Log(" RESULTAT FINAL");
        Log("==============================================");
        Log("PASS : " + passed);
        Log("FAIL : " + failed);

        if (failed == 0)
        {
            Log("");
            Log(
                ">>> TOUS LES TESTS SONT VALIDES <<<"
            );
        }
        else
        {
            Log("");
            Log(
                ">>> CERTAINS TESTS ONT ECHOUE <<<"
            );
        }

        string logDirectory =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Logs"
            );

        Directory.CreateDirectory(
            logDirectory
        );

        string output =
            Path.Combine(
                logDirectory,
                "indice-tests.txt"
            );

        File.WriteAllText(
            output,
            report.ToString()
        );

        Debug.Log(
            "Rapport sauvegarde : "
            + output
        );
    }
}
