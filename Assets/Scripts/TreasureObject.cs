using UnityEngine;

public class TreasureObject : MonoBehaviour
{
    public bool collected = false;

    public void Interact()
    {
        if (collected)
            return;

        if (TreasureGameManager.Instance == null)
            return;

        TreasureGameManager game =
            TreasureGameManager.Instance;

        if (game.foundClues < game.totalClues)
        {
            game.ShowMessage(
                "Le trésor est verrouillé : "
                + "trouve les 6 indices.",
                3f
            );

            return;
        }

        collected = true;

        game.Victory();

        foreach (
            Renderer r
            in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        foreach (
            Collider c
            in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }
}
