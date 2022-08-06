using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GridMovement : NetworkBehaviour
{
    private Animator animator;
    private bool isMoveing;
    private Vector3 origPos;
    private Vector3 targetPos;

    [SerializeField]
    private LayerMask stopMovement;
    private float timeToMove = 0.2f;
    public GameObject PlayerModel;
    public SpriteRenderer sprite;
    private bool canMove;
    private MovementCounter movementCounter;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public void Start()
    {
        movementCounter = GetComponent<MovementCounter>();
        canMove = false;
        sprite = PlayerModel.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        transform.position = new Vector3(-28f, -12f, 0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SceneManager.activeSceneChanged += SceneChanged;
        if (movementCounter.Movement <= 0)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        if (hasAuthority && canMove)
        {
            Movement();

        }
    }

    public void SceneChanged(Scene current, Scene next)
    {
        if (next.name.Equals("Scene_SteamworksGame"))
        {
            sprite.enabled = true;
            canMove = true;
        }
    }

    public void Movement()
    {
        if (!isLocalPlayer)
            return;
        if (!isMoveing)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                animator.SetFloat("yInput", 0);
                animator.SetFloat("xInput", Input.GetAxis("Horizontal"));
                if (Input.GetKey(KeyCode.A))
                {
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartCoroutine(MovePlayer(Vector3.right));
                }
            }
            else if (Input.GetAxis("Vertical") != 0)
            {
                animator.SetFloat("xInput", 0);
                animator.SetFloat("yInput", Input.GetAxis("Vertical"));
                if (Input.GetKey(KeyCode.W))
                {
                    StartCoroutine(MovePlayer(Vector3.up));
                }
                if (Input.GetKey(KeyCode.S))
                {
                    StartCoroutine(MovePlayer(Vector3.down));
                }
            }
        }
    }

    public IEnumerator MovePlayer(Vector3 dir)
    {
        origPos = transform.position;
        targetPos = origPos + dir;
        // Check if the target position is valid
        if (Physics2D.OverlapCircle(targetPos, 0.2f, stopMovement))
        {
            yield break;
        }
        isMoveing = true;
        movementCounter.Movement--;
        animator.SetBool("isWalking", true);
        float elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        float x = transform.position.x;
        float y = transform.position.y;
        // if position has drifted more than 0.1f, snap to nearest grid position
        if (Mathf.Abs(x - Mathf.Round(x)) > 0.1f || Mathf.Abs(y - Mathf.Round(y)) > 0.1f)
        {
            transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);
        }
        else // otherwise, move to target position
        {
            transform.position = targetPos;
        }
        animator.SetBool("isWalking", false);
        isMoveing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision with " + other.name);
        if (other.gameObject.tag == "Teleporter")
        {
            Vector3 destination = other.gameObject.GetComponent<Teleporter>().TeleportLocation;

            StartCoroutine(TeleportPlayer(destination));
        }
    }

    private IEnumerator TeleportPlayer(Vector3 destination)
    {
        isMoveing = true;
        float elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;
        Debug.Log("Teleporting to " + destination);
        isMoveing = false;
    }
}
