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
    public float minWalkDistance = 3f;
    public float maxWalkDistance = 7f;
    public float walkSpeed = 2f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;
    public float chanceToMoveRight = 0.5f;
    public float attackDuration = 2f;
    public float scatterChance = 0.5f;

    private bool isWalking = false;
    private bool isMovingRight = false;
    private bool isFirstWalk = true;
    private Vector3 centerPosition;

    private BirdSpawner spawner;

    private void Start()
    {
        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);

        spawner = FindObjectOfType<BirdSpawner>();

        SetState(CalmBirdState.Idle);
        StartCoroutine(IdleState());
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (isFirstWalk)
            {
                isMovingRight = transform.position.x < centerPosition.x;

                isFirstWalk = false;

                SetState(CalmBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < scatterChance)
                {
                    SetState(CalmBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    float actionRoll = Random.value;
                    if (actionRoll < 0.5f)
                    {
                        SetState(CalmBirdState.Walk);
                        yield return StartCoroutine(WalkingState());
                    }
                    else
                    {
                        SetState(CalmBirdState.Attack);
                        yield return StartCoroutine(AttackState());
                    }
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

        while (isWalking && Vector3.Distance(transform.position, initialPosition) < maxWalkDistance)
        {
            float moveDirection = isMovingRight ? 1f : -1f;
            transform.Translate(Vector3.right * moveDirection * walkSpeed * Time.deltaTime);
            yield return null;
        }

        isWalking = false;
        SetState(CalmBirdState.Idle);
    }

    private IEnumerator AttackState()
    {
        yield return new WaitForSeconds(attackDuration);
        SetState(CalmBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        Vector2 destination = new Vector2(50f, 20f);
        bool moveRight = destination.x > transform.position.x;

        FlipSprite(moveRight);

        float scatterSpeed = walkSpeed * 25f;
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
        scale.x = flipRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void SetState(CalmBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
