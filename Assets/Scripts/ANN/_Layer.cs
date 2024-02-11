using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Layer:MonoBehaviour {

    public int numNeurons;
    public List<_Neuron> neurons = new List<_Neuron>();

    public _Layer(int numNeurons,int numNeuronInputs) {
        this.numNeurons = numNeurons;
        for(int i=0;i<numNeurons;i++){
            neurons.Add(new _Neuron(numNeuronInputs));
        }
    }
}
