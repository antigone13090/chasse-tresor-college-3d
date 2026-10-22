# Validation de la campagne V2

Validation effectuée sous Linux avec Unity **6000.3.2f1**, sur la branche
`feature/v2-six-nouveaux-niveaux`.

## Résultats exécutés

| Contrôle | Résultat |
|---|---:|
| Compilation/import Unity | réussi |
| Validateur campagne V2 | **127 PASS / 0 FAIL** |
| Validateur logique v1 | **60 PASS / 0 FAIL** |
| Validateur parcours physique v1 | **16 PASS / 0 FAIL** |
| Validateur release v1 | **70 PASS / 0 FAIL** |
| Build Linux V2 Mono | **Succeeded — 98,7 Mo** |
| Démarrage du player Linux sous X virtuel | **`CAMPAIGN_V2_PLAYER_SMOKE_OK`** |

Le validateur V2 exécute les solutions déterministes, les erreurs attendues et un parcours complet de
chacun des six niveaux. Il vérifie également une reprise CDI après deux fragments sauvegardés, le
round-trip JSON, les huit scènes du build, les références des définitions, les interactions 3D,
colliders et fichiers `.meta`.

Le build validé a été produit par `CampaignV2DesktopBuild.BuildLinux` dans
`Builds/V2/Linux/ChasseAuTresorCollegeV2.x86_64`. Le répertoire `Builds/` est ignoré par Git.

## Non exécuté dans cet environnement

- parcours manuel humain complet avec clavier/souris et contrôle visuel de chaque cadrage ;
- builds Windows et macOS, leurs modules Unity n'ayant pas été utilisés ici ;
- tests sur plusieurs résolutions, GPU réels, configurations audio et performances longues durées ;
- signature, installateur, archive de distribution ou publication de release.

Le premier essai du player avec le backend graphique nul (`-nographics`) a segfaulté avant le
chargement de scène dans UnityPlayer. Le même binaire a ensuite démarré et quitté proprement sous
`xvfb-run`, ce qui isole cette limite au backend headless de l'environnement. Ce premier essai n'est
pas compté comme une validation.
