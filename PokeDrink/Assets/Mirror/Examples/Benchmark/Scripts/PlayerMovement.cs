using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Benchmark
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 5;
        public GameObject PlayerModel;
        public SpriteRenderer sprite;

        public void Start()
        {
            sprite = PlayerModel.GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
            {
                // Set active if in game scene
                if (!sprite.enabled)
                {
                    sprite.enabled = true;
                }
                if (hasAuthority)
                {
                    Movement();
                }
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
