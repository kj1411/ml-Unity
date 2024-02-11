using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        MainMenuScene,
        ANNScene,
        QBallBalanceScene,
        DodgeBallScene,
        FlappyBirdScene,
        MazeWalkersScene,
        CamouflagScene,
        MovementScene,
        PerceptronScene,
        PongScene,
        AIDriveScene,
        StayOnPlatformScene,
    }

    public static void Load(Scene targetScene) {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
