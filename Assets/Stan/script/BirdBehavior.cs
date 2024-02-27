// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Assets.FantasyMonsters.Scripts.Tweens;
// using UnityEngine;
// using Random = UnityEngine.Random;

// namespace Assets.FantasyMonsters.Scripts
// {
//     public enum BirdState
//     {
//         Idle = 0,
//         Walk = 1,
//         Attack = 2,
//     }

//     public class BirdBehavior : MonoBehaviour
//     {
//         public SpriteRenderer Head;
//         public List<Sprite> HeadSprites;
//         public Animator Animator;
//         public bool Variations;
//         public event Action<string> OnEvent = eventName => { };

//         private BirdState currentState;
//         private float nextActionTime; // Time to perform the next action
//         private float intervalBetweenActions; // Interval between actions
//         private const float minIntervalBetweenActions = 1f;
//         private const float maxIntervalBetweenActions = 5f;
//         private const float attackChance = 0.1f;

//         private float walkDistance = 3f; // Adjust this value as needed
//         private float walkSpeed = 1f; // Adjust this value as needed
//         private Vector3 initialPosition;

//         private bool IsIdling;
//         private bool IsWalking;
//         private bool IsAttacking;

//         private void Start()
//         {
//             currentState = BirdState.Walk;
//             SetState(BirdState.Walk);
//             initialPosition = transform.position;
//             intervalBetweenActions = Random.Range(minIntervalBetweenActions, maxIntervalBetweenActions);
//             nextActionTime = Time.time + intervalBetweenActions;
//         }

//         private void Update()
//         {
//             if (currentState == BirdState.Walk)
//             {
//                 MoveHorizontally(); 
//             } 
            
//             else if (currentState == BirdState.Idle && Time.time > nextActionTime)
//             {
//                 PerformAction();
//                 nextActionTime = Time.time + intervalBetweenActions;
//             }
//         }

// private void MoveHorizontally()
// {
//     if (currentState == BirdState.Walk)
//     {
//         float distanceToMove = walkSpeed * Time.deltaTime;
//         transform.Translate(Vector3.left * distanceToMove);

//         // Check if the bird has walked the desired distance
//         if (Mathf.Abs(transform.position.x - initialPosition.x) >= walkDistance)
//         {
//             SetState(BirdState.Idle);
//         }
//     }
// }

//         private void PerformAction()
//         {
//             switch (currentState)
//             {
//                 case BirdState.Walk:
//                     SetState(BirdState.Walk);
//                     IsWalking = true;
//                     MoveHorizontally();
//                     break;

//                 case BirdState.Idle:
//                     SetState(BirdState.Idle);
//                     IsIdling = true;

//                     intervalBetweenActions = Random.Range(minIntervalBetweenActions, maxIntervalBetweenActions);
//                     if (Random.value < attackChance)
//                     {
//                         SetState(BirdState.Attack);
//                         currentState = BirdState.Attack;
//                         intervalBetweenActions = Random.Range(minIntervalBetweenActions, maxIntervalBetweenActions);
//                     }
//                     break;

//                 case BirdState.Attack:
//                     SetState(BirdState.Attack);
//                     currentState = BirdState.Attack;
//                     intervalBetweenActions = Random.Range(minIntervalBetweenActions, maxIntervalBetweenActions);
//                     break;

//                 default:
//                     break;
//             }
//         }

//         private void SetState(BirdState state)
//         {
//             Animator.SetInteger("State", (int)state);
//         }

//         // Other methods as needed...
//     }
// }
