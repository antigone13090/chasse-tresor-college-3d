#!/bin/bash
set -e

PROJECT="$(cd "$(dirname "$0")" && pwd)"

UNITY="/Applications/Unity/Hub/Editor/6000.3.2f1/Unity.app/Contents/MacOS/Unity"

mkdir -p "$PROJECT/Logs"
mkdir -p "$PROJECT/Builds/macOS"
mkdir -p "$PROJECT/Releases"

echo
echo "================================================"
echo " CHASSE AU TRESOR - BUILD MAC INTEL"
echo "================================================"
echo

if [ ! -x "$UNITY" ]; then
    echo "Unity 6000.3.2f1 n'est pas installé à l'emplacement attendu."
    echo
    echo "Installer Unity 6000.3.2f1 avec Unity Hub sur le Mac,"
    echo "puis relancer build-mac.command."
    exit 1
fi

echo "1/3 - Validation du jeu"

"$UNITY" -batchmode -quit -projectPath "$PROJECT" -executeMethod CollegeGameValidator.Run -logFile "$PROJECT/Logs/mac-validation.log"

cat "$PROJECT/Logs/indice-tests.txt"

if grep -q "FAIL : 0" "$PROJECT/Logs/indice-tests.txt"; then
    echo
    echo "Validation : OK"
else
    echo
    echo "ERREUR : le jeu n'a pas passé les tests."
    exit 1
fi

echo
echo "2/3 - Build macOS Intel"

"$UNITY" -batchmode -quit -projectPath "$PROJECT" -executeMethod CollegeDesktopBuild.BuildMacIntel -logFile "$PROJECT/Logs/mac-build.log"

echo
echo "3/3 - Résultat"

grep -E "BUILD OK|BUILD RESULT|BUILD ECHEC|MODULE MACOS" "$PROJECT/Logs/mac-build.log" | tail -20

if [ -d "$PROJECT/Builds/macOS/ChasseAuTresorCollege.app" ]; then
    echo
    echo "Application créée :"
    echo "$PROJECT/Builds/macOS/ChasseAuTresorCollege.app"

    cd "$PROJECT/Builds/macOS"

    ditto -c -k --sequesterRsrc --keepParent "ChasseAuTresorCollege.app" "$PROJECT/Releases/ChasseAuTresorCollege-macOS-Intel-v1.0.zip"

    echo
    echo "Archive créée :"
    echo "$PROJECT/Releases/ChasseAuTresorCollege-macOS-Intel-v1.0.zip"
else
    echo
    echo "ERREUR : application macOS non créée."
    exit 1
fi
