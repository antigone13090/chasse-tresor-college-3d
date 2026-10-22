# Chasse au Trésor du Collège 3D

Jeu éducatif d'exploration 3D réalisé avec **Unity 6000.3.2f1**.

Le joueur explore un collège, découvre **six indices dans le bon ordre**, puis accède au trésor final.

**Version stable : v1.0.0**

> La branche `feature/v2-six-nouveaux-niveaux` contient la campagne V2 en développement.
> Elle conserve l'aventure v1 comme chapitre 1 et ajoute six véritables niveaux autonomes.

## Télécharger le jeu

| Plateforme | Téléchargement | Format |
|---|---|---|
| Windows 64 bits | **[Télécharger l'installateur Windows](https://github.com/antigone13090/chasse-tresor-college-3d/releases/download/v1.0.0/ChasseAuTresorCollege-Setup-v1.0.exe)** | `.exe` |
| macOS Intel + Apple Silicon | **[Télécharger la version macOS Universal](https://github.com/antigone13090/chasse-tresor-college-3d/releases/download/v1.0.0/ChasseAuTresorCollege-macOS-Universal-v1.0.zip)** | `.zip` |
| Linux 64 bits | **[Télécharger la version Linux](https://github.com/antigone13090/chasse-tresor-college-3d/releases/download/v1.0.0/ChasseAuTresorCollege-Linux64-v1.0.tar.gz)** | `.tar.gz` |

Tous les fichiers sont également disponibles dans la **[release GitHub v1.0.0](https://github.com/antigone13090/chasse-tresor-college-3d/releases/tag/v1.0.0)**.

Des fichiers SHA-256 sont fournis pour les versions Windows et macOS.

## Installation

### Windows

1. Télécharger `ChasseAuTresorCollege-Setup-v1.0.exe`.
2. Lancer l'installateur.
3. Choisir le dossier d'installation.
4. Lancer le jeu depuis le raccourci Bureau ou le menu Démarrer.

L'installateur comprend également la désinstallation du jeu.

### macOS

1. Télécharger `ChasseAuTresorCollege-macOS-Universal-v1.0.zip`.
2. Décompresser l'archive.
3. Lancer `ChasseAuTresorCollege.app`.

Le build macOS est **Universal** et contient les architectures :

- Intel `x86_64` ;
- Apple Silicon `arm64`.

### Linux

1. Télécharger `ChasseAuTresorCollege-Linux64-v1.0.tar.gz`.
2. Extraire l'archive.
3. Rendre l'exécutable exécutable si nécessaire.
4. Lancer le jeu.

## Parcours du jeu

Les indices doivent être trouvés dans cet ordre :

1. Vie scolaire
2. CDI
3. Auditorium
4. Gymnase
5. Arts / Musique
6. Self élèves
7. Trésor final

## Campagne V2 (branche de développement)

Le menu de campagne distingue explicitement l'aventure originale des six niveaux supplémentaires :

1. **Chapitre 1 — L'aventure originale** : scène `CollegePrototype`, six indices et trésor v1 inchangés ;
2. **CDI, codes cachés** : trois fragments à explorer puis une porte à code ;
3. **Laboratoire, logique** : quatre interrupteurs et une combinaison déduite ;
4. **Auditorium, sons et séquences** : séquence sonore et visuelle, jouable sans audio ;
5. **Gymnase, orientation** : cinq balises, parcours sans élimination et temps facultatif ;
6. **Arts et musique, motifs** : énigme de formes, couleurs et rythme en trois étapes ;
7. **Le collège secret** : finale en cinq phases et nouveau trésor.

La progression V2 est sauvegardée automatiquement dans `Application.persistentDataPath/campaign-v2.json`.
Chaque niveau possède un point de reprise, des aides progressives (`H` ou `F1`), une remise à zéro
et une condition de victoire indépendante. Les interactions peuvent être ciblées à la souris ou au
clavier avec `Tab`, puis `Entrée`.

Voir [le guide enseignant](docs/CAMPAGNE_V2_GUIDE_ENSEIGNANT.md) et
[la documentation d'architecture](docs/CAMPAGNE_V2_ARCHITECTURE.md).

## Fonctionnalités

- exploration FPS en 3D ;
- portes interactives ;
- six indices séquentiels ;
- coffre final ;
- mini-carte ;
- musique originale et bruitages ;
- effets visuels et halos animés ;
- personnages animés ;
- bâtiments et espaces visitables ;
- textures PBR ;
- écran d'accueil ;
- menu Pause ;
- réglage du son et de la sensibilité de la souris ;
- démonstration automatique ;
- bande-annonce intégrée ;
- profils de performances selon la plateforme ;
- builds Windows, macOS et Linux ;
- tests automatiques de logique, parcours et release.

## Commandes

| Touche | Action |
|---|---|
| `Z` / `W` | Avancer |
| `S` | Reculer |
| `Q` / `A` | Aller à gauche |
| `D` | Aller à droite |
| Souris | Regarder |
| `E` | Interagir |
| `M` | Afficher / masquer la mini-carte |
| `Échap` | Pause / libérer la souris |
| `F2` | Démonstration automatique |
| `F3` | Bande-annonce |
| `F10` | Quitter le jeu |
| `R` | Recommencer après la victoire |

## Validation v1.0

| Test | Résultat |
|---|---:|
| Logique du jeu | **60 PASS / 0 FAIL** |
| Parcours physique | **16 PASS / 0 FAIL** |
| Validation complète de release | **70 PASS / 0 FAIL** |

**RELEASE V1.0 VALIDÉE**

La séquence complète des six indices, le trésor final et les builds des trois plateformes ont été validés.

## Captures d'écran

Des captures d'écran et images de présentation pourront être ajoutées ici dans une prochaine mise à jour.

<!--
Exemple :
![Vue du collège](docs/screenshots/college.jpg)
![Mini-carte](docs/screenshots/minimap.jpg)
![Trésor final](docs/screenshots/treasure.jpg)
-->

## Construire depuis les sources

### Prérequis

- Unity **6000.3.2f1** ;
- projet cloné localement ;
- module de build correspondant à la plateforme cible.

### macOS

Le dépôt contient `build-mac.command` pour automatiser la validation, le build macOS et la création de l'archive de distribution.

### Windows et Linux

Les scripts Unity présents dans `Assets/Editor/` permettent de lancer les builds desktop depuis Unity.

Sur la branche V2, utiliser les méthodes dédiées afin d'obtenir des binaires identifiés `2.0.0` :

```bash
# Validation V2 déterministe
Unity -batchmode -nographics -projectPath . \
  -executeMethod CampaignV2Validator.Run -quit -logFile Logs/campaign-v2-validation.log

# Build Linux V2 (adapter le chemin de Unity)
Unity -batchmode -nographics -projectPath . \
  -executeMethod CampaignV2DesktopBuild.BuildLinux -quit -logFile Logs/campaign-v2-linux-build.log
```

Les sorties V2 sont écrites dans `Builds/V2/`. Les méthodes équivalentes Windows et macOS sont
`CampaignV2DesktopBuild.BuildWindows` et `CampaignV2DesktopBuild.BuildMac` ; elles nécessitent les
modules Unity correspondants.

## Avertissements de sécurité au premier lancement

Les exécutables Windows et macOS de cette version ne sont pas signés avec un certificat commercial.

Selon la configuration du système :

- **Windows SmartScreen** peut afficher un avertissement ;
- **macOS Gatekeeper** peut demander une confirmation au premier lancement.

Cela n'empêche pas l'utilisation locale du jeu après autorisation par l'utilisateur.

## Ressources tierces

Certaines textures PBR proviennent de **ambientCG** et sont distribuées sous licence **CC0**.

Voir :

`Assets/ThirdParty/ambientCG_LICENSE.txt`

## Projet original

Cette version Unity 3D est indépendante de la précédente version 2.5D du projet.

## État du projet

**Chasse au Trésor du Collège 3D v1.0.0 — stable, validé et disponible pour Windows, macOS et Linux.**
