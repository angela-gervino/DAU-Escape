using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAUEscape
{
    public class Dissolve : MonoBehaviour
    {
        public float dissolveTime = 3.0f; // seconds

        public void Awake() // called as soon as ragdoll is instantiated
        {
            // Time.time = the time at the beginning of the current frame in seconds since the application (game) started
            dissolveTime += Time.time;
        }

        public void Update()
        {
            if (Time.time >= dissolveTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

