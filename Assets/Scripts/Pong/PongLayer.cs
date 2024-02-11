using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongLayer {

	public int numNeurons;
	public List<PongNeuron> neurons = new List<PongNeuron>();

	public PongLayer(int nNeurons, int numNeuronInputs)
	{
		numNeurons = nNeurons;
		for(int i = 0; i < nNeurons; i++)
		{
			neurons.Add(new PongNeuron(numNeuronInputs));
		}
	}
}
