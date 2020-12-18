using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xor : MonoBehaviour
{
    float[,] dataInput =
        {
        {0, 0},
        {1, 0},
        {0, 1},
        {1, 1}
        };

    float[] ExpectedResults = { 0, 1, 1, 0 };

    Neuron H1 = new Neuron();
    Neuron H2 = new Neuron();
    Neuron OutputNeuron = new Neuron();

    // Start is called before the first frame update
    void Start()
    {
        H1.randomizeWeights();
        H2.randomizeWeights();
        OutputNeuron.randomizeWeights();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            for (int i = 0; i < 9999; i++)
            {
                Train();
            }
            
        }
    }

    void Train()
    {
        for (int i = 0; i < 4; i++)
        {
            H1.inputs = new float[] { dataInput[i, 0], dataInput[i, 1] };
            H1.inputs = new float[] { dataInput[i, 0], dataInput[i, 1] };

            OutputNeuron.inputs = new float[] { H1.output, H2.output };

            Debug.Log(dataInput[i, 0] + " xor " + dataInput[i, 1]  + " = " + OutputNeuron.output);

            OutputNeuron.error = sigmoid.derivative(OutputNeuron.output) * (ExpectedResults[i] - OutputNeuron.output);
            OutputNeuron.AdjustWeights();

            H1.error = sigmoid.derivative(H1.output) * OutputNeuron.error * OutputNeuron.weights[0];
            H2.error = sigmoid.derivative(H2.output) * OutputNeuron.error * OutputNeuron.weights[1];
        }
        if(OutputNeuron.error == 0f)
        {
            Debug.LogWarning("SUCCESS!");
        }
    }

}

class Neuron
{
    public float[] inputs = new float[2];
    public float[] weights = new float[2];
    public float error;

    float biasWeight;

    Random ran = new Random();

    public void randomizeWeights()
    {
        biasWeight = Random.Range(0.0f, 1.0f);

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(0.0f, 1.0f);
        }
    }

    public float output
    {
        get { return sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1] + biasWeight); }
    }

    public void AdjustWeights()
    {
        weights[0] += error * inputs[0];
        weights[1] += error * inputs[0];
        biasWeight += error;
    }
}

class sigmoid
{
    public static float output(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public static float derivative(float x)
    {
        return x * (1 - x);
    }
}
