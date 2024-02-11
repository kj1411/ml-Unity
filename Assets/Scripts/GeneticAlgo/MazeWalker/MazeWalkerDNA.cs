using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWalkerDNA  {
    public List<int> genes = new List<int>();
    private int dnaLength = 0;
    private int maxValue = 0;

    public MazeWalkerDNA(int l,int v) {
        dnaLength = l;
        maxValue = v;
        SetRandom();
    }

    public void SetRandom() {
        genes.Clear();
        for(int i=0;i< dnaLength;i++) {
            genes.Add(Random.Range(0,maxValue));
        }
    }

    public void SetInt(int pos,int value) {
        genes[pos] = value;
    }

    public void Combine(MazeWalkerDNA d1,MazeWalkerDNA d2) {
        for(int i=0;i< dnaLength;i++) {
            if(i< dnaLength/2.0f) {
                genes[i] = d1.genes[i];
            } else{
                genes[i] = d2.genes[i];
            }
        }
    }

    public void Mutate() {
        genes[Random.Range(0,dnaLength)] = Random.Range(0,maxValue);
    }

    public int GetGene(int pos){
        return genes[pos];
    }
}
