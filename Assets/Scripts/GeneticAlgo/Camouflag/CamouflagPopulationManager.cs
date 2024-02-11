using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamouflagPopulationManager : MonoBehaviour {

    [SerializeField] private GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> population;
    public static float elapsedTime = 0;
    private int breedingTime = 15;
    private int generation = 1;
    private readonly float topEdge = 7f, bottomEdge=-7f, leftEdge=-13f, rightEdge = 13f, zDir = 0;

    GUIStyle populationStates = new GUIStyle();

    private void OnGUI() {
        populationStates.fontSize = 50;
        populationStates.normal.textColor = Color.white;
        GUI.Label(new Rect(10,10,100,20), "Generation: "+ generation, populationStates);
        GUI.Label(new Rect(10,65,100,20), "Time Elapsed: "+ (int)elapsedTime, populationStates);
        GUI.Label(new Rect(10,110,190,20), "Press person according to preference");
    }

    private void Awake() {
        population = new List<GameObject>();
    }

    private void Start() {
        for(int i=0; i<populationSize; i++){
            Vector3 spawnedPosition = new(Random.Range(leftEdge,rightEdge),Random.Range(topEdge,bottomEdge),zDir);
            GameObject spawnedPerson = Instantiate(personPrefab,spawnedPosition , Quaternion.identity);
            CamouflagDNA dna = spawnedPerson.GetComponent<CamouflagDNA>();
            dna.r = Random.Range(0f,1f);
            dna.g = Random.Range(0f,1f);
            dna.b = Random.Range(0f,1f);
            population.Add(spawnedPerson);
        }
    }

    private void Update() {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Loader.Load(Loader.Scene.MainMenuScene);
		}

        elapsedTime += Time.deltaTime;
        if(elapsedTime > breedingTime) {
            BreedNewPopulation();
            elapsedTime = 0;
        }
    }

    private GameObject Breed(GameObject parent1, GameObject parent2) {
        Vector3 spawnedPosition = new(Random.Range(leftEdge,rightEdge),Random.Range(topEdge,bottomEdge),zDir);
        GameObject offspring = Instantiate(personPrefab, spawnedPosition , Quaternion.identity);
        CamouflagDNA dna1 = parent1.GetComponent<CamouflagDNA>();
        CamouflagDNA dna2 = parent2.GetComponent<CamouflagDNA>();

        CamouflagDNA offspringDNA = offspring.GetComponent<CamouflagDNA>();
        if(Random.Range(0,10000) > 5) {
            offspringDNA.r = Random.Range(0,10) < 5 ? dna1.r : dna2.r;
            offspringDNA.g = Random.Range(0,10) < 5 ? dna1.g : dna2.g;
            offspringDNA.b = Random.Range(0,10) < 5 ? dna1.b : dna2.b;
        } else {
            offspringDNA.r = Random.Range(0f,1f);
            offspringDNA.g = Random.Range(0f,1f);
            offspringDNA.b = Random.Range(0f,1f);
        }

        return offspring;
    }

    private void BreedNewPopulation() {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedPopulation = population.OrderBy(o => o.GetComponent<CamouflagDNA>().timeToDie).ToList();

        population.Clear();
        int listSize = sortedPopulation.Count;
        for(int i=(int)(listSize*0.4f); i<listSize-1; i++){
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        //Destroy parent population
        foreach(GameObject person in sortedPopulation) {
            Destroy(person);
        }

        generation++;
    }
}
