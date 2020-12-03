using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int powerupID; // 0 = triple shot power, 1 = speed power up, 3 = shield power up
    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //communicate with player script
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                if (powerupID == 0)
                {
                    player.TripleShotActive();

                }

                else if (powerupID == 1)
                {
                    player.SpeedBoostActive();
                }

                else if (powerupID == 2)
                {
                    player.ShieldsActive();
                }
            }

            switch (powerupID)
            {
                   
                case 0: 
                    player.TripleShotActive();
                    break;

                case 1:
                    break;

                case 2:
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
