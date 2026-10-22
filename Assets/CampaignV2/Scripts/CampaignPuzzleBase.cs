using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public abstract class CampaignPuzzleBase
    {
        protected CampaignLevelController Level { get; private set; }
        protected CampaignLevelDefinition Definition { get; private set; }
        protected CampaignWorldBuilder World { get; private set; }

        public abstract int CurrentStep { get; }
        public virtual string SerializedState { get { return string.Empty; } }

        public void Initialize(
            CampaignLevelController level,
            CampaignLevelDefinition definition,
            CampaignWorldBuilder world,
            CampaignCheckpoint checkpoint)
        {
            Level = level;
            Definition = definition;
            World = world;
            Build(checkpoint);
        }

        protected abstract void Build(CampaignCheckpoint checkpoint);
        public abstract void HandleAction(string actionId);
        public virtual void Tick(float deltaTime) { }

        public void SaveCheckpoint()
        {
            Level.SaveCheckpoint(CurrentStep, SerializedState);
        }

        protected int ParseIndex(string actionId)
        {
            int separator = actionId.LastIndexOf('-');
            int value;
            return separator >= 0 && int.TryParse(actionId.Substring(separator + 1), out value)
                ? value
                : -1;
        }
    }
}
