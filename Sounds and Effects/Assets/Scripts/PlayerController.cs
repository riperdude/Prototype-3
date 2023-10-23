using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float JumpForce = 10f;
    public float GravityModifier = 1f;
    public bool IsOnGround = true;
    public bool gameOver;
    public ParticleSystem ExplosionParticle;
    public ParticleSystem DirtParticle;
    public AudioClip JumpSound;
    public AudioClip CrashSound;

    private Rigidbody _playerRb;
    private Animator _playerAnim;
    private AudioSource _playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= GravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsOnGround && gameOver == false)
        {
            _playerRb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            IsOnGround = false;
            _playerAnim.SetTrigger("Jump_trig");
            _playerAudio.PlayOneShot(JumpSound, 1);
            DirtParticle.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsOnGround = true;
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsOnGround = true;
            DirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle")) 
        {
            ExplosionParticle.Play();
            DirtParticle.Stop();
            _playerAudio.PlayOneShot(CrashSound, 1.0f);
            gameOver = true; 
            Debug.Log("Game Over!");
            _playerAnim.SetBool("Death_b", true);
            _playerAnim.SetInteger("DeathType_int", 1);
        }
    }
}
