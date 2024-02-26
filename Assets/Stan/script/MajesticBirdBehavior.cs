using System.Collections;
using UnityEngine;

public enum MajesticBirdState
{
    Idle = 0,
    Walk = 1,
    Attack = 2,
    Scatter = 3,
}

public class MajesticBirdBehavior : MonoBehaviour
{
    public Animator animator;
    public MajesticBirdState currentState = MajesticBirdState.Idle;
    public float minWalkDistance = 3f; // Minimum walk distance
    public float maxWalkDistance = 7f; // Maximum walk distance
    public float walkSpeed = 2f; // Adjust as needed
    public float minIdleTime = 2f; // Adjust as needed
    public float maxIdleTime = 5f; // Adjust as needed
    public float chanceToMoveRight = 0.5f; // Chance to move right
    public float attackDuration = 2f; // Duration of attack animation
    public float scatterChance = 0.25f; // Chance to scatter

    private bool isWalking = false;
    private bool isMovingRight = false;
    private bool isFirstWalk = true; // Track if it's the first walk
    private Vector3 centerPosition; // Center of the game scene

    private void Start()
    {
        // Get the center position of the game scene
        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);

        // Start the bird in the Idle state
        SetState(MajesticBirdState.Idle);
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
                SetState(MajesticBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < scatterChance)
                {
                    // Start scattering
                    SetState(MajesticBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    float actionRoll = Random.value;
                    if (actionRoll < 0.75f) // Increase chance of Attack state
                    {
                        // Start attacking
                        SetState(MajesticBirdState.Attack);
                        yield return StartCoroutine(AttackState());
                    }
                    else
                    {
                        // Start walking
                        SetState(MajesticBirdState.Walk);
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
        SetState(MajesticBirdState.Idle);
    }

    private IEnumerator AttackState()
    {
        // Play attack animation for the specified duration
        yield return new WaitForSeconds(attackDuration);

        // Return to the idle state after attacking
        SetState(MajesticBirdState.Idle);
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

        // After reaching the destination, deactivate the bird game object
        gameObject.SetActive(false);
    }

    private void FlipSprite(bool flipRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = flipRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // Flip based on movement direction
        transform.localScale = scale;
    }

    private void SetState(MajesticBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
