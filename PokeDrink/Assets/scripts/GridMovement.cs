using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GridMovement : NetworkBehaviour
{
    private bool isMoveing;
    private Vector3 origPos;
    private Vector3 targetPos;
    [SerializeField] private LayerMask stopMovement;

    [SerializeField]
    private float timeToMove = 0.2f;
    public GameObject PlayerModel;
    public SpriteRenderer sprite;

    public void Start()
    {
        sprite = PlayerModel.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        transform.position = new Vector3(-24.5f, -12.35f, 0);
    }

    void Update()
    {
        SceneManager.activeSceneChanged += SceneChanged;

        if (hasAuthority)
        {
            Movement();
        }
    }

    public void SceneChanged(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
        {
            sprite.enabled = true;
        }
    }

    public void Movement()
    {
        if (!isLocalPlayer)
            return;
        if (!isMoveing)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(MovePlayer(Vector3.up));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(MovePlayer(Vector3.down));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(MovePlayer(Vector3.left));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(MovePlayer(Vector3.right));
            }
        }
    }

    public IEnumerator MovePlayer(Vector3 dir)
    {
        isMoveing = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + dir;
        // Check if the target position is valid
        if (Physics2D.OverlapCircle(targetPos, 0.2f, stopMovement))
        {
            targetPos = origPos;
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoveing = false;
    }
}
