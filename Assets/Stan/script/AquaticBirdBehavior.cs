using System.Collections;
using UnityEngine;

public enum AquaticBirdState
{
    Idle = 0,
    Walk = 1,
    Scatter = 2,
}

public class AquaticBirdBehavior : MonoBehaviour
{
    public Animator animator;
    public AquaticBirdState currentState = AquaticBirdState.Idle;
    public float minWalkDistance = 3f; // Minimum walk distance
    public float maxWalkDistance = 7f; // Maximum walk distance
    public float walkSpeed = 2f; // Adjust as needed
    public float minIdleTime = 2f; // Adjust as needed
    public float maxIdleTime = 5f; // Adjust as needed
    public float chanceToMoveRight = 0.5f; // Chance to move right
    public float scatterSpeed = 25f; // Speed for scatter

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
        SetState(AquaticBirdState.Idle);
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
                SetState(AquaticBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < 0.5f) // 50% chance to scatter
                {
                    // Start scattering
                    SetState(AquaticBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    // Start walking
                    SetState(AquaticBirdState.Walk);
                    yield return StartCoroutine(WalkingState());
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
        SetState(AquaticBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        // Define the destination coordinate outside of the game screen
        float destinationX = isMovingRight ? 100f : -100f;
        Vector2 destination = new Vector2(destinationX, transform.position.y); // Destination for scattering

        // Determine the movement direction
        bool moveRight = destination.x > transform.position.x;

        // Flip the sprite based on the movement direction
        FlipSprite(moveRight);

        // Move the bird towards the destination
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            // Move the bird towards the destination
            transform.position = Vector2.MoveTowards(transform.position, destination, scatterSpeed * Time.deltaTime);
            yield return null;
        }

        // After reaching the destination, destroy the bird GameObject
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
        scale.x = flipRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x); // Flip based on movement direction
        transform.localScale = scale;
    }

    private void SetState(AquaticBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
