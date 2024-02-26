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
    public float flySpeed = 10f; // Adjust as needed

    private Vector3 centerPosition; // Center of the game scene

    private void Start()
    {
        // Get the center position of the game scene
        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);

        // Start flying
        StartCoroutine(FlyState());
    }

    private IEnumerator FlyState()
    {
        // Determine the direction towards the center of the screen
        bool flyRight = transform.position.x < centerPosition.x;
        float destinationX = flyRight ? 100f : -100f;
        Vector2 destination = new Vector2(destinationX, transform.position.y);

        // Flip the sprite based on the direction of flight
        FlipSprite(flyRight);

        // Move the bird towards the center of the screen
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            // Move the bird towards the destination
            transform.position = Vector2.MoveTowards(transform.position, destination, flySpeed * Time.deltaTime);
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
}
