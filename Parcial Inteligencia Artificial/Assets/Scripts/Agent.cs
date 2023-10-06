using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;

    private Vector3 velocity;
    private Vector3 fleeVelocity;
    private Vector3 chaseVelocity;
    private Vector3 gravityVelocity;
    public LayerMask floorMask;

    public LayerMask foodMask;
    public LayerMask enemyMask;
    public float detectionRadius;

    Ground ground => Ground.Instance;

    public void Update()
    {
        velocity = Vector3.zero;

        Flee();
        Chase();

        velocity += chaseVelocity + fleeVelocity + gravityVelocity;
        velocity = Vector3.ClampMagnitude(velocity, 8f);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void AddAcceleration(Vector3 force)
    {
        velocity += force * Time.deltaTime;
    }

    private void Flee()
    {
        var nearColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyMask);

        if (nearColliders.Length > 0)
        {
            // Comida actual
            GameObject actualObj = null;

            //  Chequeamos las comidas cercanas
            foreach (var checkCollider in nearColliders)
            {
                // Direccion a cada Comida
                Vector3 collDir = checkCollider.transform.position - transform.position;

                // Chequeamos si hay pared entre yo y la comida
                if (!Physics.Raycast(transform.position, collDir, collDir.magnitude, floorMask))
                {
                    if (actualObj == null)
                    {
                        // Guardamos la comida si no hay pared en medio
                        actualObj = checkCollider.gameObject;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, actualObj.transform.position) > Vector3.Distance(transform.position, checkCollider.transform.position))
                        {
                            actualObj = checkCollider.gameObject;
                        }
                    }
                }
            }

            if (actualObj == null)
            {
                fleeVelocity = Vector3.zero;
                return;
            }

            Vector3 dir = transform.position - actualObj.transform.position;
            Vector3 dir2 = (actualObj.transform.position - transform.position) * -1;

            dir.Normalize();
            dir.y = 0;
            fleeVelocity += dir * moveSpeed * Time.deltaTime;
            fleeVelocity = Vector3.ClampMagnitude(fleeVelocity, 4f);
        }
        else
        {
            fleeVelocity = Vector3.zero;
        }
    }

    private void Chase()
    {
        var nearColliders = Physics.OverlapSphere(transform.position, detectionRadius, foodMask);

        if (nearColliders.Length > 0)
        {
            // Comida actual
            GameObject actualObj = null;

            //  Chequeamos las comidas cercanas
            foreach (var checkCollider in nearColliders)
            {
                // Direccion a cada Comida
                Vector3 collDir = checkCollider.transform.position - transform.position;

                // Chequeamos si hay pared entre yo y la comida
                if (!Physics.Raycast(transform.position, collDir, collDir.magnitude, floorMask))
                {
                    if (actualObj == null)
                    {
                        // Guardamos la comida si no hay pared en medio
                        actualObj = checkCollider.gameObject;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, actualObj.transform.position) > Vector3.Distance(transform.position, checkCollider.transform.position))
                        {
                            actualObj = checkCollider.gameObject;
                        }
                    }
                }
            }

            if (actualObj == null)
            {
                chaseVelocity = Vector3.zero;
                return;
            }

            Vector3 dir = actualObj.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;
            chaseVelocity += dir * moveSpeed * Time.deltaTime;
            chaseVelocity = Vector3.ClampMagnitude(chaseVelocity, 4f);
        }
        else
        {
            chaseVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    
}
