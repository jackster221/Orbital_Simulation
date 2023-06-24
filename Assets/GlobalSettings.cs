using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [Range(0f, 100)]
    public float simulationSpeed = 1f;

    private void Update()
    {
        Time.timeScale = simulationSpeed;
    }
}
