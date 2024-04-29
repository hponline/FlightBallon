using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    
    public float floatForce;
    private float gravityModifier = 4;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    // Balon yukseklik 
    private float yFlight;
    // Sınır
    private float yRange = 15;

    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Player fizik özelliklerine erişir
        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Balon yüksekligini verdigimiz değerler arasında tutar
        yFlight = Mathf.Clamp(transform.position.y, 0, yRange);
        // Yeni pozisyona atar
        transform.position = new Vector3(transform.position.x, yFlight, transform.position.z);

        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * verticalInput * Time.deltaTime);

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && gameOver == false)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
