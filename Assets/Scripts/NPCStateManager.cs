using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCStateManager : MonoBehaviour
{
    public float _movementSpeed;
    public float _aggroRange;
    public float _attackRange;
    public float _waitTime;

    [SerializeField] private Vector2 movementBounds;
    [SerializeField] private float startY;
    [SerializeField] private float endY;

    private NPCState currentNPCState;
    private Transform playerTransform;
    private Vector2 roamDestination;
    [SerializeField] private bool _playerDetected;

    private bool appearing = false;
    private bool isRoaming = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentNPCState = NPCState.Hide;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        spriteRenderer = GetComponent<SpriteRenderer>(); // Gauname SpriteRenderer komponentą
    }

    private void Update()
    {
        UpdateState();

        switch (currentNPCState)
        {
            case NPCState.Roam:
                if (!isRoaming)
                {
                    StartCoroutine(MoveTowardsRandomPoint());
                    isRoaming = true;
                }
                break;
            case NPCState.ChasePlayer:
                ChasePlayer();
                break;
            case NPCState.AttackPlayer:
                AttackPlayer();
                break;
            case NPCState.Hide:
                if (!appearing)
                {
                    AppearFromGround();
                }
                break;
            default:
                break;
        }
    }

    private void AppearFromGround()
    {
        appearing = true;
        transform.LeanMoveY(endY, 3f).setEaseOutQuad().setOnComplete(() =>
        {
            appearing = false;
            currentNPCState = NPCState.Roam;
            roamDestination = GetRandomPoint();
        });
    }

    private void AttackPlayer()
    {
        ChasePlayer();
        //cia hittinimo logic;
    }

    private void ChasePlayer()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, playerTransform.position, _movementSpeed * Time.deltaTime);

        // Patikriname, ar NPC juda į kairę ar dešinę ir sukame NPC transformaciją
        if (transform.position.x < playerTransform.position.x) // Jei judama į kairę
        {
            spriteRenderer.flipX = true; // Sukame NPC į kairę
        }
        else if (transform.position.x > playerTransform.position.x) // Jei judama į dešinę
        {
            spriteRenderer.flipX = false; // Sukame NPC į dešinę
        }
    }

    private IEnumerator MoveTowardsRandomPoint()
    {
        while (currentNPCState == NPCState.Roam)
        {

            transform.position = Vector2.MoveTowards(transform.position, roamDestination, _movementSpeed * Time.deltaTime);
            if (transform.position.x < roamDestination.x) // Jei judama į kairę
            {
                spriteRenderer.flipX = true; // Sukame NPC į kairę
            }
            else if (transform.position.x > roamDestination.x) // Jei judama į dešinę
            {
                spriteRenderer.flipX = false; // Sukame NPC į dešinę
            }
            if (Vector2.Distance(transform.position, roamDestination) < 0.1f)
            {
                yield return new WaitForSeconds(_waitTime);
                roamDestination = GetRandomPoint();
            }
            yield return null;
        }
        isRoaming = false;
    }

    private Vector2 GetRandomPoint()
    {
        return new Vector2(UnityEngine.Random.Range(movementBounds.x, movementBounds.y), transform.position.y);
    }

    private void UpdateState()
    {
        if (currentNPCState == NPCState.Hide)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _aggroRange, Vector2.zero, 0f,
            LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= _aggroRange)
            {
                _playerDetected = true;
                currentNPCState = NPCState.ChasePlayer;
            }
            else
            {
                _playerDetected = false;
                currentNPCState = NPCState.Roam;
            }

            if (distanceToPlayer <= _attackRange)
            {
                currentNPCState = NPCState.AttackPlayer;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw chase range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        if (isRoaming)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, roamDestination);
        }
    }

}