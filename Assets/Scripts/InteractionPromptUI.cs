using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    PlayerInteraction3D player;
    GUIStyle promptStyle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        if (FindFirstObjectByType<InteractionPromptUI>() != null)
            return;

        GameObject host = new GameObject("InteractionPromptUI_IA");
        host.AddComponent<InteractionPromptUI>();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerInteraction3D>();
    }

    void PrepareStyle()
    {
        if (promptStyle != null)
            return;

        promptStyle = new GUIStyle(GUI.skin.box);
        promptStyle.fontSize = 17;
        promptStyle.fontStyle = FontStyle.Bold;
        promptStyle.alignment = TextAnchor.MiddleCenter;
        promptStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerInteraction3D>();
            if (player == null)
                return;
        }

        string prompt = FindPrompt();
        if (string.IsNullOrEmpty(prompt))
            return;

        PrepareStyle();

        float width = 280f;
        float height = 42f;
        Rect rect = new Rect(
            (Screen.width - width) / 2f,
            Screen.height - 90f,
            width,
            height
        );

        GUI.Box(rect, prompt, promptStyle);
    }

    string FindPrompt()
    {
        float clueDistance = player.cluePickupDistance;
        float treasureDistance = player.treasurePickupDistance;
        float interactionDistance = player.interactionDistance;

        Collider[] clueHits = Physics.OverlapSphere(player.transform.position, clueDistance);
        foreach (Collider hit in clueHits)
        {
            ClueObject clue = hit.GetComponentInParent<ClueObject>();
            if (clue != null && !clue.collected)
                return "E - Recuperer l'indice";
        }

        Collider[] treasureHits = Physics.OverlapSphere(player.transform.position, treasureDistance);
        foreach (Collider hit in treasureHits)
        {
            TreasureObject treasure = hit.GetComponentInParent<TreasureObject>();
            if (treasure != null && !treasure.collected)
                return "E - Ouvrir le tresor";
        }

        Collider[] doorHits = Physics.OverlapSphere(player.transform.position, interactionDistance);
        foreach (Collider hit in doorHits)
        {
            DoorController door = hit.GetComponentInParent<DoorController>();
            if (door != null)
                return "E - Ouvrir / fermer";
        }

        return "";
    }
}
