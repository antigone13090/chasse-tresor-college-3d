# Plan V2 — six nouveaux niveaux de la chasse au trésor 3D

## Point de départ vérifié
Le jeu Unity 6000.3.2f1 en v1.0.0 propose un **parcours unique de six indices séquentiels**, puis un coffre final. Ces six indices ne sont pas six scènes/niveaux distincts. La branche `main` et ses releases doivent rester inchangées.

## Objectif
Conserver le parcours d'origine comme **Chapitre 1** et créer six **nouveaux niveaux jouables** (chapitres 2 à 7), avec difficulté graduée, objectifs, énigmes, retours explicites, points de reprise et fin de campagne. Ne pas prétendre que le jeu compte déjà six niveaux autonomes.

## Niveaux proposés (à adapter aux espaces et assets réellement présents)
1. **Niveau supplémentaire 1 — CDI, codes cachés** : exploration et décodage de trois indices pour ouvrir une porte.
2. **Niveau supplémentaire 2 — Laboratoire, logique** : interrupteurs et déduction, combinaison unique validée.
3. **Niveau supplémentaire 3 — Auditorium, sons et séquences** : mémorisation d'une séquence sonore/visuelle, alternative accessible sans audio.
4. **Niveau supplémentaire 4 — Gymnase, orientation** : parcours spatial, balises, chronomètre facultatif, sans blocage irréversible.
5. **Niveau supplémentaire 5 — Arts et musique, motifs** : reconnaissance de formes et énigme multi-étapes, indices contextuels.
6. **Niveau supplémentaire 6 — Le collège secret** : réutilisation des acquis, énigme finale à plusieurs phases et nouveau trésor.

## Architecture et rétrocompatibilité
- Inspecter `Assets/Scripts/TreasureGameManager.cs`, `ClueObject.cs`, `PlayerInteraction3D.cs`, scènes, prefabs, scripts Editor et tests existants avant toute modification.
- Introduire une définition de niveau (id stable, objectifs, portes, énigmes, transitions, sauvegarde) découplée du parcours original; privilégier `ScriptableObject` si cohérent avec le projet.
- Réutiliser les systèmes existants (déplacement FPS, interactions, minimap, audio et UI) sans casser le mode original.
- Définir des conditions de victoire explicites, anti-softlock, pause/reprise, réinitialisation, aide progressive et accessibilité clavier.
- Toute scène ou ressource Unity ajoutée doit inclure ses métadonnées `.meta` et références cohérentes.
- Aucune modification de `main` ni publication de release sans validation.

## Étapes de réalisation pour Codex
1. Audit du dépôt, des scènes et des tests; état réel du système de progression.
2. Socle de niveaux, navigation entre chapitres et sauvegarde; tests.
3. Implémenter chaque niveau séparément avec tests déterministes de réussite/échec et de progression.
4. Campagne complète, menu de sélection, rejouabilité et documentation enseignant.
5. Vérification automatique (compilation Unity, tests Editor/PlayMode, validation release), contrôle manuel de jouabilité et performance sur au moins une plateforme.
6. Ouvrir une PR depuis `feature/v2-six-nouveaux-niveaux` vers `main` une fois le tout vérifié.

## Critères d'acceptation
- Ancienne aventure inchangée et jouable.
- Six nouveaux niveaux distincts effectivement jouables, pas seulement six indices ou maquettes.
- Transitions, progression et sauvegarde opérationnelles.
- Aucune impasse bloquante; énigmes résolubles, explications et indices disponibles.
- Tests existants toujours verts et nouveaux tests pour chaque niveau.
- Documentation et consignes de build reproductibles.

## État d'implémentation de la branche

Le socle modulaire, le menu de campagne, la sauvegarde et les six niveaux sont implémentés. Les
résultats reproductibles et les limites de validation sont consignés dans `docs/VALIDATION_V2.md` ;
les choix techniques dans `docs/CAMPAGNE_V2_ARCHITECTURE.md` ; l'exploitation pédagogique dans
`docs/CAMPAGNE_V2_GUIDE_ENSEIGNANT.md`.
