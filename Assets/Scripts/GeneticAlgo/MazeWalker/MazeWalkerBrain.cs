using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWalkerBrain : MonoBehaviour {
    
    private const string DEAD = "dead";
    private const string WALL = "wall";

    private int DNALength = 2;
    public MazeWalkerDNA dna;
    public GameObject eyes;
    private bool seeWall = true;
    Vector3 startPos;
    public float distanceTravelled = 0;
    private bool alive = true;

    public void Init() {
        //0 forward
        //1 Angle turn
        dna= new MazeWalkerDNA(DNALength,360);
        startPos = this.transform.position;
    }

    private void OnCollisionEnter(Collision obj) {
        if(obj.gameObject.CompareTag(DEAD)) {
            alive = false;
            distanceTravelled=0;
            Debug.Log("dead");
            gameObject.SetActive(false);
        }
    }

    // private void OnControllerColliderHit(ControllerColliderHit hit) {
    //     if(hit.gameObject.CompareTag(DEAD)) {
    //         alive=false;
    //         distanceTravelled=0;
    //         gameObject.SetActive(false);
    //     }
    // }

    private void Update() {

        if(!alive) return;

        seeWall=false;
        Debug.DrawRay(eyes.transform.position,eyes.transform.forward*0.5f,Color.red);
        if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out RaycastHit hit, 0.5f)) {
            if(hit.collider.gameObject.CompareTag(WALL)){
                seeWall=true;
            }
        }
    }

    private void FixedUpdate() {
        if(!alive) return;

        float h=0;
        float v=dna.GetGene(0);

        if(seeWall){
            h=dna.GetGene(1);
        }

        this.transform.Translate(0,0,v*0.001f);
        this.transform.Rotate(0,h,0);
        distanceTravelled = Vector3.Distance(startPos,this.transform.position);
    }
}
