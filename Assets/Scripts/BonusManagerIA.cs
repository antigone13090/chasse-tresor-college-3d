using UnityEngine;

public class BonusManagerIA : MonoBehaviour
{
    public static BonusManagerIA Instance { get; private set; }
    public int foundBonuses;
    public int totalBonuses = 3;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterBonus()
    {
        foundBonuses = Mathf.Min(foundBonuses + 1, totalBonuses);
    }
}
