using UnityEngine;
using static UnityEngine.Screen;

namespace AwesomeAchievements.GameClasses; 

internal static class GameScreen {
    private static Resolution _lastRes;

    static GameScreen() => _lastRes = currentResolution;

    public static bool ResolutionChanged() {
        if (_lastRes.width == currentResolution.width && _lastRes.height == currentResolution.height) return false;
        else {
            _lastRes = currentResolution;
            return true;
        }
    }
}