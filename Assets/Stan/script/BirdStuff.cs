using System.Collections;
using UnityEngine;

public enum BirdState
{
    Idle = 0,
    Walk = 1,
    Attack = 2,
}

public class BirdAnimatorController : MonoBehaviour
{
    public Animator animator;
    public BirdState currentState = BirdState.Idle;
    public float minWalkDistance = 3f; // Minimum walk distance
    public float maxWalkDistance = 7f; // Maximum walk distance
    public float walkSpeed = 2f; // Adjust as needed
    public float minIdleTime = 2f; // Adjust as needed
    public float maxIdleTime = 5f; // Adjust as needed

    private Vector3 initialPosition;
    private bool isWalking = false;

    private void Start()
    {
        // Start the bird in the Walk state
        SetState(BirdState.Walk);
        initialPosition = transform.position;
        isWalking = true;

        // Start coroutine to check when to stop walking
        StartCoroutine(StopWalkingAfterDistance());
    }

    private void Update()
    {
        // Handle transition to Idle state if necessary
        if (isWalking && Vector3.Distance(transform.position, initialPosition) >= maxWalkDistance)
        {
            isWalking = false;
            SetState(BirdState.Idle);
            StartCoroutine(WaitAndStartWalking(Random.Range(minIdleTime, maxIdleTime)));
        }
    }

    IEnumerator StopWalkingAfterDistance()
    {
        // Keep walking until the distance is reached
        while (isWalking)
        {
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator WaitAndStartWalking(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetState(BirdState.Walk);
        initialPosition = transform.position;
        isWalking = true;
        StartCoroutine(StopWalkingAfterDistance());
    }

    public void SetState(BirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);

        if(newState == BirdState.Walk) {
            // Randomize the walk distance when transitioning to Walk state
            maxWalkDistance = Random.Range(minWalkDistance, maxWalkDistance);
        }
    }
}
