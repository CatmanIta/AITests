using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.NN
{ 

    public class NeuralNetwork : MonoBehaviour
    {
        public float learningRate = 0.5f;
        public int iterations = 100;

        private void Awake()
        {
            float[] inputs = {0.05f, 0.1f};
            float[] weights1 = {0.15f, 0.20f, 0.25f, 0.30f};
            float bias1 = 0.35f;
            float[] weights2 = { 0.40f, 0.45f, 0.50f, 0.55f };
            float bias2 = 0.60f;
            float[] outputs = {0.01f, 0.99f};

            for (int i = 0; i < iterations; i++)
            {
                Debug.Log("Iteration: " + i);
                // Feed forward
                float s1 = inputs[0] * weights1[0] + inputs[1] * weights1[1] + bias1;
                float h1 = Sigmoid(s1);
                float s2 = inputs[0] * weights1[2] + inputs[1] * weights1[3] + bias1;
                float h2 = Sigmoid(s2);
                //Debug.Log("h1: " + h1);
                //Debug.Log("h2: " + h2);

                float z1 = h1 * weights2[0] + h2 * weights2[1] + bias2;
                float o1 = Sigmoid(z1);
                float z2 = h1 * weights2[2] + h2 * weights2[3] + bias2;
                float o2 = Sigmoid(z2);
                //Debug.Log("o1: " + o1);
                //Debug.Log("o2: " + o2);

                // Error
                float e1 = (1 / 2f) * (o1 - outputs[0]) * (o1 - outputs[0]);
                float e2 = (1 / 2f) * (o2 - outputs[1]) * (o2 - outputs[1]);
                //float etot = e1 + e2;
                Debug.Log("e1: " + e1);
                Debug.Log("e2: " + e2);

                // Error Delta: output layer
                float d_etot_o1 = -(outputs[0] - o1);
                //Debug.Log("d_etot_o1: " + d_etot_o1);
                float d_o1_z1 = o1 * (1 - o1);
                //Debug.Log("d_o1_z1: " + d_o1_z1);
                float d_z1_w5 = h1;
                //Debug.Log("d_z1_w1: " + d_z1_w5);
                float d_etot_w5 = d_etot_o1 * d_o1_z1 * d_z1_w5;

                float d_z1_w6 = h2;
                //Debug.Log("d_z1_w6: " + d_z1_w6);
                float d_etot_w6 = d_etot_o1 * d_o1_z1 * d_z1_w6;

                float d_etot_o2 = -(outputs[1] - o2);
                //Debug.Log("d_etot_o2: " + d_etot_o2);
                float d_o2_z2 = o2 * (1 - o2);
                //Debug.Log("d_o2_z2: " + d_o2_z2);
                float d_z2_w7 = h1;
                //Debug.Log("d_z2_w7: " + d_z2_w7);
                float d_etot_w7 = d_etot_o2 * d_o2_z2 * d_z2_w7;

                float d_z2_w8 = h2;
                //Debug.Log("d_z2_w8: " + d_z2_w8);
                float d_etot_w8 = d_etot_o2 * d_o2_z2 * d_z2_w8;

                // Error Delta: hidden layer
                float d_z1_h1 = weights2[0]; // w5
                float d_e1_o1 = d_etot_o1; // it's the same!
                float d_e1_h1 = d_e1_o1 * d_o1_z1 * d_z1_h1;
                //Debug.Log("d_e1_h1: " + d_e1_h1);

                float d_z2_h1 = weights2[2]; // w7
                float d_e2_o2 = d_etot_o2; // it's the same!
                float d_e2_h1 = d_e2_o2 * d_o2_z2 * d_z2_h1;
                //Debug.Log("d_e2_h1: " + d_e2_h1);

                float d_etot_h1 = d_e1_h1 + d_e2_h1;
                //Debug.Log("d_etot_h1: " + d_etot_h1);

                float d_h1_s1 = s1 * (1 - s1);
                float d_etot_w1 = d_etot_h1 * d_h1_s1 * inputs[0];
                //Debug.Log("d_etot_w1: " + d_etot_w1);
                float d_etot_w2 = d_etot_h1 * d_h1_s1 * inputs[1];
                //Debug.Log("d_etot_w2: " + d_etot_w2);


                float d_z1_h2 = weights2[1]; // w6
                float d_e1_h2 = d_e1_o1 * d_o1_z1 * d_z1_h2;
                //Debug.Log("d_e1_h2: " + d_e1_h2);

                float d_z2_h2 = weights2[3]; // w8
                float d_e2_h2 = d_e2_o2 * d_o2_z2 * d_z2_h2;
                //Debug.Log("d_e2_h2: " + d_e2_h2);

                float d_etot_h2 = d_e1_h2 + d_e2_h2;
                //Debug.Log("d_etot_h2: " + d_etot_h2);

                float d_h2_s2 = s2 * (1 - s2);
                float d_etot_w3 = d_etot_h2 * d_h2_s2 * inputs[0];
                //Debug.Log("d_etot_w3: " + d_etot_w3);
                float d_etot_w4 = d_etot_h2 * d_h1_s1 * inputs[1];
                //Debug.Log("d_etot_w4: " + d_etot_w4);


                // Weights update
                weights1[0] = weights1[0] - learningRate * d_etot_w1;
                //Debug.Log("w1: " + weights1[0]);
                weights1[1] = weights1[1] - learningRate * d_etot_w2;
                //Debug.Log("w2: " + weights1[1]);
                weights1[2] = weights1[2] - learningRate * d_etot_w3;
                //Debug.Log("w3: " + weights1[2]);
                weights1[3] = weights1[3] - learningRate * d_etot_w4;
                //Debug.Log("w4: " + weights1[3]);

                weights2[0] = weights2[0] - learningRate * d_etot_w5;
                //Debug.Log("w5: " + weights2[0]);
                weights2[1] = weights2[1] - learningRate * d_etot_w6;
                //Debug.Log("w6: " + weights2[1]);
                weights2[2] = weights2[2] - learningRate * d_etot_w7;
                //Debug.Log("w7: " + weights2[2]);
                weights2[3] = weights2[3] - learningRate * d_etot_w8;
                //Debug.Log("w8: " + weights2[3]);

            }

        }

        float Sigmoid(float s)
        {
            return 1 / (1 + Mathf.Exp(-s));
        }
    }
}
