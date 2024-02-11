using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Neuron : MonoBehaviour{
    public int numInputs;
    public double bias;
    public double output;
    public double errorGradient;
    public List<double> inputs = new List<double>();
    public List<double> weights = new List<double>();

    public _Neuron(int numInputs){
        bias = Random.Range(-1.0f,1.0f);
        this.numInputs = numInputs;
        for(int i=0;i<numInputs;i++){
            weights.Add(Random.Range(-1.0f,1.0f));
        }
    }
}
