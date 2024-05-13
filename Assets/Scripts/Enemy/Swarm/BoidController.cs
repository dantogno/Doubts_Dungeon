using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int SwarmIndex;
    public float NoClumpingRadius = 2.0f; // Set to a low value for fish-like behavior
    public float LocalAreaRadius = 10.0f; // Set to a high value for fish-like behavior
    public float Speed;
    public float SteeringSpeed;
    public float LeaderWeight = 0.5f; // Weight for steering towards the leader

    public LayerMask obstacleMask; // Define the layer mask for obstacles
    public Vector3 pointOfInterest = Vector3.zero; // Define the point of interest

    public void SimulateMovement(List<BoidController> other, float time)
    {
        // Default vars
        var steering = Vector3.zero;

        // Separation vars
        Vector3 separationDirection = Vector3.zero;
        int separationCount = 0;

        Vector3 alignmentDirection = Vector3.zero;
        int alignmentCount = 0;

        Vector3 cohesionDirection = Vector3.zero;
        int cohesionCount = 0;

        BoidController leaderBoid = null;
        float leaderAngle = float.MaxValue;

        foreach (BoidController boid in other)
        {
            // Skip self
            if (boid == this || boid.SwarmIndex != this.SwarmIndex)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            // Identify local neighbors for separation
            if (distance < NoClumpingRadius)
            {
                separationDirection += (transform.position - boid.transform.position).normalized;
                separationCount++;
            }

            // Identify local neighbors for alignment
            if (distance < LocalAreaRadius)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;
            }

            // Identify local neighbors for cohesion
            if (distance < LocalAreaRadius)
            {
                cohesionDirection += (boid.transform.position - transform.position).normalized;
                cohesionCount++;
            }

            // Identify local leader
            var angle = Vector3.Angle(boid.transform.position - transform.position, transform.forward);
            if (angle < leaderAngle && angle < 90f)
            {
                leaderBoid = boid;
                leaderAngle = angle;
            }
        }

        // Weighted steering calculation
        if (separationCount > 0)
            steering += separationDirection.normalized * 0.5f;

        if (alignmentCount > 0)
            steering += alignmentDirection.normalized * 0.34f;

        if (cohesionCount > 0)
            steering += cohesionDirection.normalized * 0.16f;

        // Apply leader steering
        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized * LeaderWeight;

        // Steering towards point of interest
        if (Vector3.Distance(transform.position, pointOfInterest) < LocalAreaRadius)
            steering += (pointOfInterest - transform.position).normalized;

        // Raycast for obstacle avoidance
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, LocalAreaRadius, obstacleMask))
            steering = -(hitInfo.point - transform.position).normalized;

        // Normalize final steering vector
        if (steering != Vector3.zero)
            steering = steering.normalized;

        // Apply steering
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);

        // Move 
        transform.position += transform.forward * Speed * time;

        // Ensure rotation only around the Y-axis while retaining targeting direction
        Vector3 targetDirection = (steering != Vector3.zero) ? steering : transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }


}
