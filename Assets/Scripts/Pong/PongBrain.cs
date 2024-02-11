using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBrain : MonoBehaviour {

    private const string BACKWALL = "backwall";
    private const string TOPS = "tops";

    public GameObject paddle;
    public GameObject ball;
    Rigidbody2D ballRB;
    float yVelocity;
    float paddleMinY = 8.8f;
    float paddleMaxY = 17.4f;
    float paddleMaxSpeed =15;
    public float numSaved = 0;
    public float numMissed = 0;

    PongANN ann;

    private void Start() {
        ann= new PongANN(6,1,1,4,0.11);
        ballRB = ball.GetComponent<Rigidbody2D>();
    }

    List<double> Run(double ballPositionX,double ballPositionY,double ballVelocityX,double ballVelocityY,double paddlePositionX,double paddlePositionY,double paddleVelocity,bool train){
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(ballPositionX);
        inputs.Add(ballPositionY);
        inputs.Add(ballVelocityX);
        inputs.Add(ballVelocityY);
        inputs.Add(paddlePositionX);
        inputs.Add(paddlePositionY);

        outputs.Add(paddleVelocity);

        if(train){
            return ann.Train(inputs,outputs);
        }
        else return ann.CalcOutput(inputs,outputs);
    }

    private void Update() {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Loader.Load(Loader.Scene.MainMenuScene);
		}

        float positionY = Mathf.Clamp(paddle.transform.position.y + (yVelocity*Time.deltaTime*paddleMaxSpeed),paddleMinY,paddleMaxY);
        paddle.transform.position = new Vector3(paddle.transform.position.x,positionY,paddle.transform.position.z);

        List<double> output = new List<double>();
        int layerMask = 1<<9;
        int raycastLength = 1000;
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position,ballRB.velocity,raycastLength,layerMask);

        if(hit.collider){
            if(hit.collider.gameObject.tag == TOPS) {
                Vector3 reflection = Vector3.Reflect(ballRB.velocity,hit.normal);
                hit = Physics2D.Raycast(hit.point,reflection,raycastLength,layerMask);
            }
            if(hit.collider?.gameObject.tag == BACKWALL){
                float dy = hit.point.y - paddle.transform.position.y;

                output = Run(ball.transform.position.x,
                                ball.transform.position.y,
                                ballRB.velocity.x,
                                ballRB.velocity.y,
                                paddle.transform.position.x,
                                paddle.transform.position.y,
                                dy,
                                true);
                yVelocity = (float)output[0];
            }
        }
        else yVelocity = 0;
    }
}
