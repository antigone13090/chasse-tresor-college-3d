# Architecture de la campagne V2

## Audit de départ

La v1 repose sur une scène unique, `Assets/Scenes/CollegePrototype.unity`, un
`TreasureGameManager` global et six `ClueObject` séquentiels. Ces indices ne sont pas des niveaux.
La scène contient déjà le déplacement FPS, les portes, la mini-carte, les sons, l'UI, les décors et
les six espaces thématiques. Les trois validateurs Editor historiques couvrent la logique des indices,
le chemin physique et la release, mais le dépôt ne contenait pas de campagne multi-scène ni de
sauvegarde.

La scène originale et ses scripts de progression n'ont pas été modifiés. Elle reste chargeable comme
chapitre 1 depuis le nouveau menu.

## Composition

- `CampaignLevelDefinition` est un `ScriptableObject` à identifiant stable. Il décrit le chapitre,
  la scène, le type d'énigme, les objectifs, solutions, aides, couleurs et sons.
- `CampaignCatalog` charge les sept définitions depuis `Resources/CampaignLevels` et les ordonne.
- `CampaignMenuController` présente l'original et les six nouveaux niveaux, avec verrouillage,
  statut terminé et remise à zéro de campagne.
- `CampaignFlow` centralise les transitions entre menu et chapitres.
- `CampaignProgressService` sérialise progression et checkpoints en JSON. L'écriture passe par un
  fichier temporaire et conserve une copie `.bak`; une sauvegarde illisible revient à un état neuf.
- `CampaignLevelController` gère objectifs, HUD, pause, aides, reprise, victoire et passage au niveau
  suivant.
- `CampaignPuzzleBase` isole le cycle de vie commun. Les six implémentations se trouvent dans
  `CampaignPuzzles.cs`; leurs règles pures partagées sont dans `CampaignPuzzleRules.cs`.
- `CampaignWorldBuilder` construit les environnements 3D à partir de primitives Unity et réutilise
  les sons du projet. Chaque scène reste légère et référence sa propre définition.
- `CampaignPlayerInteractor` accepte la visée classique ou la sélection clavier `Tab` + `Entrée`.
- `CampaignV2Generator` régénère de façon idempotente définitions, scènes et Build Settings.
- `CampaignV2Validator` vérifie structure, métadonnées, règles, sauvegarde, environnements physiques
  et parcours complets avec erreurs volontaires.

## Scènes et progression

Le build contient huit scènes dans cet ordre : menu V2, aventure originale, puis les six nouvelles
scènes. Le chapitre 2 est disponible dès la première partie afin que la conservation de la v1 ne rende
pas la nouvelle campagne dépendante d'une modification du gestionnaire historique. Terminer un niveau
V2 débloque le suivant. Une reprise restaure l'étape validée, mais jamais une erreur temporaire.

Les niveaux construisent leur décor au chargement. Ils possèdent tous un sol et des murs avec
colliders, un `CharacterController`, une caméra FPS, plusieurs objets `CampaignActionObject`, une
condition de sortie explicite et au moins trois aides. Les erreurs ne suppriment jamais une phase déjà
validée.

## Ajouter un niveau

1. Ajouter une valeur à `CampaignPuzzleKind` et une classe dérivée de `CampaignPuzzleBase`.
2. Définir ses règles déterministes dans `CampaignPuzzleRules`.
3. Ajouter sa définition et sa scène dans `CampaignV2Generator`.
4. Ajouter un parcours gagnant et au moins un parcours d'échec dans `CampaignV2Validator`.
5. Relancer le générateur, le validateur V2, les trois validateurs v1 et un build de plateforme.

Ne pas éditer manuellement les GUID des `.meta`. Le générateur Unity crée et conserve les références
de scènes et de `ScriptableObject`.
