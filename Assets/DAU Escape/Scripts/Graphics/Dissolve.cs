using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAUEscape
{
    public class Dissolve : MonoBehaviour
    {
        public float dissolveTime = 2.0f; // seconds

        private Renderer[] renderers;

        public void Awake() // called as soon as ragdoll is instantiated
        {
            // Time.time = the time at the beginning of the current frame in seconds since the application (game) started
            dissolveTime += Time.time;

            renderers = GetComponentsInChildren<Renderer>(true);
        }// Awake


        public void Update()
        {
            if (Time.time < dissolveTime)
            {
                double alpha = 1 - Math.Pow(Time.time / dissolveTime, 2); // want alpha to go to 0 as the time gets closer to dissolveTime

                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        Color original = mat.color;
                        mat.color = new Color(original[0], original[1], original[2], (float)alpha);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }// Update

    }
}

