using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed, cameraZoom, dashSpeed, size, cooldown;
    [SerializeField] TMP_Text moveSpeedText, cameraZoomText, dashSpeedText, sizeText, cooldownText;
    [SerializeField] ParticleSystem cooldownParticles;
    float speedCalculationVar;

    float inputX, inputY;

    [SerializeField] bool canDash, isDashing;
    Vector2 dashDirection;

    GameObject gameManager;

    Rigidbody2D rb;

    float sizeAdjustmentY, sizeAdjustmentX;

    [SerializeField] ScoreScript scoreScript;

    [SerializeField] SpriteRenderer sprite, shadow;

    bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        gameManager = GameObject.Find("Game Manager");
        rb = GetComponent<Rigidbody2D>();

        SetupStats();

        sizeAdjustmentY = 11 - Camera.main.orthographicSize;
        sizeAdjustmentX = sizeAdjustmentY * (1.8f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -59.5f*0.8f, 59.5f*0.8f), Mathf.Clamp(transform.position.y, -59.2f*0.8f, 57.3f*0.8f), 0);
        // Input handling
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // Dash
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash(inputX, inputY);
        }

        // Camera follow, temp until Cinemachine
        
        Camera.main.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -29.5f - sizeAdjustmentX, 29.5f + sizeAdjustmentX), Mathf.Clamp(transform.position.y, -39 - sizeAdjustmentY, 37 + sizeAdjustmentY), -10f);
    }

    private void FixedUpdate()
    {
        // Actual movement code
        if (!isDashing && canMove) transform.Translate(new Vector3(inputX, inputY, 0).normalized * moveSpeed / 10);
        else
        {
            rb.velocity = dashDirection * dashSpeed * 12;
        }
    }

    void Dash(float inputX, float inputY)
    {
        canDash = false;
        dashDirection = new Vector2(inputX, inputY).normalized;
        StartCoroutine(DashHandler());
        StartCoroutine(DashCooldown());
    }

    void SetupStats()
    {
        canDash = true;

        cameraZoom = Random.Range(11f, 20f);
        Camera.main.orthographicSize = cameraZoom;
        cameraZoomText.text = (Mathf.Round(100*cameraZoom)/100).ToString();

        moveSpeed = Random.Range(2f, 5f);
        moveSpeedText.text = (Mathf.Round(100 * moveSpeed)/100).ToString();

        size = Random.Range(1f, 2.25f);
        transform.localScale = Vector3.one * size;
        sizeText.text= (Mathf.Round(100 * size)/100).ToString();

        dashSpeed = Random.Range(4f, 10f);
        speedCalculationVar = moveSpeed;
        dashSpeedText.text = (Mathf.Round(100 * dashSpeed)/100).ToString();

        cooldown = Random.Range(1.5f, 4f);
        cooldownText.text = (Mathf.Round(100 * cooldown) / 100).ToString();
    }

    public void NewWaveSetup()
    {
        // transform.position = Vector3.zero;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        cooldownParticles.Play();
        canDash = true;
    }

    IEnumerator DashHandler()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet" && !isDashing)
        {
            canMove = false;
            Debug.Log("Yo he just died hahahahahahahahahahahahahahahahahahahahahahahaha git gud");
            // Game End
            int score = scoreScript.GetScore();
            PlayerPrefs.SetFloat("curr", score);
            int bestInt;
            if (PlayerPrefs.HasKey("hiScore"))
            {
                bestInt = PlayerPrefs.GetInt("hiScore");

                if (score > bestInt)
                {
                    PlayerPrefs.SetInt("hiScore", score);
                }
            }
            else
            {
                PlayerPrefs.SetInt("hiScore", score);
            }

            sprite.enabled = false;
            shadow.enabled = false;
            StartCoroutine(EndScreen());
        }
    }

    IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }
}
