using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Benchmark
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 20;
        public GameObject PlayerModel;
        public SpriteRenderer sprite;

        public void Start()
        {
            sprite = PlayerModel.GetComponent<SpriteRenderer>();
            sprite.enabled = false;
            transform.position = new Vector3(-115, -61, 0);
        }

        void Update()
        {
            SceneManager.activeSceneChanged += SceneChanged;
            // if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
            // {
            //     // Set active if in game scene
            //     if (!sprite.enabled)
            //     {
            //         sprite.enabled = true;
            //     }
            //     if (hasAuthority)
            //     {
            //         Movement();
            //     }
            // }
            if (hasAuthority)
                {
                    Movement();
                }
        }
        public void SceneChanged(Scene current, Scene next)
        {
            Debug.Log("SceneChanged");
            if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
            {
                sprite.enabled = true;
            }
        }

        public void Movement()
        {
            if (!isLocalPlayer)
                return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, v, 0);
            transform.position += dir.normalized * (Time.deltaTime * speed);
        }
    }

}
