using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLayer {

	public int numNeurons;
	public List<QNeuron> neurons = new List<QNeuron>();

	public QLayer(int nNeurons, int numNeuronInputs)
	{
		numNeurons = nNeurons;
		for(int i = 0; i < nNeurons; i++)
		{
			neurons.Add(new QNeuron(numNeuronInputs));
		}
	}
}
