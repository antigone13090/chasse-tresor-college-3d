using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public interface ICampaignInteractable
    {
        string InteractionPrompt { get; }
        bool InteractionEnabled { get; }
        Transform InteractionTransform { get; }
        void Interact(CampaignPlayerInteractor interactor);
    }
}
