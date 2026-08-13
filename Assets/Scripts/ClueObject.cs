using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public int order;

    public string clueTitle;
    public string clueText;
    public string nextObjective;

    public bool collected = false;

    [Header("IA - Visibilite de l'indice")]
    public float rotationSpeed = 60f;
    public float bobHeight = 0.18f;
    public float bobSpeed = 2f;

    private Renderer objectRenderer;
    private Vector3 baseLocalPosition;

    void Start()
    {
        objectRenderer =
            GetComponent<Renderer>();

        baseLocalPosition =
            transform.localPosition;
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
        if (collected)
            return;

        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );

        Vector3 position =
            baseLocalPosition;

        position.y +=
            Mathf.Sin(Time.time * bobSpeed)
            * bobHeight;

        transform.localPosition =
            position;
    }
}
