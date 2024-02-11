using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Drive : MonoBehaviour { 

    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;
    public float visibleDistance = 50.0f;
    List<string> collectedTrainingData = new List<string>();
    List<string> collectedOutputs = new List<string>();
    StreamWriter trainingDataFile;

    private void Start() {
        string path = Application.dataPath + "/_Assets/AIDriveAssets/trainingData.txt";
        trainingDataFile = File.CreateText(path);
    }

    private void OnApplicationQuit() {
        for(int i=0;i<collectedTrainingData.Count;i++){
            string temp = collectedTrainingData[i] + collectedOutputs[i];
            trainingDataFile.WriteLine(temp);
        }
        trainingDataFile.Close();
    }

    float Round(float x){
        return (float) System.Math.Round(x,System.MidpointRounding.AwayFromZero)/2.0f;
    }

    private void Update() {
        float translationInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");
        float translation = translationInput*speed*Time.deltaTime;
        float rotation = rotationInput*rotationSpeed*Time.deltaTime;
        transform.Translate(0,0,translation);
        transform.Rotate(0,rotation,0);

        Debug.DrawRay(transform.position,this.transform.forward*(visibleDistance),Color.red);
        Debug.DrawRay(transform.position,this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,-this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,Quaternion.AngleAxis(-45,Vector3.up)*this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,Quaternion.AngleAxis(45,Vector3.up)*-this.transform.right*visibleDistance,Color.red);

        RaycastHit hit;
        float frontDist=0,
                rightDist=0,
                leftDist=0,
                right45Dist=0,
                left45Dist=0;

        if(Physics.Raycast(transform.position,this.transform.forward,out hit,visibleDistance)){
            frontDist = 1- Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,this.transform.right,out hit,visibleDistance)){
            rightDist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,-this.transform.right,out hit,visibleDistance)){
            leftDist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,Quaternion.AngleAxis(-45,Vector3.up)*this.transform.right,out hit,visibleDistance)){
            right45Dist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,Quaternion.AngleAxis(45,Vector3.up)*-this.transform.right,out hit,visibleDistance)){
            left45Dist = 1 - Round(hit.distance/visibleDistance);
        }


        string trainingData = frontDist + "," + 
                    rightDist + "," +
                    leftDist + "," +
                    right45Dist + "," +
                    left45Dist;
        string outputData = "," +
                    Round(translationInput) + "," +
                    Round(rotationInput);
        if(!collectedTrainingData.Contains(trainingData)){
            collectedTrainingData.Add(trainingData);
            collectedOutputs.Add(outputData);
        }
    }

}
