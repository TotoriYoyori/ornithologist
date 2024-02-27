using System.Collections;
using UnityEngine;

public enum FlyingBirdState
{
    Fly = 0,
}

public class FlyingBirdBehavior : MonoBehaviour
{
    public Animator animator;
    public FlyingBirdState currentState = FlyingBirdState.Fly;
    public float flySpeed = 10f;

    private BirdSpawner spawner;

    private Vector3 centerPosition;

    private void Start()
    {
        spawner = FindObjectOfType<BirdSpawner>();

        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);

        StartCoroutine(FlyState());
    }

    private IEnumerator FlyState()
    {
        bool flyRight = transform.position.x < centerPosition.x;
        float destinationX = flyRight ? 100f : -100f;
        Vector2 destination = new Vector2(destinationX, transform.position.y);

        FlipSprite(flyRight);

        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, flySpeed * Time.deltaTime);
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
}
