using CollegeTreasure.Campaign;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CampaignV2Generator
{
    const string DefinitionFolder = "Assets/Resources/CampaignLevels";
    const string SceneFolder = "Assets/CampaignV2/Scenes";

    [MenuItem("College Treasure/V2/Generer campagne et niveaux")]
    public static void Generate()
    {
        EnsureFolder("Assets", "Resources");
        EnsureFolder("Assets/Resources", "CampaignLevels");
        EnsureFolder("Assets/CampaignV2", "Scenes");

        AudioClip ambience = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/college_adventure.wav");
        AudioClip interaction = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/fx_clue.wav");
        AudioClip success = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/fx_victory.wav");

        CampaignLevelDefinition original = Upsert(
            "01_OriginalAdventure", "original-adventure", 1, "CollegePrototype",
            "L'aventure originale", CampaignPuzzleKind.OriginalAdventure,
            "Le parcours historique de six indices, conserve sans modification.",
            "Trouve les six indices dans leur ordre puis ouvre le tresor original.",
            "L'aventure originale reste disponible independamment de la campagne V2.",
            new string[0], new int[0], new bool[0], false,
            new Color(0.12f, 0.38f, 0.62f), new Color(0.95f, 0.64f, 0.12f),
            ambience, interaction, success);

        CampaignLevelDefinition cdi = Upsert(
            "02_CDI_Codes", "cdi-codes", 2, "V2_01_CDI_Codes",
            "CDI, codes caches", CampaignPuzzleKind.HiddenCodes,
            "Explore un CDI autonome, retrouve trois fragments dissimules et ouvre sa porte codee.",
            "Retrouve les trois fragments de code caches dans le CDI.",
            "La porte du CDI est ouverte : premier niveau supplementaire termine.",
            new[]
            {
                "Inspecte les rayonnages sur les deux cotes de la salle.",
                "Chaque fragment porte une lettre : lis-les dans l'ordre A, B, C.",
                "La combinaison complete est 4, 1, 7. Une erreur ne supprime pas les fragments."
            },
            new[] { 4, 1, 7 }, new bool[0], false,
            new Color(0.12f, 0.42f, 0.62f), new Color(0.95f, 0.62f, 0.12f),
            ambience, interaction, success);

        CampaignLevelDefinition lab = Upsert(
            "03_Laboratoire_Logique", "laboratoire-logique", 3, "V2_02_Laboratoire_Logique",
            "Laboratoire, logique", CampaignPuzzleKind.LogicLab,
            "Deduis l'etat de quatre interrupteurs, teste ta combinaison et ouvre le sas du laboratoire.",
            "Regle les quatre interrupteurs puis valide la combinaison unique.",
            "Le sas scientifique est stabilise : le niveau de logique est termine.",
            new[]
            {
                "Lis les trois relations affichees au-dessus de la console.",
                "Les interrupteurs 1, 3 et 4 doivent avoir le meme etat.",
                "La solution est ON, OFF, ON, ON. Les essais sont illimites."
            },
            new int[0], new[] { true, false, true, true }, false,
            new Color(0.08f, 0.48f, 0.52f), new Color(0.92f, 0.42f, 0.14f),
            ambience, interaction, success);

        CampaignLevelDefinition auditorium = Upsert(
            "04_Auditorium_Sequence", "auditorium-sequence", 4, "V2_03_Auditorium_Sequence",
            "Auditorium, sons et sequences", CampaignPuzzleKind.SoundSequence,
            "Observe et ecoute quatre signaux, puis reproduis-les sur les pads de la scene.",
            "Lance la sequence puis reproduis les quatre couleurs dans le bon ordre.",
            "La sequence de l'auditorium est restituee, avec ou sans le son.",
            new[]
            {
                "Le bouton Rejouer repete la sequence autant de fois que necessaire.",
                "Le grand afficheur ecrit chaque couleur : le son n'est jamais obligatoire.",
                "La sequence est JAUNE, ROUGE, VERT, BLEU. Une erreur relance seulement la saisie."
            },
            new[] { 2, 0, 3, 1 }, new bool[0], false,
            new Color(0.38f, 0.16f, 0.52f), new Color(0.94f, 0.25f, 0.42f),
            ambience, interaction, success);

        CampaignLevelDefinition gym = Upsert(
            "05_Gymnase_Orientation", "gymnase-orientation", 5, "V2_04_Gymnase_Orientation",
            "Gymnase, orientation", CampaignPuzzleKind.OrientationCourse,
            "Traverse un parcours spatial et active cinq balises dans l'ordre. Le chronometre reste indicatif.",
            "Retrouve et active les cinq balises du gymnase dans l'ordre.",
            "Les cinq balises sont validees sans penalite bloquante.",
            new[]
            {
                "Les numeros des balises donnent directement l'ordre attendu.",
                "Les obstacles se contournent tous par au moins deux chemins.",
                "Le temps affiche est facultatif : aucune limite ne peut faire echouer le niveau."
            },
            new[] { 0, 1, 2, 3, 4 }, new bool[0], true,
            new Color(0.08f, 0.48f, 0.22f), new Color(0.96f, 0.56f, 0.08f),
            ambience, interaction, success);

        CampaignLevelDefinition arts = Upsert(
            "06_Arts_Motifs", "arts-motifs", 6, "V2_05_Arts_Motifs",
            "Arts et musique, motifs", CampaignPuzzleKind.ArtPatterns,
            "Resous trois motifs successifs de forme, couleur et rythme dans une galerie interactive.",
            "Observe la consigne et selectionne le bon motif a chaque etape.",
            "La composition en trois etapes ouvre la galerie des arts.",
            new[]
            {
                "Chaque etape remplace la consigne ; une erreur ne retire aucune etape validee.",
                "Pense successivement aux angles, aux couleurs opposees puis a l'alternance.",
                "Les choix sont : cercle jaune, arc vert, puis colonne rouge."
            },
            new[] { 1, 3, 2 }, new bool[0], false,
            new Color(0.54f, 0.20f, 0.12f), new Color(0.96f, 0.72f, 0.10f),
            ambience, interaction, success);

        CampaignLevelDefinition secret = Upsert(
            "07_College_Secret", "college-secret", 7, "V2_06_College_Secret",
            "Le college secret", CampaignPuzzleKind.SecretCollege,
            "Combine exploration, code, logique et sequence dans une epreuve finale en cinq phases sauvegardees.",
            "Retrouve les trois sceaux qui protegent le nouveau tresor.",
            "Le nouveau tresor est ouvert : les six niveaux supplementaires sont accomplis.",
            new[]
            {
                "Chaque porte correspond a un acquis : exploration, code, logique puis sequence.",
                "Aucune erreur ne remet les phases terminees a zero ; utilise le menu Pause pour reprendre plus tard.",
                "Code 7-3-1 ; relais ON-ON-OFF ; symboles 2-4-1-3."
            },
            new[] { 7, 3, 1, 1, 3, 0, 2 }, new[] { true, true, false }, false,
            new Color(0.08f, 0.16f, 0.34f), new Color(0.94f, 0.58f, 0.08f),
            ambience, interaction, success);

        CreateMenuScene();
        CreateLevelScene(cdi);
        CreateLevelScene(lab);
        CreateLevelScene(auditorium);
        CreateLevelScene(gym);
        CreateLevelScene(arts);
        CreateLevelScene(secret);

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(SceneFolder + "/CampaignMenu.unity", true),
            new EditorBuildSettingsScene("Assets/Scenes/CollegePrototype.unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + cdi.sceneName + ".unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + lab.sceneName + ".unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + auditorium.sceneName + ".unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + gym.sceneName + ".unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + arts.sceneName + ".unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/" + secret.sceneName + ".unity", true)
        };

        EditorSceneManager.OpenScene(SceneFolder + "/CampaignMenu.unity");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        CampaignCatalog.ClearCacheForEditor();
        Debug.Log("CAMPAGNE V2 GENEREE : 1 aventure originale + 6 nouveaux niveaux.");
    }

    static CampaignLevelDefinition Upsert(
        string assetName,
        string id,
        int chapter,
        string scene,
        string title,
        CampaignPuzzleKind kind,
        string description,
        string objective,
        string completion,
        string[] hints,
        int[] sequence,
        bool[] switches,
        bool timer,
        Color primary,
        Color accent,
        AudioClip ambience,
        AudioClip interaction,
        AudioClip success)
    {
        string path = DefinitionFolder + "/" + assetName + ".asset";
        CampaignLevelDefinition definition = AssetDatabase.LoadAssetAtPath<CampaignLevelDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<CampaignLevelDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.levelId = id;
        definition.chapterNumber = chapter;
        definition.sceneName = scene;
        definition.title = title;
        definition.puzzleKind = kind;
        definition.description = description;
        definition.initialObjective = objective;
        definition.completionMessage = completion;
        definition.hints = hints;
        definition.primarySequence = sequence;
        definition.switchSolution = switches;
        definition.optionalTimer = timer;
        definition.primaryColor = primary;
        definition.accentColor = accent;
        definition.ambience = ambience;
        definition.interactionSound = interaction;
        definition.successSound = success;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    static void CreateMenuScene()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject menu = new GameObject("CampaignMenu");
        menu.AddComponent<CampaignMenuController>();
        EditorSceneManager.SaveScene(scene, SceneFolder + "/CampaignMenu.unity");
    }

    static void CreateLevelScene(CampaignLevelDefinition definition)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject bootstrapObject = new GameObject("CampaignLevel_" + definition.levelId);
        CampaignLevelBootstrap bootstrap = bootstrapObject.AddComponent<CampaignLevelBootstrap>();
        bootstrap.definition = definition;
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, SceneFolder + "/" + definition.sceneName + ".unity");
    }

    static void EnsureFolder(string parent, string child)
    {
        string path = parent + "/" + child;
        if (!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder(parent, child);
    }
}
