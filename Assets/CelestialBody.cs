using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CelestialBody : MonoBehaviour
{

    public enum BodyShape
    {
        Spherical,
        OblateSpheroid,
        Irregular
    }

    public enum BodyComposition
    {
        Rock,
        Gas,
        Ice
    }

    public static List<CelestialBody> allBodies = new List<CelestialBody>();
    public ObjectInfoUI objectInfo;

    private static CelestialBody selectedBody = null;

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    [SerializeField]
    private string _bodyType;
    public string BodyType
    {
        get { return _bodyType; }
        set { _bodyType = value; }
    }

    [SerializeField]
    private BodyShape _shape;
    public BodyShape Shape
    {
        get { return _shape; }
        set { _shape = value; }
    }

    [SerializeField]
    private BodyComposition _composition;
    public BodyComposition Composition
    {
        get { return _composition; }
        set { _composition = value; }
    }

    [SerializeField]
    private bool _hasAtmosphere;
    public bool HasAtmosphere
    {
        get { return _hasAtmosphere; }
        set { _hasAtmosphere = value; }
    }

    [SerializeField]
    private bool _hasMagneticField;
    public bool HasMagneticField
    {
        get { return _hasMagneticField; }
        set { _hasMagneticField = value; }
    }

    [SerializeField]
    private Vector3 _rotationAxis;
    public Vector3 RotationAxis
    {
        get { return _rotationAxis; }
        set { _rotationAxis = value; }
    }

    [SerializeField]
    private float _rotationSpeed;
    public float RotationSpeed
    {
        get { return _rotationSpeed; }
        set { _rotationSpeed = value; }
    }

    [SerializeField]
    private float _radius;
    public float Radius
    {
        get
        {
            _radius = transform.localScale.x / 2.0f;
            return _radius;
        }
        set
        {
            _radius = value;
            transform.localScale = new Vector3(_radius * 2.0f, _radius * 2.0f, _radius * 2.0f);
        }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    [SerializeField]
    private Vector3 _velocity;
    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    [SerializeField]
    private Vector3 _acceleration;
    public Vector3 Acceleration
    {
        get { return _acceleration; }
        set { _acceleration = value; }
    }

    [SerializeField]
    private Vector3 _force;
    public Vector3 Force
    {
        get { return _force; }
        set { _force = value; }
    }

    [SerializeField]
    private float _mass;
    public float Mass
    {
        get { return _mass; }
        set { _mass = value; }
    }

    // Gravity constant 
    protected const float G = 0.01f;


    public Vector3 CalculateGravitationalForce(CelestialBody other)
    {
        Vector3 direction = other.Position - this.Position;
        float distance = direction.magnitude;
        float forceMagnitude = G * (this.Mass * other.Mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        return force;
    }


    public CelestialBody(string bodyType, BodyShape shape, BodyComposition composition, bool hasAtmosphere,
                         bool hasMagneticField, Vector3 rotationAxis, float rotationSpeed, Vector3 velocity, Vector3 acceleration, float mass)

    {
        this.BodyType = bodyType;
        this.Shape = shape;
        this.Composition = composition;
        this.HasAtmosphere = hasAtmosphere;
        this.HasMagneticField = hasMagneticField;
        this.RotationAxis = rotationAxis;
        this.RotationSpeed = rotationSpeed;
        this.Radius = transform.localScale.x / 2.0f;
    }

    void FixedUpdate()
    {
        if (SimulationManager.isSimulationRunning)
        {
            PhysicsUpdate();

            if(selectedBody == this)
            {
                objectInfo.UpdateInfo(this);
            }
        }
        else if (SimulationManager.isSimulationReset)
        {
            ResetToInitialState();
        }

        //transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    void ResetToInitialState()
    {
        if (SimulationManager.initialStates.ContainsKey(this))
        {
            var initialState = SimulationManager.initialStates[this];
            Position = initialState.position;
            Velocity = initialState.velocity;
            Acceleration = initialState.acceleration;
        }
    }

    private void Awake()
    {
        allBodies.Add(this);
    }

    private void OnDestroy()
    {
        allBodies.Remove(this);
    }

    private void OnMouseOver()
    {
        if (selectedBody == null)
        {
            objectInfo.UpdateInfo(this);
        }
    }

    private void OnMouseExit()
    {
        if (selectedBody == null)
        {
            objectInfo.HideInfo();
        }
    }

    private void OnMouseDown()
    {
        if (selectedBody == this)
        {
            selectedBody = null;
            objectInfo.HideInfo();
        }
        else
        {
            selectedBody = this;
            objectInfo.UpdateInfo(this);
        }
    }


    public void PhysicsUpdate()
    {
        // Calculate total gravitational force exerted by all other bodies
        Vector3 force = Vector3.zero;
        foreach (CelestialBody body in allBodies)
        {
            if (body != this)
            {
                force += CalculateGravitationalForce(body);
            }
        }

        this.Force = force;

        // Implementing the 4th order Runge-Kutta method for better accuracy
        Vector3 k1v = this.Force / this.Mass * Time.deltaTime;
        Vector3 k1r = this.Velocity * Time.deltaTime;

        this.Force += force; // Updating force for the midpoint velocity
        Vector3 k2v = this.Force / this.Mass * Time.deltaTime;
        Vector3 k2r = (this.Velocity + k1v / 2) * Time.deltaTime;

        this.Force += force; // Updating force for the midpoint velocity
        Vector3 k3v = this.Force / this.Mass * Time.deltaTime;
        Vector3 k3r = (this.Velocity + k2v / 2) * Time.deltaTime;

        this.Force += force; // Updating force for the end-point velocity
        Vector3 k4v = this.Force / this.Mass * Time.deltaTime;
        Vector3 k4r = (this.Velocity + k3v) * Time.deltaTime;

        this.Velocity += (k1v + 2 * k2v + 2 * k3v + k4v) / 6;
        this.Position += (k1r + 2 * k2r + 2 * k3r + k4r) / 6;
    }
}