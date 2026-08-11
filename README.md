# Chasse au Trésor du Collège 3D

Jeu éducatif d'exploration 3D réalisé avec **Unity 6000.3.2f1**.

Le joueur explore un collège, découvre **six indices dans le bon ordre**, puis accède au trésor final.

**Version stable : v1.0.0**

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
