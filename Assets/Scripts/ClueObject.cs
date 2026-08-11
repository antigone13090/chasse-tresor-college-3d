using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public int order;

    public string clueTitle;
    public string clueText;
    public string nextObjective;

    public bool collected = false;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer =
            GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (collected)
            return;

        if (
            TreasureGameManager.Instance
            == null
        )
            return;

        if (
            !TreasureGameManager.Instance
                .CanCollect(order)
        )
        {
            TreasureGameManager.Instance
                .ShowMessage(
                    "Cet indice n'est pas encore accessible. "
                    + "Cherche d'abord l'indice "
                    + (
                        TreasureGameManager
                            .Instance
                            .foundClues
                        + 1
                    )
                    + ".",
                    3f
                );

            return;
        }

        TreasureGameManager.Instance
            .CollectClue(
                order,
                clueTitle,
                clueText,
                nextObjective
            );

        collected = true;

        Collider col =
            GetComponent<Collider>();

        if (col != null)
            col.enabled = false;

        if (objectRenderer != null)
            objectRenderer.enabled = false;
    }

    void Update()
    {
        if (!collected)
        {
            transform.Rotate(
                0f,
                60f * Time.deltaTime,
                0f
            );
        }
    }
}
