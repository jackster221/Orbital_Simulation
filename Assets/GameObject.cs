using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObject : MonoBehaviour
{
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
    protected const float G = 10e-3f;


    public Vector3 CalculateGravitationalForce(GameObject other)
    {
        Vector3 direction = other.Position - this.Position;
        float distance = direction.magnitude;
        float forceMagnitude = G * (this.Mass * other.Mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        return force;
    }



    public abstract void PhysicsUpdate();

 
}
