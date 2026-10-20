using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignActionObject : MonoBehaviour, ICampaignInteractable
    {
        string actionId;
        string displayName;
        CampaignPuzzleBase puzzle;
        Renderer cachedRenderer;
        bool interactionEnabled = true;

        public string ActionId { get { return actionId; } }

        public string InteractionPrompt
        {
            get { return interactionEnabled ? "E / Entree — " + displayName : string.Empty; }
        }

        public bool InteractionEnabled { get { return interactionEnabled && isActiveAndEnabled; } }
        public Transform InteractionTransform { get { return transform; } }

        public void Configure(
            CampaignPuzzleBase owner,
            string id,
            string label,
            Color color)
        {
            puzzle = owner;
            actionId = id;
            displayName = label;
            cachedRenderer = GetComponent<Renderer>();
            SetColor(color);
        }

        public void Interact(CampaignPlayerInteractor interactor)
        {
            if (InteractionEnabled && puzzle != null)
                puzzle.HandleAction(actionId);
        }

        public void SetInteractionEnabled(bool value)
        {
            interactionEnabled = value;
        }

        public void SetDisplayName(string value)
        {
            displayName = value;
        }

        public void SetColor(Color color)
        {
            if (cachedRenderer == null)
                cachedRenderer = GetComponent<Renderer>();

            if (cachedRenderer != null)
                cachedRenderer.material.color = color;
        }
    }
}
