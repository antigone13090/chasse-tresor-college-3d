namespace CollegeTreasure.Campaign
{
    public enum SequenceStepResult
    {
        Incorrect,
        Accepted,
        Completed
    }

    public static class CampaignPuzzleRules
    {
        public static bool SequenceEquals(int[] values, int[] solution)
        {
            if (values == null || solution == null || values.Length != solution.Length)
                return false;

            for (int i = 0; i < solution.Length; i++)
            {
                if (values[i] != solution[i])
                    return false;
            }

            return true;
        }

        public static bool SwitchesMatch(bool[] values, bool[] solution)
        {
            if (values == null || solution == null || values.Length != solution.Length)
                return false;

            for (int i = 0; i < solution.Length; i++)
            {
                if (values[i] != solution[i])
                    return false;
            }

            return true;
        }

        public static SequenceStepResult AcceptSequenceStep(
            int[] solution,
            int currentIndex,
            int selectedValue)
        {
            if (solution == null || currentIndex < 0 || currentIndex >= solution.Length)
                return SequenceStepResult.Incorrect;

            if (solution[currentIndex] != selectedValue)
                return SequenceStepResult.Incorrect;

            return currentIndex == solution.Length - 1
                ? SequenceStepResult.Completed
                : SequenceStepResult.Accepted;
        }

        public static bool IsExpectedOrderedTarget(int currentIndex, int selectedIndex)
        {
            return currentIndex == selectedIndex;
        }

        public static bool IsCorrectPatternChoice(int stage, int selectedIndex, int[] solutions)
        {
            return solutions != null
                && stage >= 0
                && stage < solutions.Length
                && solutions[stage] == selectedIndex;
        }
    }
}
