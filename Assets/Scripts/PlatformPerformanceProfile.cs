using UnityEngine;

public class PlatformPerformanceProfile : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        // Profil spécialement adapté aux anciens Mac Intel
        // disposant de peu de mémoire graphique / système.
        if (
            Application.platform
            == RuntimePlatform.OSXPlayer
            &&
            SystemInfo.systemMemorySize <= 9000
        )
        {
            QualitySettings.shadowDistance =
                Mathf.Min(
                    QualitySettings.shadowDistance,
                    45f
                );

            QualitySettings.lodBias = 1.0f;

            QualitySettings.antiAliasing = 2;

            QualitySettings.pixelLightCount =
                Mathf.Min(
                    QualitySettings.pixelLightCount,
                    3
                );

            Debug.Log(
                "PROFIL PERFORMANCE MAC INTEL ACTIVE"
            );
        }
        else
        {
            QualitySettings.shadowDistance =
                Mathf.Min(
                    QualitySettings.shadowDistance,
                    100f
                );
        }
    }
}
