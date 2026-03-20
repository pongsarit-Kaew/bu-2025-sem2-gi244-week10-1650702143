using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController3 : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;
    public float dashForce = 10f; // แรงพุ่ง
    private bool isDashing = false;

    private Rigidbody rb;
    private InputAction jumpAction;

    
    private bool isOnGround = true;
    private int jumpCount = 0;
    public int maxJumps = 2;
    // ----------------------------

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        
        Physics.gravity = new Vector3(0, -9.81f, 0) * gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        gameOver = false;
    }

    void Update()
    {

        if (jumpAction.triggered && jumpCount < maxJumps && !gameOver)
        {

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpCount++;
            isOnGround = false;

            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
        }
        {
            // เช็คการกดปุ่ม Shift และต้องไม่ GameOver
            if (Input.GetKeyDown(KeyCode.LeftShift) && !gameOver && !isDashing)
            {
                StartCoroutine(DashRoutine());
            }
        }
        // 2. ฟังก์ชัน Dash ที่แยกออกมาเป็นอิสระ (วางก่อนปีกกาปิดอันสุดท้ายของไฟล์)
        System.Collections.IEnumerator DashRoutine()
        {
            isDashing = true;
            Debug.Log("Dashing Now!");
            // ใส่แรงพุ่งไปข้างหน้า (แกน X หรือ Z ขึ้นอยู่กับฉากของคุณ)
            Rigidbody playerRb = GetComponent<Rigidbody>();
            // ใช้ ForceMode.VelocityChange เพื่อให้พุ่งทันทีโดยไม่สนน้ำหนัก
            playerRb.AddForce(Vector3.right * dashForce, ForceMode.VelocityChange);
            yield return new WaitForSeconds(0.2f); // ระยะเวลาพุ่ง
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 0; 
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSfx);
        }
    }
}