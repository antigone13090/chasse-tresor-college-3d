using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CollegeTreasure.Campaign;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CampaignV2Validator
{
    const string SceneFolder = "Assets/CampaignV2/Scenes";
    static int pass;
    static int fail;
    static readonly StringBuilder Report = new StringBuilder();

    [MenuItem("College Treasure/V2/Valider campagne complete")]
    public static void Run()
    {
        pass = 0;
        fail = 0;
        Report.Clear();
        Log("================================================");
        Log(" CAMPAGNE V2 - VALIDATION DES SIX NIVEAUX");
        Log("================================================");

        ValidateDefinitions();
        ValidateScenes();
        ValidateBuildSettings();
        ValidateRules();
        ValidateSaveRoundTrip();
        ValidateCompletePlaythroughs();
        ValidateRuntimeConstruction();
        ValidateMetas();
        Finish();
    }

    static void ValidateDefinitions()
    {
        CampaignCatalog.ClearCacheForEditor();
        CampaignLevelDefinition[] levels = CampaignCatalog.All;
        Check(levels.Length == 7, "Catalogue : 1 chapitre original + 6 nouveaux niveaux.");
        Check(levels.Count(level => !level.IsOriginalAdventure) == 6, "Six definitions de niveaux supplementaires.");
        Check(levels.Select(level => level.levelId).Distinct().Count() == levels.Length, "Identifiants de niveau uniques.");
        Check(levels.Select(level => level.chapterNumber).SequenceEqual(Enumerable.Range(1, 7)), "Chapitres ordonnes de 1 a 7.");

        foreach (CampaignLevelDefinition level in levels)
        {
            string error;
            Check(level.IsValid(out error), "Definition valide : " + level.levelId + (string.IsNullOrEmpty(error) ? string.Empty : " (" + error + ")"));
            Check(level.IsOriginalAdventure || level.hints.Length >= 3, "Aide progressive disponible : " + level.levelId);
            Check(level.IsOriginalAdventure || !string.IsNullOrWhiteSpace(level.completionMessage), "Condition de fin documentee : " + level.levelId);
        }

        CampaignLevelDefinition original = levels.FirstOrDefault(level => level.IsOriginalAdventure);
        Check(original != null && original.sceneName == "CollegePrototype", "L'aventure originale pointe toujours vers CollegePrototype.");
    }

    static void ValidateScenes()
    {
        Check(File.Exists(SceneFolder + "/CampaignMenu.unity"), "Scene du menu de campagne presente.");
        Check(File.Exists("Assets/Scenes/CollegePrototype.unity"), "Scene originale preservee.");

        foreach (CampaignLevelDefinition level in CampaignCatalog.All.Where(item => !item.IsOriginalAdventure))
        {
            string path = SceneFolder + "/" + level.sceneName + ".unity";
            Check(File.Exists(path), "Scene distincte presente : " + level.sceneName);
            if (!File.Exists(path)) continue;

            EditorSceneManager.OpenScene(path);
            CampaignLevelBootstrap bootstrap = UnityEngine.Object.FindFirstObjectByType<CampaignLevelBootstrap>();
            Check(bootstrap != null, "Bootstrap present : " + level.sceneName);
            Check(bootstrap != null && bootstrap.definition == level, "Definition liee a la scene : " + level.sceneName);
        }

        EditorSceneManager.OpenScene("Assets/Scenes/CollegePrototype.unity");
        Check(UnityEngine.Object.FindFirstObjectByType<CampaignLevelBootstrap>() == null,
            "Aucun bootstrap V2 injecte dans l'aventure originale.");
        Check(UnityEngine.Object.FindFirstObjectByType<TreasureGameManager>() != null,
            "TreasureGameManager original toujours present.");
    }

    static void ValidateBuildSettings()
    {
        string[] paths = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        Check(paths.Length == 8, "Build Settings : menu, aventure originale et six niveaux.");
        Check(paths.Length > 0 && paths[0] == SceneFolder + "/CampaignMenu.unity", "Menu de campagne en premiere scene.");
        Check(paths.Contains("Assets/Scenes/CollegePrototype.unity"), "Aventure originale incluse au build.");

        foreach (CampaignLevelDefinition level in CampaignCatalog.All.Where(item => !item.IsOriginalAdventure))
            Check(paths.Contains(SceneFolder + "/" + level.sceneName + ".unity"), "Niveau inclus au build : " + level.levelId);
    }

    static void ValidateRules()
    {
        Check(CampaignPuzzleRules.SequenceEquals(new[] { 4, 1, 7 }, new[] { 4, 1, 7 }), "CDI : code correct accepte.");
        Check(!CampaignPuzzleRules.SequenceEquals(new[] { 4, 7, 1 }, new[] { 4, 1, 7 }), "CDI : code incorrect refuse.");
        Check(CampaignPuzzleRules.SwitchesMatch(new[] { true, false, true, true }, new[] { true, false, true, true }), "Laboratoire : combinaison unique acceptee.");
        Check(!CampaignPuzzleRules.SwitchesMatch(new[] { true, true, true, true }, new[] { true, false, true, true }), "Laboratoire : mauvaise combinaison refusee.");

        int[] audioSequence = { 2, 0, 3, 1 };
        Check(CampaignPuzzleRules.AcceptSequenceStep(audioSequence, 0, 2) == SequenceStepResult.Accepted, "Auditorium : premier signal accepte.");
        Check(CampaignPuzzleRules.AcceptSequenceStep(audioSequence, 3, 1) == SequenceStepResult.Completed, "Auditorium : dernier signal termine la sequence.");
        Check(CampaignPuzzleRules.AcceptSequenceStep(audioSequence, 1, 3) == SequenceStepResult.Incorrect, "Auditorium : erreur detectee sans ambiguite.");

        for (int i = 0; i < 5; i++)
            Check(CampaignPuzzleRules.IsExpectedOrderedTarget(i, i), "Gymnase : balise " + (i + 1) + " acceptee dans l'ordre.");
        Check(!CampaignPuzzleRules.IsExpectedOrderedTarget(2, 4), "Gymnase : balise hors ordre refusee sans perte.");

        int[] patterns = { 1, 3, 2 };
        for (int stage = 0; stage < patterns.Length; stage++)
            Check(CampaignPuzzleRules.IsCorrectPatternChoice(stage, patterns[stage], patterns), "Arts : motif correct a l'etape " + (stage + 1) + ".");
        Check(!CampaignPuzzleRules.IsCorrectPatternChoice(1, 0, patterns), "Arts : motif incorrect detecte.");

        int[] finalSequence = { 1, 3, 0, 2 };
        Check(CampaignPuzzleRules.AcceptSequenceStep(finalSequence, 3, 2) == SequenceStepResult.Completed, "College secret : phase sequence resoluble.");
        Check(CampaignPuzzleRules.SwitchesMatch(new[] { true, true, false }, new[] { true, true, false }), "College secret : phase logique resoluble.");
    }

    static void ValidateSaveRoundTrip()
    {
        CampaignSaveData source = new CampaignSaveData
        {
            unlockedThroughChapter = 5,
            lastPlayedLevelId = "gymnase-orientation"
        };
        source.completedLevelIds.Add("cdi-codes");
        source.checkpoints.Add(new CampaignCheckpoint { levelId = "arts-motifs", step = 2, state = "2|0" });
        string json = CampaignProgressService.SerializeForTests(source);
        CampaignSaveData restored = CampaignProgressService.DeserializeForTests(json);
        Check(restored.unlockedThroughChapter == 5, "Sauvegarde : chapitre debloque restaure.");
        Check(restored.completedLevelIds.SequenceEqual(source.completedLevelIds), "Sauvegarde : niveaux termines restaures.");
        Check(restored.checkpoints.Count == 1 && restored.checkpoints[0].state == "2|0", "Sauvegarde : checkpoint multi-etape restaure.");
    }

    static void ValidateRuntimeConstruction()
    {
        foreach (CampaignLevelDefinition level in CampaignCatalog.All.Where(item => !item.IsOriginalAdventure))
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject root = new GameObject("RuntimeValidation");
            CampaignLevelController controller = root.AddComponent<CampaignLevelController>();
            controller.Initialize(level);
            CampaignActionObject[] actions = UnityEngine.Object.FindObjectsByType<CampaignActionObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            CharacterController player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            Collider[] colliders = UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Check(player != null, "Construction runtime : joueur 3D present dans " + level.levelId);
            Check(actions.Length >= 5, "Construction runtime : interactions jouables dans " + level.levelId + " (" + actions.Length + ").");
            Check(colliders.Length > actions.Length, "Construction runtime : environnement physique dans " + level.levelId + ".");
            Time.timeScale = 1f;
        }
    }

    static void ValidateCompletePlaythroughs()
    {
        CampaignProgressService.BeginEditorTestSession();
        try
        {
            ValidateCdiPlaythroughWithResume();
            ValidateLabPlaythrough();
            ValidateAuditoriumPlaythrough();
            ValidateGymPlaythrough();
            ValidateArtsPlaythrough();
            ValidateSecretPlaythrough();
        }
        finally
        {
            Time.timeScale = 1f;
            CampaignProgressService.EndEditorTestSession();
        }
    }

    static void ValidateCdiPlaythroughWithResume()
    {
        CampaignLevelController first = BuildForPlaythrough("cdi-codes");
        Dictionary<string, CampaignActionObject> actions = Actions();
        Use(actions, "fragment-0");
        Use(actions, "fragment-1");
        Check(CampaignProgressService.GetCheckpoint("cdi-codes") != null,
            "CDI : checkpoint cree apres exploration partielle.");

        CampaignLevelController resumed = BuildForPlaythrough("cdi-codes");
        actions = Actions();
        Check(!actions["fragment-0"].InteractionEnabled && !actions["fragment-1"].InteractionEnabled,
            "CDI : fragments restaures apres reprise.");
        Use(actions, "fragment-2");
        Use(actions, "digit-9");
        Use(actions, "digit-4");
        Use(actions, "digit-1");
        Use(actions, "digit-7");
        Use(actions, "exit");
        Check(resumed.IsCompleted, "CDI : parcours complet jouable apres erreur et reprise.");
    }

    static void ValidateLabPlaythrough()
    {
        CampaignLevelController controller = BuildForPlaythrough("laboratoire-logique");
        Dictionary<string, CampaignActionObject> actions = Actions();
        Use(actions, "submit");
        Use(actions, "switch-0");
        Use(actions, "switch-2");
        Use(actions, "switch-3");
        Use(actions, "submit");
        Use(actions, "exit");
        Check(controller.IsCompleted, "Laboratoire : parcours complet jouable apres une combinaison fausse.");
    }

    static void ValidateAuditoriumPlaythrough()
    {
        CampaignLevelController controller = BuildForPlaythrough("auditorium-sequence");
        Dictionary<string, CampaignActionObject> actions = Actions();
        Use(actions, "pad-1");
        Use(actions, "pad-2");
        Use(actions, "pad-0");
        Use(actions, "pad-3");
        Use(actions, "pad-1");
        Use(actions, "exit");
        Check(controller.IsCompleted, "Auditorium : sequence complete jouable apres erreur.");
    }

    static void ValidateGymPlaythrough()
    {
        CampaignLevelController controller = BuildForPlaythrough("gymnase-orientation");
        Dictionary<string, CampaignActionObject> actions = Actions();
        Use(actions, "beacon-4");
        for (int i = 0; i < 5; i++) Use(actions, "beacon-" + i);
        Use(actions, "exit");
        Check(controller.IsCompleted, "Gymnase : parcours complet sans blocage apres balise hors ordre.");
    }

    static void ValidateArtsPlaythrough()
    {
        CampaignLevelController controller = BuildForPlaythrough("arts-motifs");
        Dictionary<string, CampaignActionObject> actions = Actions();
        Use(actions, "pattern-0");
        Use(actions, "pattern-1");
        Use(actions, "pattern-3");
        Use(actions, "pattern-2");
        Use(actions, "exit");
        Check(controller.IsCompleted, "Arts : enigme multi-etapes jouable apres mauvais motif.");
    }

    static void ValidateSecretPlaythrough()
    {
        CampaignLevelController controller = BuildForPlaythrough("college-secret");
        Dictionary<string, CampaignActionObject> actions = Actions();
        for (int i = 0; i < 3; i++) Use(actions, "final-fragment-" + i);
        Use(actions, "final-digit-9");
        Use(actions, "final-digit-7");
        Use(actions, "final-digit-3");
        Use(actions, "final-digit-1");
        Use(actions, "final-switch-0");
        Use(actions, "final-submit");
        Use(actions, "final-switch-1");
        Use(actions, "final-submit");
        Use(actions, "final-sequence-3");
        Use(actions, "final-sequence-1");
        Use(actions, "final-sequence-3");
        Use(actions, "final-sequence-0");
        Use(actions, "final-sequence-2");
        Use(actions, "new-treasure");
        Check(controller.IsCompleted, "College secret : cinq phases et nouveau tresor jouables apres erreurs.");
    }

    static CampaignLevelController BuildForPlaythrough(string levelId)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("AutomatedPlaythrough_" + levelId);
        CampaignLevelController controller = root.AddComponent<CampaignLevelController>();
        controller.Initialize(CampaignCatalog.FindById(levelId));
        return controller;
    }

    static Dictionary<string, CampaignActionObject> Actions()
    {
        return UnityEngine.Object.FindObjectsByType<CampaignActionObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None)
            .ToDictionary(action => action.ActionId, action => action);
    }

    static void Use(Dictionary<string, CampaignActionObject> actions, string actionId)
    {
        CampaignActionObject action;
        if (!actions.TryGetValue(actionId, out action))
        {
            FailDirect("Interaction absente pendant le parcours : " + actionId);
            return;
        }

        action.Interact(null);
    }

    static void ValidateMetas()
    {
        string[] extensions = { "*.cs", "*.unity", "*.asset" };
        foreach (string extension in extensions)
        {
            foreach (string path in Directory.GetFiles("Assets/CampaignV2", extension, SearchOption.AllDirectories))
                Check(File.Exists(path + ".meta"), "Meta present : " + path);
        }
    }

    static void Check(bool condition, string text)
    {
        if (condition)
        {
            pass++;
            Log("[PASS] " + text);
        }
        else
        {
            fail++;
            Log("[FAIL] " + text);
            Debug.LogError("[FAIL] " + text);
        }
    }

    static void FailDirect(string text)
    {
        fail++;
        Log("[FAIL] " + text);
        Debug.LogError("[FAIL] " + text);
    }

    static void Log(string text)
    {
        Debug.Log(text);
        Report.AppendLine(text);
    }

    static void Finish()
    {
        Log(string.Empty);
        Log("PASS : " + pass);
        Log("FAIL : " + fail);
        Directory.CreateDirectory("Logs");
        File.WriteAllText("Logs/campaign-v2-tests.txt", Report.ToString());
        AssetDatabase.SaveAssets();

        if (fail > 0)
            throw new InvalidOperationException("Validation V2 echouee : " + fail + " erreur(s).");

        Log(">>> CAMPAGNE V2 VALIDEE <<<");
    }
}
