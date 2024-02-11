using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Brain : MonoBehaviour {
    private _ANN ann;
    private double sumSquaredError = 0;

    void Start() {
        int numInputs=2;
        int numOutputs=1;
        int numHiddenLayers=1;
        int numNeuronsPerLayer=2;
        double alpha=0.9f;
        int epochs=1000;
        ann = new _ANN(numInputs,numOutputs,numHiddenLayers,numNeuronsPerLayer,alpha);

        List<double> result;

        for(int i=0;i<epochs;i++){
            sumSquaredError=0;
            result = Train(1,1,0);
            sumSquaredError += Mathf.Pow((float) result[0]-0,2);
            result = Train(1,0,1);
            sumSquaredError += Mathf.Pow((float) result[0]-1,2);
            result = Train(0,1,1);
            sumSquaredError += Mathf.Pow((float) result[0]-1,2);
            result = Train(0,0,0);
            sumSquaredError += Mathf.Pow((float) result[0]-0,2);
        }
        Debug.Log("SSE: "+sumSquaredError);

        result = Train(1,1,0);
        Debug.Log("1 1 "+result[0]);
        result = Train(0,1,1);
        Debug.Log("0 1 "+result[0]);
        result = Train(1,0,1);
        Debug.Log("1 0 "+result[0]);
        result = Train(0,0,0);
        Debug.Log("0 0 "+result[0]);
    }

    private List<double> Train(int i1, int i2, int o) {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(i1);
        inputs.Add(i2);
        outputs.Add(o);
        return ann.Run(inputs,outputs);
    }
}
