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

        SetState(TimidBirdState.Idle);
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

                SetState(TimidBirdState.Walk);
                yield return StartCoroutine(WalkingState());
            }
            else
            {
                float scatterRoll = Random.value;
                if (scatterRoll < scatterChance)
                {
                    SetState(TimidBirdState.Scatter);
                    yield return StartCoroutine(ScatterState());
                }
                else
                {
                    float actionRoll = Random.value;
                    if (actionRoll < 0.7f)
                    {
                        SetState(TimidBirdState.Attack);
                        yield return StartCoroutine(AttackState());
                    }
                    else
                    {
                        SetState(TimidBirdState.Walk);
                        yield return StartCoroutine(WalkingState());
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
        SetState(TimidBirdState.Idle);
    }

    private IEnumerator AttackState()
    {
        int flipCount = 0;
        while (flipCount < 5)
        {
            FlipSprite(!isMovingRight);
            yield return new WaitForSeconds(0.1f);
            FlipSprite(isMovingRight);
            yield return new WaitForSeconds(0.1f);
            flipCount++;
        }

        SetState(TimidBirdState.Idle);
    }

    private IEnumerator ScatterState()
    {
        Vector2 destination = new Vector2(-50f, 25f);

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

    private void SetState(TimidBirdState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)newState);
    }
}
