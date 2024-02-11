using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ANN :MonoBehaviour{
    public int numInputs;
    public int numOutputs;
    public int numHiddenLayers;
    public int numNeuronsPerLayer;
    public double alpha;
    List<_Layer> layers = new List<_Layer>();

    public _ANN(int numInputs,int numOutputs,int numHiddenLayers,int numNeuronsPerLayer,double alpha){
        this.numInputs = numInputs;
        this.numOutputs = numOutputs;
        this.numHiddenLayers = numHiddenLayers;
        this.numNeuronsPerLayer = numNeuronsPerLayer;
        this.alpha = alpha;

        if(numHiddenLayers > 0){
            layers.Add(new _Layer(numNeuronsPerLayer,numInputs));
            for(int i=0;i<numHiddenLayers-1;i++) {
                layers.Add(new _Layer(numNeuronsPerLayer,numNeuronsPerLayer));
            }
            layers.Add(new _Layer(numOutputs,numNeuronsPerLayer));
        }
        else {
            layers.Add(new _Layer(numOutputs,numInputs));
        }
    }

    public List<double> Run(List<double> inputValues,List<double> desiredOutput) {
        List<double> outputs = new List<double>();

        if(inputValues.Count != numInputs){
            Debug.Log("Please provide "+numInputs+" inputs");
            return outputs;
        }

        List<double> inputs = new List<double>(inputValues);
        for(int i=0;i<=numHiddenLayers;i++){
            if(i>0){
                inputs = new List<double>(outputs);
            }
            outputs.Clear();
            _Layer layer=layers[i];
            for(int j=0;j<layer.numNeurons;j++){
                double N=0; // weight*input
                _Neuron neuron = layers[i].neurons[j];
                neuron.inputs.Clear();
                for(int k=0;k<neuron.numInputs;k++){
                    neuron.inputs.Add(inputs[k]);
                    N+=neuron.weights[k]*inputs[k];
                }
                N-=neuron.bias;
                neuron.output = ActivationFunction(N);
                outputs.Add(neuron.output);
            }
        }
        UpdateWeights(outputs,desiredOutput);   
        return outputs;
    }

    private double ActivationFunction(double n){
        return Tanh(n);
    }

    private double Sigmoid(double n){
        double k=System.Math.Exp(n);
        return k/(1.0f+k);
    }

    private double Tanh(double n){
        double k=2*System.Math.Exp(n);
        return (k-1)/(k+1);
    }

    private double Relu(double n){
        if(n<0) return 0;
        else return n;
    }
    private double LeakyRelu(double n){
        if(n<0) return 0.001*n;
        else return n;
    }

    private void UpdateWeights(List<double> outputs, List<double> desiredOutput){
        double error;
        for(int i=numHiddenLayers;i>=0;i--){
            _Layer layer = layers[i];
            for(int j=0;j<layer.numNeurons;j++){
                _Neuron neuron = layer.neurons[j];
                if(i==numHiddenLayers){
                    error = desiredOutput[j] - outputs[j];
                    neuron.errorGradient = outputs[j] * (1-outputs[j]) * error;
                }
                else{
                    double errorGradientSum=0;
                    for(int p=0;p<layers[i+1].numNeurons;p++){
                        errorGradientSum += layers[i+1].neurons[p].errorGradient * layers[i+1].neurons[p].weights[j];
                    }
                    neuron.errorGradient = neuron.output*(1-neuron.output)*errorGradientSum;
                }

                for(int k=0;k<neuron.numInputs;k++){
                    if(i==numHiddenLayers){
                        error = desiredOutput[j] - outputs[j];
                        neuron.weights[k] += alpha*neuron.inputs[k]*error;
                    }
                    else{
                        neuron.weights[k] += alpha*neuron.inputs[k]*neuron.errorGradient;
                    }
                }
                neuron.bias -= alpha*neuron.errorGradient;
            }
        }
    }
}
