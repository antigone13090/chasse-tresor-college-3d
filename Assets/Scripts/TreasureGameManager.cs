using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureGameManager : MonoBehaviour
{
    public static TreasureGameManager Instance;

    public int foundClues = 0;
    public int totalClues = 6;

    public string objective =
        "Trouve l'indice 1 à la Vie scolaire.";

    public bool victory = false;

    private string message = "";
    private float messageUntil = 0f;

    GUIStyle hudStyle;
    GUIStyle objectiveStyle;
    GUIStyle messageStyle;
    GUIStyle victoryStyle;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (victory && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
    }

    public bool CanCollect(int order)
    {
        return order == foundClues + 1;
    }

    public void CollectClue(
        int order,
        string title,
        string text,
        string nextObjective)
    {
        if (!CanCollect(order))
        {
            ShowMessage(
                "Tu dois d'abord trouver l'indice "
                + (foundClues + 1) + ".",
                3f
            );

            return;
        }

        foundClues++;
        objective = nextObjective;

        ShowMessage(
            title + "\n" + text,
            6f
        );

        Debug.Log(
            "Indice recupere : "
            + foundClues
            + "/"
            + totalClues
        );
    }

    public void Victory()
    {
        if (victory)
            return;

        victory = true;

        objective =
            "Victoire : tu as terminé la chasse au trésor.";

        message =
            "Victoire ! Tu as trouvé le trésor du collège.";

        messageUntil =
            Time.time + 9999f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log("VICTOIRE - TRESOR TROUVE");
    }

    public void ShowMessage(
        string text,
        float seconds)
    {
        message = text;
        messageUntil =
            Time.time + seconds;
    }

    void PrepareStyles()
    {
        if (hudStyle != null)
            return;

        hudStyle =
            new GUIStyle(GUI.skin.box);

        hudStyle.fontSize = 25;
        hudStyle.alignment =
            TextAnchor.MiddleCenter;

        hudStyle.normal.textColor =
            Color.white;

        objectiveStyle =
            new GUIStyle(GUI.skin.box);

        objectiveStyle.fontSize = 20;
        objectiveStyle.alignment =
            TextAnchor.MiddleLeft;

        objectiveStyle.normal.textColor =
            Color.white;

        messageStyle =
            new GUIStyle(GUI.skin.box);

        messageStyle.fontSize = 23;
        messageStyle.alignment =
            TextAnchor.MiddleCenter;

        messageStyle.wordWrap = true;

        messageStyle.normal.textColor =
            Color.white;

        victoryStyle =
            new GUIStyle(GUI.skin.box);

        victoryStyle.fontSize = 38;
        victoryStyle.fontStyle =
            FontStyle.Bold;

        victoryStyle.alignment =
            TextAnchor.MiddleCenter;

        victoryStyle.wordWrap = true;

        victoryStyle.normal.textColor =
            Color.white;
    }

    void OnGUI()
    {
        PrepareStyles();

        GUI.Box(
            new Rect(20,20,220,55),
            "Indices : "
            + foundClues
            + "/"
            + totalClues,
            hudStyle
        );

        GUI.Box(
            new Rect(20,85,700,60),
            "Objectif : " + objective,
            objectiveStyle
        );

        if (!victory)
        {
            GUI.Label(
                new Rect(
                    Screen.width/2-10,
                    Screen.height/2-15,
                    30,
                    30
                ),
                "+",
                hudStyle
            );
        }

        if (
            !string.IsNullOrEmpty(message)
            &&
            Time.time < messageUntil
            &&
            !victory
        )
        {
            GUI.Box(
                new Rect(
                    Screen.width/2-320,
                    Screen.height-180,
                    640,
                    120
                ),
                message,
                messageStyle
            );
        }

        if (victory)
        {
            GUI.Box(
                new Rect(
                    Screen.width/2-360,
                    Screen.height/2-150,
                    720,
                    300
                ),
                "VICTOIRE !\n\n"
                + "Tu as trouvé le trésor du collège.\n\n"
                + "Appuie sur R pour recommencer.",
                victoryStyle
            );
        }
    }
}
