using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace CollegeTreasure.Campaign
{
    sealed class HiddenCodesPuzzle : CampaignPuzzleBase
    {
        readonly bool[] fragments = new bool[3];
        readonly List<int> entered = new List<int>();
        readonly List<CampaignActionObject> fragmentActions = new List<CampaignActionObject>();
        GameObject gate;
        CampaignActionObject exit;
        bool unlocked;

        public override int CurrentStep { get { return unlocked ? 4 : CountFound(); } }
        public override string SerializedState
        {
            get { return CampaignStateCodec.Bits(fragments) + "|" + CampaignStateCodec.Ints(entered) + "|" + (unlocked ? "1" : "0"); }
        }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            Restore(checkpoint);
            World.CreateRoom(new Vector2(30f, 24f), 6f, new Color(0.12f, 0.09f, 0.055f), Definition.primaryColor * 0.38f);
            World.CreateWorldLabel("TitreCDI", "CDI — LES CODES CACHES", new Vector3(0f, 4.6f, 11.6f), 0.28f, Color.white);

            for (int row = 0; row < 3; row++)
            {
                float z = -4f + row * 4.2f;
                World.CreateBlock("RayonnageGauche_" + row, new Vector3(-10.5f, 1.5f, z), new Vector3(2.2f, 3f, 3.2f), new Color(0.22f, 0.12f, 0.05f));
                World.CreateBlock("RayonnageDroit_" + row, new Vector3(10.5f, 1.5f, z), new Vector3(2.2f, 3f, 3.2f), new Color(0.22f, 0.12f, 0.05f));
            }

            Vector3[] fragmentPositions =
            {
                new Vector3(-8.8f, 1.1f, -3.5f),
                new Vector3(9f, 2.2f, 1f),
                new Vector3(-5.5f, 0.8f, 5f)
            };

            string[] clues = { "Fragment A : 4", "Fragment B : 1", "Fragment C : 7" };
            for (int i = 0; i < fragments.Length; i++)
            {
                CampaignActionObject action = World.CreateAction(this, "fragment-" + i, clues[i], fragmentPositions[i],
                    new Vector3(0.7f, 0.7f, 0.18f), Definition.accentColor);
                fragmentActions.Add(action);
                if (fragments[i])
                {
                    action.SetInteractionEnabled(false);
                    action.SetColor(Color.green * 0.6f);
                }
            }

            World.CreateBlock("ConsoleCode", new Vector3(0f, 1.2f, 6.8f), new Vector3(8f, 2.4f, 0.8f), Definition.primaryColor * 0.6f);
            for (int digit = 0; digit <= 9; digit++)
            {
                float x = -5.4f + (digit % 5) * 2.7f;
                float y = digit < 5 ? 1.75f : 0.55f;
                World.CreateAction(this, "digit-" + digit, digit.ToString(), new Vector3(x, y, 6.25f),
                    new Vector3(1.1f, 0.7f, 0.35f), new Color(0.12f, 0.32f, 0.48f));
            }

            gate = World.CreateGate("PorteCDI", new Vector3(0f, 2.2f, 9.2f), new Vector3(5.5f, 4.4f, 0.55f));
            exit = World.CreateAction(this, "exit", "Franchir la porte", new Vector3(0f, 1f, 10.6f),
                new Vector3(2.2f, 2f, 0.6f), Color.green * 0.65f);
            ApplyUnlockedState();
            RefreshObjective();
        }

        public override void HandleAction(string actionId)
        {
            if (actionId.StartsWith("fragment-"))
            {
                int index = ParseIndex(actionId);
                if (index < 0 || index >= fragments.Length || fragments[index]) return;
                fragments[index] = true;
                fragmentActions[index].SetInteractionEnabled(false);
                fragmentActions[index].SetColor(Color.green * 0.6f);
                Level.PlayInteractionSound(0.9f + index * 0.12f);
                Level.ShowMessage("Code revele : " + Definition.primarySequence[index]);
                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId.StartsWith("digit-"))
            {
                if (CountFound() < fragments.Length)
                {
                    Level.ShowMessage("La console est incomplete : retrouve d'abord les trois fragments.");
                    return;
                }

                int digit = ParseIndex(actionId);
                entered.Add(digit);
                Level.PlayInteractionSound(0.8f + digit * 0.035f);

                int expectedIndex = entered.Count - 1;
                if (expectedIndex >= Definition.primarySequence.Length
                    || Definition.primarySequence[expectedIndex] != digit)
                {
                    entered.Clear();
                    Level.ShowMessage("Code incorrect. La console se remet a zero, les fragments restent acquis.");
                }
                else if (entered.Count == Definition.primarySequence.Length)
                {
                    unlocked = true;
                    ApplyUnlockedState();
                    Level.SetObjective("La porte est ouverte : rejoins la sortie du CDI.");
                    Level.SetProgress("Porte deverrouillee");
                    Level.ShowMessage("Combinaison correcte : la porte du CDI est ouverte !", 6f);
                }
                else
                {
                    Level.ShowMessage("Chiffre accepte (" + entered.Count + "/" + Definition.primarySequence.Length + ").", 2f);
                }

                SaveCheckpoint();
                return;
            }

            if (actionId == "exit" && unlocked)
                Level.CompleteLevel();
        }

        void RefreshObjective()
        {
            int found = CountFound();
            if (unlocked)
            {
                Level.SetObjective("Franchis la porte deverrouillee.");
                Level.SetProgress("Porte ouverte");
            }
            else if (found < fragments.Length)
            {
                Level.SetObjective("Explore les rayonnages et revele les trois fragments de code.");
                Level.SetProgress("Fragments : " + found + "/3");
            }
            else
            {
                Level.SetObjective("Entre les trois chiffres dans l'ordre A, B, C.");
                Level.SetProgress("Code : " + entered.Count + "/3");
            }
        }

        int CountFound()
        {
            int count = 0;
            foreach (bool value in fragments) if (value) count++;
            return count;
        }

        void ApplyUnlockedState()
        {
            Collider collider = gate.GetComponent<Collider>();
            collider.enabled = !unlocked;
            gate.GetComponent<Renderer>().material.color = unlocked ? Color.green * 0.55f : Definition.accentColor * 0.65f;
            exit.SetInteractionEnabled(unlocked);
        }

        void Restore(CampaignCheckpoint checkpoint)
        {
            if (checkpoint == null || string.IsNullOrEmpty(checkpoint.state)) return;
            string[] parts = checkpoint.state.Split('|');
            if (parts.Length > 0) CampaignStateCodec.ReadBits(parts[0], fragments);
            if (parts.Length > 1) CampaignStateCodec.ReadInts(parts[1], entered);
            unlocked = parts.Length > 2 && parts[2] == "1";
        }
    }

    sealed class LogicLabPuzzle : CampaignPuzzleBase
    {
        readonly bool[] switches = new bool[4];
        readonly List<CampaignActionObject> switchActions = new List<CampaignActionObject>();
        GameObject gate;
        CampaignActionObject exit;
        bool solved;
        int failures;

        public override int CurrentStep { get { return solved ? 5 : 0; } }
        public override string SerializedState { get { return CampaignStateCodec.Bits(switches) + "|" + (solved ? "1" : "0"); } }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            Restore(checkpoint);
            World.CreateRoom(new Vector2(30f, 25f), 6f, new Color(0.06f, 0.13f, 0.15f), Definition.primaryColor * 0.36f);
            World.CreateWorldLabel("TitreLabo", "LABORATOIRE — DEDUCTION", new Vector3(0f, 4.7f, 12.1f), 0.27f, Color.white);

            for (int i = 0; i < 4; i++)
            {
                float x = -9f + i * 6f;
                World.CreateBlock("Paillasse_" + i, new Vector3(x, 0.8f, -1f), new Vector3(4.4f, 1.6f, 2.2f), new Color(0.16f, 0.20f, 0.22f));
                World.CreateBlock("Eprouvette_" + i, new Vector3(x, 2f, -1f), new Vector3(0.5f, 1.4f, 0.5f),
                    Color.Lerp(Definition.primaryColor, Definition.accentColor, i / 3f), PrimitiveType.Cylinder);
                CampaignActionObject action = World.CreateAction(this, "switch-" + i, "Interrupteur " + (i + 1),
                    new Vector3(x, 1.2f, 2.2f), new Vector3(1.4f, 1.4f, 0.5f), Color.gray);
                switchActions.Add(action);
            }

            World.CreateWorldLabel("ReglesLabo",
                "Regles : 1 et 3 partagent un etat.\n2 est l'oppose de 1. 4 suit 3.",
                new Vector3(0f, 3.3f, 5.6f), 0.22f, Color.white);
            World.CreateAction(this, "submit", "Valider la combinaison", new Vector3(0f, 1f, 5.8f),
                new Vector3(4.2f, 2f, 0.7f), Definition.accentColor);
            gate = World.CreateGate("SasLaboratoire", new Vector3(0f, 2.2f, 8.6f), new Vector3(5.2f, 4.4f, 0.55f));
            exit = World.CreateAction(this, "exit", "Sortir du laboratoire", new Vector3(0f, 1f, 10.5f),
                new Vector3(2f, 2f, 0.7f), Color.green * 0.65f);
            ApplyState();
            RefreshDisplay();
        }

        public override void HandleAction(string actionId)
        {
            if (actionId.StartsWith("switch-") && !solved)
            {
                int index = ParseIndex(actionId);
                switches[index] = !switches[index];
                switchActions[index].SetColor(switches[index] ? Color.green * 0.7f : Color.gray);
                switchActions[index].SetDisplayName("Interrupteur " + (index + 1) + " : " + (switches[index] ? "ON" : "OFF"));
                Level.PlayInteractionSound(switches[index] ? 1.15f : 0.85f);
                RefreshDisplay();
                SaveCheckpoint();
                return;
            }

            if (actionId == "submit" && !solved)
            {
                if (CampaignPuzzleRules.SwitchesMatch(switches, Definition.switchSolution))
                {
                    solved = true;
                    ApplyState();
                    Level.SetObjective("Le sas est ouvert : rejoins la sortie.");
                    Level.SetProgress("Combinaison logique validee");
                    Level.ShowMessage("Deduction correcte. Le sas du laboratoire est ouvert !", 6f);
                }
                else
                {
                    failures++;
                    string help = failures >= 2 ? " Indice : les interrupteurs 1, 3 et 4 sont identiques." : string.Empty;
                    Level.ShowMessage("La combinaison est incoherente." + help, 6f);
                }
                SaveCheckpoint();
                return;
            }

            if (actionId == "exit" && solved)
                Level.CompleteLevel();
        }

        void RefreshDisplay()
        {
            int active = 0;
            foreach (bool value in switches) if (value) active++;
            Level.SetProgress("Interrupteurs actifs : " + active + "/4");
        }

        void ApplyState()
        {
            for (int i = 0; i < switches.Length; i++)
            {
                switchActions[i].SetColor(switches[i] ? Color.green * 0.7f : Color.gray);
                switchActions[i].SetDisplayName("Interrupteur " + (i + 1) + " : " + (switches[i] ? "ON" : "OFF"));
                switchActions[i].SetInteractionEnabled(!solved);
            }
            gate.GetComponent<Collider>().enabled = !solved;
            gate.GetComponent<Renderer>().material.color = solved ? Color.green * 0.55f : Definition.accentColor * 0.65f;
            exit.SetInteractionEnabled(solved);
        }

        void Restore(CampaignCheckpoint checkpoint)
        {
            if (checkpoint == null || string.IsNullOrEmpty(checkpoint.state)) return;
            string[] parts = checkpoint.state.Split('|');
            CampaignStateCodec.ReadBits(parts[0], switches);
            solved = parts.Length > 1 && parts[1] == "1";
        }
    }

    sealed class SoundSequencePuzzle : CampaignPuzzleBase
    {
        readonly List<CampaignActionObject> pads = new List<CampaignActionObject>();
        TextMesh visualDisplay;
        CampaignActionObject exit;
        GameObject gate;
        int inputIndex;
        int playbackIndex = -1;
        float playbackTimer;
        bool playing;
        bool solved;

        public override int CurrentStep { get { return solved ? Definition.primarySequence.Length : inputIndex; } }
        public override string SerializedState { get { return solved ? "1" : "0"; } }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            solved = checkpoint != null && checkpoint.state == "1";
            World.CreateRoom(new Vector2(32f, 27f), 7f, new Color(0.11f, 0.06f, 0.13f), Definition.primaryColor * 0.34f);
            World.CreateWorldLabel("TitreAuditorium", "AUDITORIUM — MEMOIRE VISUELLE ET SONORE", new Vector3(0f, 5.4f, 13.1f), 0.24f, Color.white);

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (col == 2 || col == 3) continue;
                    World.CreateBlock("Fauteuil_" + row + "_" + col,
                        new Vector3(-10f + col * 4f, 0.65f, -5f + row * 2.6f),
                        new Vector3(1.5f, 1.3f, 1.4f), new Color(0.32f, 0.04f, 0.08f));
                }
            }

            World.CreateAction(this, "replay", "Rejouer la sequence", new Vector3(0f, 1f, 1.8f),
                new Vector3(3.8f, 2f, 0.7f), Definition.accentColor);
            visualDisplay = World.CreateWorldLabel("AffichageAccessible", "Appuie sur REJOUER", new Vector3(0f, 4.2f, 7.8f), 0.28f, Color.white);

            Color[] colors = { Color.red, Color.blue, Color.yellow, Color.green };
            string[] names = { "ROUGE", "BLEU", "JAUNE", "VERT" };
            for (int i = 0; i < 4; i++)
            {
                CampaignActionObject pad = World.CreateAction(this, "pad-" + i, "Pad " + names[i],
                    new Vector3(-6f + i * 4f, 0.35f, 6f), new Vector3(2.5f, 0.7f, 2.5f), colors[i]);
                pads.Add(pad);
            }

            gate = World.CreateGate("RideauSortie", new Vector3(0f, 2.5f, 10f), new Vector3(5.5f, 5f, 0.5f));
            exit = World.CreateAction(this, "exit", "Quitter la scene", new Vector3(0f, 1f, 11.7f),
                new Vector3(2.2f, 2f, 0.7f), Color.green * 0.65f);
            ApplySolvedState();
            Level.SetProgress(solved ? "Sequence reproduite" : "Sequence : 0/" + Definition.primarySequence.Length);
        }

        public override void Tick(float deltaTime)
        {
            if (!playing) return;
            playbackTimer -= deltaTime;
            if (playbackTimer > 0f) return;

            playbackIndex++;
            if (playbackIndex >= Definition.primarySequence.Length)
            {
                playing = false;
                playbackIndex = -1;
                visualDisplay.text = "A TOI — reproduis la sequence";
                SetPadsEnabled(true);
                return;
            }

            int value = Definition.primarySequence[playbackIndex];
            string[] names = { "ROUGE", "BLEU", "JAUNE", "VERT" };
            visualDisplay.text = "FLASH " + (playbackIndex + 1) + " : " + names[value];
            Level.PlayInteractionSound(0.75f + value * 0.18f);
            playbackTimer = 0.8f;
        }

        public override void HandleAction(string actionId)
        {
            if (actionId == "replay" && !solved)
            {
                playing = true;
                playbackIndex = -1;
                playbackTimer = 0.15f;
                inputIndex = 0;
                SetPadsEnabled(false);
                Level.SetProgress("Ecoute/lecture de la sequence...");
                return;
            }

            if (actionId.StartsWith("pad-") && !solved && !playing)
            {
                int selected = ParseIndex(actionId);
                SequenceStepResult result = CampaignPuzzleRules.AcceptSequenceStep(
                    Definition.primarySequence, inputIndex, selected);
                Level.PlayInteractionSound(0.75f + selected * 0.18f);

                if (result == SequenceStepResult.Incorrect)
                {
                    inputIndex = 0;
                    visualDisplay.text = "ERREUR — recommence depuis le debut";
                    Level.ShowMessage("Mauvaise note/couleur. La sequence repart au premier element.", 5f);
                }
                else
                {
                    inputIndex++;
                    visualDisplay.text = "CORRECT : " + inputIndex + "/" + Definition.primarySequence.Length;
                    if (result == SequenceStepResult.Completed)
                    {
                        solved = true;
                        ApplySolvedState();
                        Level.SetObjective("La sequence est validee : rejoins la sortie de scene.");
                        Level.ShowMessage("Sequence parfaite ! Le rideau de sortie s'ouvre.", 6f);
                    }
                }

                Level.SetProgress(solved ? "Sequence reproduite" : "Sequence : " + inputIndex + "/" + Definition.primarySequence.Length);
                SaveCheckpoint();
                return;
            }

            if (actionId == "exit" && solved)
                Level.CompleteLevel();
        }

        void SetPadsEnabled(bool value)
        {
            foreach (CampaignActionObject pad in pads) pad.SetInteractionEnabled(value && !solved);
        }

        void ApplySolvedState()
        {
            gate.GetComponent<Collider>().enabled = !solved;
            gate.GetComponent<Renderer>().material.color = solved ? Color.green * 0.55f : Definition.accentColor * 0.65f;
            exit.SetInteractionEnabled(solved);
            SetPadsEnabled(!solved);
            if (solved) visualDisplay.text = "SEQUENCE VALIDEE";
        }
    }

    sealed class OrientationCoursePuzzle : CampaignPuzzleBase
    {
        readonly List<CampaignActionObject> beacons = new List<CampaignActionObject>();
        GameObject gate;
        CampaignActionObject exit;
        int nextBeacon;
        float elapsed;
        bool timerStarted;
        bool solved;

        public override int CurrentStep { get { return nextBeacon; } }
        public override string SerializedState
        {
            get { return nextBeacon + "|" + elapsed.ToString("R", CultureInfo.InvariantCulture) + "|" + (solved ? "1" : "0"); }
        }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            Restore(checkpoint);
            World.CreateRoom(new Vector2(36f, 32f), 8f, new Color(0.07f, 0.17f, 0.11f), Definition.primaryColor * 0.32f);
            World.CreateWorldLabel("TitreGymnase", "GYMNASE — PARCOURS D'ORIENTATION", new Vector3(0f, 6.2f, 15.6f), 0.26f, Color.white);

            Vector3[] obstacles =
            {
                new Vector3(-7f, 1f, -3f), new Vector3(6f, 1.5f, -1f),
                new Vector3(-2f, 1f, 5f), new Vector3(9f, 1.2f, 8f), new Vector3(-10f, 1.2f, 9f)
            };
            for (int i = 0; i < obstacles.Length; i++)
                World.CreateBlock("ObstacleGym_" + i, obstacles[i], new Vector3(3.2f, 2f + (i % 2), 1.2f),
                    Color.Lerp(Definition.primaryColor, Definition.accentColor, i / 5f));

            Vector3[] positions =
            {
                new Vector3(-12f, 1.1f, -5f), new Vector3(11f, 1.1f, -2f),
                new Vector3(0f, 1.1f, 4f), new Vector3(-11f, 1.1f, 10f), new Vector3(10f, 1.1f, 12f)
            };
            string[] compass = { "NORD", "EST", "CENTRE", "OUEST", "ARRIVEE" };
            for (int i = 0; i < positions.Length; i++)
            {
                CampaignActionObject beacon = World.CreateAction(this, "beacon-" + i,
                    "Balise " + (i + 1) + " — " + compass[i], positions[i],
                    new Vector3(1.1f, 2.2f, 1.1f), i < nextBeacon ? Color.green : Definition.accentColor,
                    PrimitiveType.Cylinder);
                beacon.SetInteractionEnabled(i >= nextBeacon && !solved);
                beacons.Add(beacon);
            }

            gate = World.CreateGate("SortieGymnase", new Vector3(0f, 2.5f, 13.8f), new Vector3(5.5f, 5f, 0.55f));
            exit = World.CreateAction(this, "exit", "Terminer le parcours", new Vector3(0f, 1f, 15f),
                new Vector3(2.2f, 2f, 0.7f), Color.green * 0.65f);
            ApplySolvedState();
            RefreshObjective();
        }

        public override void Tick(float deltaTime)
        {
            if (timerStarted && !solved)
            {
                elapsed += deltaTime;
                RefreshProgress();
            }
        }

        public override void HandleAction(string actionId)
        {
            if (actionId.StartsWith("beacon-") && !solved)
            {
                int selected = ParseIndex(actionId);
                if (!CampaignPuzzleRules.IsExpectedOrderedTarget(nextBeacon, selected))
                {
                    Level.ShowMessage("Cette balise viendra plus tard. Cherche la balise " + (nextBeacon + 1) + ".");
                    return;
                }

                if (!timerStarted)
                    timerStarted = true;

                beacons[selected].SetColor(Color.green * 0.7f);
                beacons[selected].SetInteractionEnabled(false);
                nextBeacon++;
                Level.PlayInteractionSound(0.85f + selected * 0.08f);

                if (nextBeacon >= beacons.Count)
                {
                    solved = true;
                    ApplySolvedState();
                    Level.SetObjective("Toutes les balises sont activees : franchis la sortie.");
                    Level.ShowMessage("Parcours termine en " + FormatTime(elapsed) + ". Le temps etait facultatif.", 7f);
                }
                else
                {
                    beacons[nextBeacon].SetInteractionEnabled(true);
                    Level.ShowMessage("Balise validee. Prochaine cible : " + (nextBeacon + 1) + ".", 3f);
                }

                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId == "exit" && solved)
                Level.CompleteLevel();
        }

        void RefreshObjective()
        {
            if (!solved)
                Level.SetObjective("Active les balises dans l'ordre, sans penalite ni blocage.");
            RefreshProgress();
        }

        void RefreshProgress()
        {
            string timer = Definition.optionalTimer ? " • Temps indicatif : " + FormatTime(elapsed) : string.Empty;
            Level.SetProgress("Balises : " + nextBeacon + "/5" + timer);
        }

        void ApplySolvedState()
        {
            gate.GetComponent<Collider>().enabled = !solved;
            gate.GetComponent<Renderer>().material.color = solved ? Color.green * 0.55f : Definition.accentColor * 0.65f;
            exit.SetInteractionEnabled(solved);
        }

        static string FormatTime(float seconds)
        {
            return ((int)seconds / 60).ToString("00") + ":" + ((int)seconds % 60).ToString("00");
        }

        void Restore(CampaignCheckpoint checkpoint)
        {
            if (checkpoint == null || string.IsNullOrEmpty(checkpoint.state)) return;
            string[] parts = checkpoint.state.Split('|');
            int.TryParse(parts[0], out nextBeacon);
            if (parts.Length > 1) float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out elapsed);
            solved = parts.Length > 2 && parts[2] == "1";
            timerStarted = nextBeacon > 0 && !solved;
        }
    }

    sealed class ArtPatternsPuzzle : CampaignPuzzleBase
    {
        readonly List<CampaignActionObject> choices = new List<CampaignActionObject>();
        TextMesh instruction;
        GameObject gate;
        CampaignActionObject exit;
        int stage;
        int failures;
        bool solved;

        public override int CurrentStep { get { return stage; } }
        public override string SerializedState { get { return stage + "|" + (solved ? "1" : "0"); } }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            Restore(checkpoint);
            World.CreateRoom(new Vector2(32f, 27f), 7f, new Color(0.16f, 0.10f, 0.07f), Definition.primaryColor * 0.30f);
            World.CreateWorldLabel("TitreArts", "ARTS ET MUSIQUE — MOTIFS", new Vector3(0f, 5.4f, 13.1f), 0.28f, Color.white);

            for (int i = 0; i < 5; i++)
            {
                World.CreateBlock("Chevalet_" + i, new Vector3(-11f + i * 5.5f, 1.4f, -3f),
                    new Vector3(0.25f, 2.8f, 1.8f), new Color(0.34f, 0.18f, 0.06f));
            }

            instruction = World.CreateWorldLabel("InstructionMotif", string.Empty, new Vector3(0f, 4.2f, 5.8f), 0.24f, Color.white);
            PrimitiveType[] primitives = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Cylinder, PrimitiveType.Capsule };
            string[] names = { "CARRE BLEU", "CERCLE JAUNE", "COLONNE ROUGE", "ARC VERT" };
            Color[] colors = { Color.blue, Color.yellow, Color.red, Color.green };
            for (int i = 0; i < 4; i++)
            {
                CampaignActionObject choice = World.CreateAction(this, "pattern-" + i, names[i],
                    new Vector3(-7.5f + i * 5f, 1f, 7.5f), new Vector3(1.8f, 1.8f, 1.8f), colors[i], primitives[i]);
                choices.Add(choice);
            }

            gate = World.CreateGate("GalerieFinale", new Vector3(0f, 2.5f, 10.5f), new Vector3(5.5f, 5f, 0.55f));
            exit = World.CreateAction(this, "exit", "Entrer dans la galerie", new Vector3(0f, 1f, 12f),
                new Vector3(2.2f, 2f, 0.7f), Color.green * 0.65f);
            ApplySolvedState();
            RefreshStage();
        }

        public override void HandleAction(string actionId)
        {
            if (actionId.StartsWith("pattern-") && !solved)
            {
                int selected = ParseIndex(actionId);
                if (CampaignPuzzleRules.IsCorrectPatternChoice(stage, selected, Definition.primarySequence))
                {
                    choices[selected].SetColor(Color.white);
                    stage++;
                    failures = 0;
                    Level.PlayInteractionSound(0.95f + stage * 0.1f);
                    if (stage >= Definition.primarySequence.Length)
                    {
                        solved = true;
                        ApplySolvedState();
                        Level.SetObjective("Le motif complet ouvre la galerie : rejoins la sortie.");
                        Level.ShowMessage("Les trois motifs s'accordent. La galerie est ouverte !", 6f);
                    }
                    else
                    {
                        Level.ShowMessage("Etape correcte. Le motif suivant apparait.", 3f);
                    }
                }
                else
                {
                    failures++;
                    string contextual = failures >= 2 ? ContextHint() : "Observe la regle affichee avant de choisir.";
                    Level.ShowMessage("Ce motif ne convient pas. " + contextual, 6f);
                }

                RefreshStage();
                SaveCheckpoint();
                return;
            }

            if (actionId == "exit" && solved)
                Level.CompleteLevel();
        }

        void RefreshStage()
        {
            string[] instructions =
            {
                "ETAPE 1 — Choisis la forme sans angle.",
                "ETAPE 2 — Choisis la couleur opposee au rouge dans la fresque.",
                "ETAPE 3 — Termine le rythme : bas, haut, bas, ..."
            };
            instruction.text = solved ? "MOTIF COMPLET" : instructions[Mathf.Clamp(stage, 0, instructions.Length - 1)];
            Level.SetProgress("Motifs resolus : " + stage + "/3");
        }

        string ContextHint()
        {
            string[] hints =
            {
                "Une sphere n'a aucun angle.",
                "Sur le cercle chromatique simplifie, le vert repond au rouge.",
                "L'alternance demande maintenant la position haute : la colonne."
            };
            return hints[Mathf.Clamp(stage, 0, hints.Length - 1)];
        }

        void ApplySolvedState()
        {
            gate.GetComponent<Collider>().enabled = !solved;
            gate.GetComponent<Renderer>().material.color = solved ? Color.green * 0.55f : Definition.accentColor * 0.65f;
            exit.SetInteractionEnabled(solved);
            foreach (CampaignActionObject choice in choices) choice.SetInteractionEnabled(!solved);
        }

        void Restore(CampaignCheckpoint checkpoint)
        {
            if (checkpoint == null || string.IsNullOrEmpty(checkpoint.state)) return;
            string[] parts = checkpoint.state.Split('|');
            int.TryParse(parts[0], out stage);
            solved = parts.Length > 1 && parts[1] == "1";
        }
    }

    sealed class SecretCollegePuzzle : CampaignPuzzleBase
    {
        readonly bool[] fragments = new bool[3];
        readonly bool[] switches = new bool[3];
        readonly List<int> code = new List<int>();
        readonly List<CampaignActionObject> fragmentActions = new List<CampaignActionObject>();
        readonly List<CampaignActionObject> codeActions = new List<CampaignActionObject>();
        readonly List<CampaignActionObject> switchActions = new List<CampaignActionObject>();
        readonly List<CampaignActionObject> sequenceActions = new List<CampaignActionObject>();
        GameObject gateOne;
        GameObject gateTwo;
        CampaignActionObject switchSubmit;
        CampaignActionObject treasure;
        int phase;
        int sequenceIndex;

        public override int CurrentStep { get { return phase; } }
        public override string SerializedState
        {
            get
            {
                return phase + "|" + CampaignStateCodec.Bits(fragments) + "|" + CampaignStateCodec.Ints(code)
                    + "|" + CampaignStateCodec.Bits(switches) + "|" + sequenceIndex;
            }
        }

        protected override void Build(CampaignCheckpoint checkpoint)
        {
            Restore(checkpoint);
            World.CreateRoom(new Vector2(30f, 48f), 8f, new Color(0.025f, 0.035f, 0.055f), Definition.primaryColor * 0.24f);
            World.CreateWorldLabel("TitreSecret", "LE COLLEGE SECRET — EPREUVE FINALE", new Vector3(0f, 6.1f, 23.6f), 0.27f, Color.white);

            Vector3[] fragmentPositions =
            {
                new Vector3(-8f, 1f, -7f), new Vector3(8f, 1f, -3f), new Vector3(-3f, 2f, 0f)
            };
            for (int i = 0; i < 3; i++)
            {
                CampaignActionObject fragment = World.CreateAction(this, "final-fragment-" + i,
                    "Sceau secret " + (i + 1), fragmentPositions[i], new Vector3(0.9f, 0.9f, 0.25f), Definition.accentColor);
                fragmentActions.Add(fragment);
            }

            World.CreateBlock("ConsoleFinale", new Vector3(0f, 1f, 2f), new Vector3(8f, 2f, 0.8f), Definition.primaryColor * 0.6f);
            for (int digit = 0; digit <= 9; digit++)
            {
                CampaignActionObject action = World.CreateAction(this, "final-digit-" + digit, digit.ToString(),
                    new Vector3(-5.4f + (digit % 5) * 2.7f, digit < 5 ? 1.55f : 0.45f, 1.45f),
                    new Vector3(1.05f, 0.62f, 0.32f), new Color(0.16f, 0.27f, 0.44f));
                codeActions.Add(action);
            }
            gateOne = World.CreateGate("PorteSecreteUn", new Vector3(0f, 2.5f, 5f), new Vector3(6f, 5f, 0.55f));

            for (int i = 0; i < 3; i++)
            {
                CampaignActionObject action = World.CreateAction(this, "final-switch-" + i, "Relais " + (i + 1),
                    new Vector3(-5f + i * 5f, 1.1f, 9f), new Vector3(1.5f, 2.2f, 0.7f), Color.gray);
                switchActions.Add(action);
            }
            switchSubmit = World.CreateAction(this, "final-submit", "Synchroniser les relais", new Vector3(0f, 1f, 12f),
                new Vector3(4f, 2f, 0.7f), Definition.accentColor);
            gateTwo = World.CreateGate("PorteSecreteDeux", new Vector3(0f, 2.5f, 14.5f), new Vector3(6f, 5f, 0.55f));

            Color[] colors = { Color.red, Color.blue, Color.yellow, Color.green };
            for (int i = 0; i < 4; i++)
            {
                CampaignActionObject action = World.CreateAction(this, "final-sequence-" + i, "Symbole " + (i + 1),
                    new Vector3(-7.5f + i * 5f, 0.8f, 18f), new Vector3(2f, 1.6f, 2f), colors[i],
                    i % 2 == 0 ? PrimitiveType.Cylinder : PrimitiveType.Sphere);
                sequenceActions.Add(action);
            }

            treasure = World.CreateAction(this, "new-treasure", "Ouvrir le nouveau tresor", new Vector3(0f, 1.2f, 22f),
                new Vector3(3.2f, 2.4f, 2.1f), new Color(1f, 0.62f, 0.05f));
            World.CreateBlock("SocleTresor", new Vector3(0f, 0.35f, 22f), new Vector3(5f, 0.7f, 3.5f), new Color(0.16f, 0.16f, 0.18f));
            ApplyPhase();
            RefreshObjective();
        }

        public override void HandleAction(string actionId)
        {
            if (actionId.StartsWith("final-fragment-") && phase == 0)
            {
                int index = ParseIndex(actionId);
                if (fragments[index]) return;
                fragments[index] = true;
                fragmentActions[index].SetColor(Color.green * 0.65f);
                fragmentActions[index].SetInteractionEnabled(false);
                Level.PlayInteractionSound(0.85f + index * 0.12f);
                Level.ShowMessage("Sceau " + (index + 1) + " : chiffre " + Definition.primarySequence[index] + ".");
                if (CampaignStateCodec.CountTrue(fragments) == fragments.Length)
                {
                    phase = 1;
                    Level.ShowMessage("Les trois sceaux sont reunis. Entre leur code sur la console.", 6f);
                }
                ApplyPhase();
                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId.StartsWith("final-digit-") && phase == 1)
            {
                int digit = ParseIndex(actionId);
                int expected = code.Count;
                if (expected >= 3 || Definition.primarySequence[expected] != digit)
                {
                    code.Clear();
                    Level.ShowMessage("Code refuse. Les sceaux restent acquis, recommence la saisie.");
                }
                else
                {
                    code.Add(digit);
                    Level.PlayInteractionSound(0.8f + digit * 0.03f);
                    if (code.Count == 3)
                    {
                        phase = 2;
                        Level.ShowMessage("Premiere porte ouverte. Configure maintenant les trois relais.", 6f);
                    }
                }
                ApplyPhase();
                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId.StartsWith("final-switch-") && phase == 2)
            {
                int index = ParseIndex(actionId);
                switches[index] = !switches[index];
                switchActions[index].SetColor(switches[index] ? Color.green * 0.7f : Color.gray);
                switchActions[index].SetDisplayName("Relais " + (index + 1) + " : " + (switches[index] ? "ON" : "OFF"));
                Level.PlayInteractionSound(switches[index] ? 1.15f : 0.85f);
                SaveCheckpoint();
                return;
            }

            if (actionId == "final-submit" && phase == 2)
            {
                if (CampaignPuzzleRules.SwitchesMatch(switches, Definition.switchSolution))
                {
                    phase = 3;
                    Level.ShowMessage("Relais synchronises. Reproduis la suite des quatre symboles.", 6f);
                }
                else
                {
                    Level.ShowMessage("Synchronisation impossible. Indice : deux relais voisins sont actifs.", 6f);
                }
                ApplyPhase();
                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId.StartsWith("final-sequence-") && phase == 3)
            {
                int selected = ParseIndex(actionId);
                int[] sequence = FinalSequence();
                SequenceStepResult result = CampaignPuzzleRules.AcceptSequenceStep(sequence, sequenceIndex, selected);
                Level.PlayInteractionSound(0.75f + selected * 0.18f);
                if (result == SequenceStepResult.Incorrect)
                {
                    sequenceIndex = 0;
                    Level.ShowMessage("Suite incorrecte. Repars du premier symbole ; aucune phase precedente n'est perdue.", 6f);
                }
                else
                {
                    sequenceIndex++;
                    if (result == SequenceStepResult.Completed)
                    {
                        phase = 4;
                        Level.ShowMessage("Tous les mecanismes repondent. Le nouveau tresor est accessible !", 7f);
                    }
                }
                ApplyPhase();
                RefreshObjective();
                SaveCheckpoint();
                return;
            }

            if (actionId == "new-treasure" && phase == 4)
            {
                treasure.SetColor(Color.white);
                Level.CompleteLevel();
            }
        }

        void ApplyPhase()
        {
            for (int i = 0; i < fragmentActions.Count; i++)
            {
                fragmentActions[i].SetInteractionEnabled(phase == 0 && !fragments[i]);
                if (fragments[i]) fragmentActions[i].SetColor(Color.green * 0.65f);
            }
            foreach (CampaignActionObject action in codeActions) action.SetInteractionEnabled(phase == 1);

            bool firstOpen = phase >= 2;
            gateOne.GetComponent<Collider>().enabled = !firstOpen;
            gateOne.GetComponent<Renderer>().material.color = firstOpen ? Color.green * 0.5f : Definition.accentColor * 0.65f;

            for (int i = 0; i < switchActions.Count; i++)
            {
                switchActions[i].SetInteractionEnabled(phase == 2);
                switchActions[i].SetColor(switches[i] ? Color.green * 0.7f : Color.gray);
                switchActions[i].SetDisplayName("Relais " + (i + 1) + " : " + (switches[i] ? "ON" : "OFF"));
            }
            switchSubmit.SetInteractionEnabled(phase == 2);

            bool secondOpen = phase >= 3;
            gateTwo.GetComponent<Collider>().enabled = !secondOpen;
            gateTwo.GetComponent<Renderer>().material.color = secondOpen ? Color.green * 0.5f : Definition.accentColor * 0.65f;
            foreach (CampaignActionObject action in sequenceActions) action.SetInteractionEnabled(phase == 3);
            treasure.SetInteractionEnabled(phase == 4);
        }

        void RefreshObjective()
        {
            string[] objectives =
            {
                "Retrouve les trois sceaux caches du college secret.",
                "Entre le code donne par les sceaux, dans leur ordre.",
                "Configure les relais : les deux premiers actifs, le troisieme eteint.",
                "Reproduis la suite apprise a l'auditorium : 2, 4, 1, 3.",
                "Ouvre le nouveau tresor du college."
            };
            Level.SetObjective(objectives[Mathf.Clamp(phase, 0, objectives.Length - 1)]);
            Level.SetProgress("Epreuve finale — phase " + (phase + 1) + "/5");
        }

        int[] FinalSequence()
        {
            return new[]
            {
                Definition.primarySequence[3], Definition.primarySequence[4],
                Definition.primarySequence[5], Definition.primarySequence[6]
            };
        }

        void Restore(CampaignCheckpoint checkpoint)
        {
            if (checkpoint == null || string.IsNullOrEmpty(checkpoint.state)) return;
            string[] parts = checkpoint.state.Split('|');
            int.TryParse(parts[0], out phase);
            if (parts.Length > 1) CampaignStateCodec.ReadBits(parts[1], fragments);
            if (parts.Length > 2) CampaignStateCodec.ReadInts(parts[2], code);
            if (parts.Length > 3) CampaignStateCodec.ReadBits(parts[3], switches);
            if (parts.Length > 4) int.TryParse(parts[4], out sequenceIndex);
        }
    }

    static class CampaignStateCodec
    {
        public static string Bits(bool[] values)
        {
            char[] result = new char[values.Length];
            for (int i = 0; i < values.Length; i++) result[i] = values[i] ? '1' : '0';
            return new string(result);
        }

        public static void ReadBits(string value, bool[] destination)
        {
            if (string.IsNullOrEmpty(value)) return;
            int count = Mathf.Min(value.Length, destination.Length);
            for (int i = 0; i < count; i++) destination[i] = value[i] == '1';
        }

        public static string Ints(List<int> values)
        {
            return string.Join(",", values);
        }

        public static void ReadInts(string value, List<int> destination)
        {
            destination.Clear();
            if (string.IsNullOrEmpty(value)) return;
            foreach (string part in value.Split(','))
            {
                int parsed;
                if (int.TryParse(part, out parsed)) destination.Add(parsed);
            }
        }

        public static int CountTrue(bool[] values)
        {
            int count = 0;
            foreach (bool value in values) if (value) count++;
            return count;
        }
    }
}
