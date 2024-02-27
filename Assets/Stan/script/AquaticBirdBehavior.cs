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
    public float minWalkDistance = 3f; 
    public float maxWalkDistance = 7f; 
    public float walkSpeed = 2f; 
    public float minIdleTime = 2f; 
    public float maxIdleTime = 5f; 
    public float chanceToMoveRight = 0.5f;
    public float scatterSpeed = 25f;

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

        spawner = FindObjectOfType<BirdSpawner>();

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
                isFirstWalk = false;

                SetState(AquaticBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < 0.5f) 
                {
                    SetState(AquaticBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    SetState(AquaticBirdState.Walk);
                    yield return StartCoroutine(WalkingState());
                }
            }
            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        }
    }

    private IEnumerator WalkingState()
    {
        Vector3 initialPosition = transform.position;
        isWalking = true;

        FlipSprite(isMovingRight);

        // Move until the maximum walk distance is reached
        while (isWalking && Vector3.Distance(transform.position, initialPosition) < maxWalkDistance)
        {
            float moveDirection = isMovingRight ? 1f : -1f;
            transform.Translate(Vector3.right * moveDirection * walkSpeed * Time.deltaTime);
            yield return null;
        }

        isWalking = false;
        SetState(AquaticBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        // Define the destination coordinate outside of the game screen
        float destinationX = isMovingRight ? 100f : -100f;
        Vector2 destination = new Vector2(destinationX, transform.position.y); 

        bool moveRight = destination.x > transform.position.x;

        FlipSprite(moveRight);

        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, scatterSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
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
