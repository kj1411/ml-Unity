using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeWalkerPopulationManager : MonoBehaviour {
    
    public GameObject botPrefab;
    public GameObject spawningPoint;
    public GameObject targetPoint;
    public int populationSize = 50;
    private List<GameObject> population;
    public static float elapsedTime = 0;
    public float trialTime = 5;
    private int generation = 1;

    
    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI() {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10,10,250,150));
        GUI.Box(new Rect(0,0,140,140),"States",guiStyle);
        GUI.Label(new Rect(10,25,200,30),"Gen: "+generation,guiStyle);
        GUI.Label(new Rect(10,50,200,30),string.Format("Time: {0:0.00} ", elapsedTime),guiStyle);
        GUI.Label(new Rect(10,75,200,30),"Population: "+population.Count,guiStyle);
        GUI.EndGroup();
    }

    private void Awake() {
        population = new List<GameObject>();
    }

    private void Start() {
        for(int i=0;i<populationSize;i++) {
            GameObject bot = Instantiate(botPrefab,spawningPoint.transform.position,this.transform.rotation);
            bot.transform.parent = this.transform;
            bot.GetComponent<MazeWalkerBrain>().Init();
            population.Add(bot);
        }
    }

    private GameObject Breed(GameObject parent1, GameObject parent2) {
        
        GameObject offspring = Instantiate(botPrefab,spawningPoint.transform.position,this.transform.rotation);
        offspring.transform.parent = this.transform;
        MazeWalkerBrain brain = offspring.GetComponent<MazeWalkerBrain>();
        brain.Init();
        if(Random.Range(0,1000) == 1){
            brain.dna.Mutate();
        } else {
            brain.dna.Combine(parent1.GetComponent<MazeWalkerBrain>().dna,parent2.GetComponent<MazeWalkerBrain>().dna);
        }
        return offspring;
    }

    private void BreedNewPopulation() {
        List<GameObject> sortedPopulation = population.OrderBy(o =>Vector3.Distance(targetPoint.transform.position , o.transform.position)).ToList();
        population.Clear();

        int listSize = sortedPopulation.Count;
        for(int i=(int)(listSize*0.4f)-1; i<listSize-1; i++){
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        //Destroy parent population
        foreach(GameObject person in sortedPopulation) {
            Destroy(person);
        }
        generation++;
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Escape))
		{
			Loader.Load(Loader.Scene.MainMenuScene);
		}    
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= trialTime){
            BreedNewPopulation();
            elapsedTime=0;
        }
    }
}
