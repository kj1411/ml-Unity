using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingLayer {

	public int numNeurons;
	public List<RacingNeuron> neurons = new List<RacingNeuron>();

	public RacingLayer(int nNeurons, int numNeuronInputs)
	{
		numNeurons = nNeurons;
		for(int i = 0; i < nNeurons; i++)
		{
			neurons.Add(new RacingNeuron(numNeuronInputs));
		}
	}
}
