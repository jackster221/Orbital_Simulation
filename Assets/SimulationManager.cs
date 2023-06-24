using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    // Store the initial states for each celestial body
    public static Dictionary<CelestialBody, (Vector3 position, Vector3 velocity, Vector3 acceleration)> initialStates = new Dictionary<CelestialBody, (Vector3 position, Vector3 velocity, Vector3 acceleration)>();
    

    [SerializeField] public static bool isSimulationRunning;
    [SerializeField] public static bool isSimulationReset;
    [SerializeField] private List<CelestialBody> celestialBodies;

    private void Start()
    {
        PopulateCelestialBodiesList();
        StoreInitialStates();
    }

    public void PopulateCelestialBodiesList()
    {
        celestialBodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());
    }

    public void StartSimulation()
    {
        isSimulationRunning = true;
        isSimulationReset = false;
    }

    public void PauseSimulation()
    {
        isSimulationRunning = false;      
    }

    public void ResetSimulation()
    {
        isSimulationRunning = false;
        isSimulationReset = true;
    }

    private void StoreInitialStates()
    {
        foreach (CelestialBody body in celestialBodies)
        {
            initialStates[body] = (body.Position, body.Velocity, body.Acceleration);
        }
    }
}


