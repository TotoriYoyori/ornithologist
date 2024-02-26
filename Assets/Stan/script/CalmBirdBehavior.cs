using System.Collections;
using UnityEngine;

public enum CalmBirdState
{
    Idle = 0,
    Walk = 1,
    Attack = 2,
    Scatter = 3,
}

public class CalmBirdBehavior : MonoBehaviour
{
    public Animator animator;
    public CalmBirdState currentState = CalmBirdState.Idle;
    public float minWalkDistance = 3f; // Minimum walk distance
    public float maxWalkDistance = 7f; // Maximum walk distance
    public float walkSpeed = 2f; // Adjust as needed
    public float minIdleTime = 2f; // Adjust as needed
    public float maxIdleTime = 5f; // Adjust as needed
    public float chanceToMoveRight = 0.5f; // Chance to move right
    public float attackDuration = 2f; // Duration of attack animation
    public float scatterChance = 0.5f; // Chance to scatter

    private bool isWalking = false;
    private bool isMovingRight = false;
    private bool isFirstWalk = true; // Track if it's the first walk
    private bool isFirstWalkRight = false; // Track if the first walk is to the right

    private void Start()
    {
        // Start the bird in the Idle state
        SetState(CalmBirdState.Idle);
        StartCoroutine(IdleState());
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (isFirstWalk)
            {
                // Always walk first towards the right when the game starts
                isFirstWalk = false;
                isFirstWalkRight = true;
                SetState(CalmBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < scatterChance)
                {
                    // Start scattering
                    SetState(CalmBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    float actionRoll = Random.value;
                    if (actionRoll < 0.5f)
                    {
                        // Start walking
                        SetState(CalmBirdState.Walk);
                        yield return StartCoroutine(WalkingState());
                    }
                    else
                    {
                        // Start attacking
                        SetState(CalmBirdState.Attack);
                        yield return StartCoroutine(AttackState());
                    }
                }
            }

            // Wait for a random idle time
            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        }
    }

    private IEnumerator WalkingState()
    {
        // Set initial position and movement direction
        Vector3 initialPosition = transform.position;
        isWalking = true;

        if (isFirstWalk && isFirstWalkRight)
        {
            isMovingRight = true;
            isFirstWalkRight = false;
        }
        else
        {
            isMovingRight = Random.value < chanceToMoveRight;
        }

        // Flip the sprite based on movement direction
        FlipSprite(isMovingRight);

        // Move until the maximum walk distance is reached
        while (isWalking && Vector3.Distance(transform.position, initialPosition) < maxWalkDistance)
        {
            // Determine movement direction
            float moveDirection = isMovingRight ? 1f : -1f;
            transform.Translate(Vector3.right * moveDirection * walkSpeed * Time.deltaTime);
            yield return null;
        }

        // Return to the idle state after walking
        isWalking = false;
        SetState(CalmBirdState.Idle);
    }

    private IEnumerator AttackState()
    {
        // Play attack animation for the specified duration
        yield return new WaitForSeconds(attackDuration);
        SetState(CalmBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        // Move the bird a great distance (outside of game bound) and then disappear
        Vector3 destination = transform.position + (isMovingRight ? Vector3.right : Vector3.left) * 100f; // Move up high
        float scatterSpeed = walkSpeed * 25f; // Speed for scatter
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, scatterSpeed * Time.deltaTime);
            yield return null;
        }

        // After reaching the destination, deactivate the bird game object
        gameObject.SetActive(false);
    }

    private void FlipSprite(bool flipRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = flipRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // Flip based on movement direction
        transform.localScale = scale;
    }

    private void SetState(CalmBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
