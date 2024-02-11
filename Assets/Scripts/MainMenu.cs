using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    // [SerializeField] private Button ANN;
    [SerializeField] private Button BallBalance;
    [SerializeField] private Button DodgeBall;
    [SerializeField] private Button Camouflag;
    [SerializeField] private Button FlappyBird;
    [SerializeField] private Button MazeWalkers;
    [SerializeField] private Button Movement;
    // [SerializeField] private Button Perceptron;
    [SerializeField] private Button Pong;
    [SerializeField] private Button AIDrive;
    [SerializeField] private Button StayOnPlatform;


    private void Awake() {
        // ANN.onClick.AddListener(()=> {
        //     Loader.Load(Loader.Scene.ANNScene);
        // });
        BallBalance.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.QBallBalanceScene);
        });
        DodgeBall.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.DodgeBallScene);
        });
        Camouflag.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.CamouflagScene);
        });
        FlappyBird.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.FlappyBirdScene);
        });
        MazeWalkers.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.MazeWalkersScene);
        });
        Movement.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.MovementScene);
        });
        // Perceptron.onClick.AddListener(()=> {
        //     Loader.Load(Loader.Scene.PerceptronScene);
        // });
        Pong.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.PongScene);
        });
        AIDrive.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.AIDriveScene);
        });
        StayOnPlatform.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.StayOnPlatformScene);
        });

        Time.timeScale = 1f;
    }
}
