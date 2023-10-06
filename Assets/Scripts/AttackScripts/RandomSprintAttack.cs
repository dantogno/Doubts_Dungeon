using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomSprintAttack : MonoBehaviour
{
    private WaitForSeconds waitTime = new WaitForSeconds(1f);

    public float sprintSpeed = 8f;
    public float regularSpeed = 4f;// Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Start the coroutine to generate random numbers every second
        StartCoroutine(GenerateRandomNumberRoutine());
    }

    private IEnumerator GenerateRandomNumberRoutine()
    {
        while (true)
        {
            int randomNumber = Random.Range(1, 6);
            CheckForSprint(randomNumber);

            yield return waitTime;
        }
    }

    private void CheckForSprint(int randomNumber)
    {
        if(randomNumber == 3) 
        {
            StartSprint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprinting)
        {
            currentSprintTime += Time.deltaTime;
            if (currentSprintTime >= sprintDuration)
            {
                StopSprint();
            }
        }
    }

    private void StartSprint()
    {
        isSprinting = true;
        currentSprintTime = 0f;
        navMeshAgent.speed = sprintSpeed;
    }

    private void StopSprint()
    {
        isSprinting = false;
        navMeshAgent.speed = regularSpeed;
        // You can add any post-sprint logic here, if needed.
    }
}
