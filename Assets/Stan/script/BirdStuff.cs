// using System.Collections;
// using UnityEngine;

// // public enum BirdState
// // {
// //     Idle = 0,
// //     Walk = 1,
// //     Attack = 2,
// // }

// public class CalmBirdBehavior : MonoBehaviour
// {
//     public Animator animator;
//     public BirdState currentState = BirdState.Idle;
//     public float minWalkDistance = 3f; // Minimum walk distance
//     public float maxWalkDistance = 7f; // Maximum walk distance
//     public float walkSpeed = 2f; // Adjust as needed
//     public float minIdleTime = 2f; // Adjust as needed
//     public float maxIdleTime = 5f; // Adjust as needed
//     public float chanceToMoveRight = 0.5f; // Chance to move right
//     public float attackDuration = 2f; // Duration of attack animation

//     private bool isWalking = false;
//     private bool isMovingRight = false;

//     private void Start()
//     {
//         // Start the bird in the Idle state
//         SetState(BirdState.Walk);
//         StartCoroutine(WalkingState());
//     }

//     private IEnumerator IdleState()
//     {
//         while (true)
//         {
//             float randomChoice = Random.value;
//             if (randomChoice < 0.5f)
//             {
//                 // Start walking
//                 SetState(BirdState.Walk);
//                 yield return StartCoroutine(WalkingState());
//             }
//             else
//             {
//                 // Start attacking
//                 SetState(BirdState.Attack);
//                 yield return StartCoroutine(AttackState());
//             }

//             // Wait for a random idle time
//             yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
//         }
//     }

//     private IEnumerator WalkingState()
//     {
//         // Set initial position and movement direction
//         Vector3 initialPosition = transform.position;
//         isWalking = true;
//         isMovingRight = Random.value < chanceToMoveRight;
//         if (isMovingRight)
//             FlipSprite();
//         else
//             FlipSprite(false);

//         // Move until the maximum walk distance is reached
//         while (isWalking && Vector3.Distance(transform.position, initialPosition) < maxWalkDistance)
//         {
//             // Determine movement direction
//             float moveDirection = isMovingRight ? 1f : -1f;
//             transform.Translate(Vector3.right * moveDirection * walkSpeed * Time.deltaTime);
//             yield return null;
//         }

//         // Return to the idle state after walking
//         isWalking = false;
//         SetState(BirdState.Idle);
//         StartCoroutine(IdleState());
//     }

//     private IEnumerator AttackState()
//     {
//         // Play attack animation for the specified duration
//         yield return new WaitForSeconds(attackDuration);
//         SetState(BirdState.Idle);
//     }

//     private void FlipSprite(bool flipRight = true)
//     {
//         Vector3 scale = transform.localScale;
//         scale.x = flipRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
//         transform.localScale = scale;
//     }

//     private void SetState(BirdState newState)
//     {
//         currentState = newState;
//         animator.SetInteger("State", (int)newState);
//     }
// }
