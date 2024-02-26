using System.Collections;
using UnityEngine;

public enum TimidBirdState
{
    Idle = 0,
    Walk = 1,
    Attack = 2,
    Scatter = 3,
}

public class TimidBirdBehavior : MonoBehaviour
{
    public Animator animator;
    public TimidBirdState currentState = TimidBirdState.Idle;
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
    private Vector3 centerPosition; // Center of the game scene

    private BirdSpawner spawner; // Reference to the BirdSpawner script

    private void Start()
    {
        // Get the center position of the game scene
        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);

        // Find the BirdSpawner script
        spawner = FindObjectOfType<BirdSpawner>();

        // Start the bird in the Idle state
        SetState(TimidBirdState.Idle);
        StartCoroutine(IdleState());
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (isFirstWalk)
            {
                // Determine the direction towards the center of the screen
                isMovingRight = transform.position.x < centerPosition.x;

                // Set isFirstWalk to false
                isFirstWalk = false;

                // Start walking
                SetState(TimidBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < scatterChance)
                {
                    // Start scattering
                    SetState(TimidBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    float actionRoll = Random.value;
                    if (actionRoll < 0.7f) // Increase chance of Attack state
                    {
                        // Start attacking
                        SetState(TimidBirdState.Attack);
                        yield return StartCoroutine(AttackState());
                    }
                    else
                    {
                        // Start walking
                        SetState(TimidBirdState.Walk);
                        yield return StartCoroutine(WalkingState());
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
        SetState(TimidBirdState.Idle);
    }

    private IEnumerator AttackState()
    {
        // Flip the sprite back and forth multiple times
        int flipCount = 0;
        while (flipCount < 5) // Flip the sprite 5 times
        {
            FlipSprite(!isMovingRight); // Flip the sprite
            yield return new WaitForSeconds(0.1f); // Wait for a short duration before flipping again
            FlipSprite(isMovingRight); // Flip back to original state
            yield return new WaitForSeconds(0.1f); // Wait again
            flipCount++; // Increment flip count
        }

        // Return to the idle state after attacking
        SetState(TimidBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        // Define the destination coordinate outside of the game screen
        Vector2 destination = new Vector2(-50f, 25f); // Destination for scattering

        // Determine the movement direction
        bool moveRight = destination.x > transform.position.x;

        // Flip the sprite based on the movement direction
        FlipSprite(moveRight);

        // Move the bird towards the destination
        float scatterSpeed = walkSpeed * 25f; // Speed for scatter
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            // Move the bird towards the destination
            transform.position = Vector2.MoveTowards(transform.position, destination, scatterSpeed * Time.deltaTime);

            yield return null;
        }

        // After reaching the destination, destroy the bird game object
        Destroy(gameObject);

        // Call the DecreaseBirdCount method in the spawner
        if (spawner != null)
        {
            spawner.DecreaseBirdCount();
        }
    }

    private void FlipSprite(bool flipRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = flipRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // Flip based on movement direction
        transform.localScale = scale;
    }

    private void SetState(TimidBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
