using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private GameObject camera;
    [SerializeField] private TMP_Text currentFormText;
    [SerializeField] private TMP_Text timesLeftSpeedText;
    [SerializeField] private TMP_Text timesLeftJumpText;
    [SerializeField] private TMP_Text timesLeftSpeedBoostText;
    [SerializeField] private AudioClip speedFormClip;
    [SerializeField] private AudioClip jumpFormClip;
    [SerializeField] private AudioClip goalClip;
    [SerializeField] private AudioClip speedBoostClip;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField] private AudioClip playerJumpClip;
    [SerializeField] private AudioClip baseFormClip;
    
    private Color baseColor;
    private SpriteRenderer sprite;
    public Color jumpColor;
  

    private float moveDirection;
    private bool isPlayerMoving;
    public bool didJumpOccur;
    private bool isJumpFormEnabled;
    private bool isSpeedFormEnabled;
    public bool isLevelTwo;
    public bool isLevelThree;
    private bool isInAir;
    private bool didSpeedBoostOccur;
    private int speedFormLimit = 10;
    private int jumpFormLimit = 10;
    private int speedBoostLimit = 5;
    private int currentScene;
    private float previousDirection;

    public Coroutine SpeedBoostRef;

    private Animator anim;
   



    private InputAction move;
    private InputAction jump;
    private InputAction speedForm;
    private InputAction jumpForm;
    private InputAction speedBoost;
    private InputAction restart;
    private InputAction quit;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
      //  baseColor = sprite.color;
        anim = gameObject.GetComponent<Animator>();

        EnableInputs();

        

        isPlayerMoving = false;
        didJumpOccur = false;
        isJumpFormEnabled = false;
        isSpeedFormEnabled = false;
        didSpeedBoostOccur = false;
        isLevelTwo = false;
        isLevelThree = false;

        currentScene = SceneManager.GetActiveScene().buildIndex;
        if((currentScene == 1) || (currentScene == 2))
        {
            timesLeftSpeedBoostText.gameObject.SetActive(false);
        }
        else
        {
            timesLeftSpeedBoostText.gameObject.SetActive(true);
            timesLeftSpeedBoostText.text = "Times left you can Speed boost: <color=green>" + speedBoostLimit.ToString() + "</color>";
        }

        timesLeftJumpText.text = "Times Left in Jump: " + jumpFormLimit.ToString();
        timesLeftSpeedText.text = "Times Left in Speed: " + speedFormLimit.ToString();

        currentFormText.text = "Current Form: Base";

        


    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = new Vector3(Rb2D.transform.position.x, Rb2D.transform.position.y, -10);

       if(didSpeedBoostOccur)
        {
            anim.Play("Player_Speed_Boost");
        }

        if(!didJumpOccur)
         {
               if(isSpeedFormEnabled && !didSpeedBoostOccur)
            {
                anim.Play("Player_Speed_Jump");
            }
               else if(isJumpFormEnabled && !didSpeedBoostOccur)
            {
                anim.Play("Player_Jump_Jump");
            }
               else
            {
                if(!didSpeedBoostOccur)
                {
                    anim.Play("Player_Base_Jump");
                }
                
            }
                

                moveDirection = move.ReadValue<float>();

                if (moveDirection > 0)
                {
                    // sprite.flipX = false;
                    transform.localScale = new Vector2(1, 1);
                }
                else if (moveDirection < 0)
                {
                    // sprite.flipX = true;
                    transform.localScale = new Vector2(-1, 1);
                }

         }


      else if (isPlayerMoving)
        {
            if(isSpeedFormEnabled && !didSpeedBoostOccur)
            {
                anim.Play("Player_Speed_Run");
            }
            else if(isJumpFormEnabled && !didSpeedBoostOccur)
            {
                anim.Play("Player_Jump_Run");
            }
            else
            {
                if(!didSpeedBoostOccur)
                {
                    anim.Play("Player_Base_Run");
                }
               
            }
            

            moveDirection = move.ReadValue<float>();

            if (moveDirection > 0)
            {
                // sprite.flipX = false;
                transform.localScale = new Vector2(1, 1);
            }
            else if (moveDirection < 0)
            {
                // sprite.flipX = true;
                transform.localScale = new Vector2(-1, 1);
            }






        }

        else if(!isPlayerMoving && didJumpOccur )
        {
            if(isSpeedFormEnabled)
            {
                anim.Play("Player_Speed_Idle");
            }
            else if(isJumpFormEnabled)
            {
                anim.Play("Player_Jump_Idle");
            }
            else
            {
                anim.Play("Player_Base_Idle");
            }
            
        }

       
      

        
    }

    private void FixedUpdate()
    {
        // Base form speed:

        if (isPlayerMoving && !(isSpeedFormEnabled))
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection * moveSpeed, Rb2D.velocity.y);
        }

        // Speed form speed:

        else if((isPlayerMoving) && (isSpeedFormEnabled) && (!didSpeedBoostOccur))
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(2.3f * (moveDirection * moveSpeed), Rb2D.velocity.y);
        }

        else if((isPlayerMoving) && (isSpeedFormEnabled) && (didSpeedBoostOccur))
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(4.8f * (moveDirection * moveSpeed), Rb2D.velocity.y);
            // sprite.color = Color.blue;

            
        }

        if(!isPlayerMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Rb2D.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(playerDeathClip, gameObject.transform.position);
            SceneManager.LoadScene(currentScene);
        }

        if((collision.gameObject.tag == "Platform") && (!didJumpOccur))
        {
            
            didJumpOccur = true;
        }

        if(collision.gameObject.name == "FallingDebris(Clone)")
        {
            AudioSource.PlayClipAtPoint(playerDeathClip, gameObject.transform.position);
            SceneManager.LoadScene(currentScene);
        }

        if(collision.gameObject.tag == "Pit")
        {
            AudioSource.PlayClipAtPoint(playerDeathClip, gameObject.transform.position);
            SceneManager.LoadScene(currentScene);
        }

        if(collision.gameObject.tag == "Goal")
        {
            currentScene++;
            LevelCheck();
            AudioSource.PlayClipAtPoint(goalClip, gameObject.transform.position);
            SceneManager.LoadScene(currentScene);
            
        }
        if(collision.gameObject.tag == "EndGame")
        {
            currentScene = 0;
            AudioSource.PlayClipAtPoint(goalClip, gameObject.transform.position);
            SceneManager.LoadScene(currentScene);
        }
    }

    private void LevelCheck()
    {
        if(currentScene == 2)
        {
            isLevelTwo = true;
        }

        if(currentScene == 3)
        {
            isLevelThree = true;
        }
    }

    private void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        jump = playerInput.currentActionMap.FindAction("Jump");
        speedForm = playerInput.currentActionMap.FindAction("SpeedForm");
        jumpForm = playerInput.currentActionMap.FindAction("JumpForm");
        speedBoost = playerInput.currentActionMap.FindAction("SpeedBoost");
        restart = playerInput.currentActionMap.FindAction("Restart");
        quit = playerInput.currentActionMap.FindAction("Quit");

        move.started += Move_started;
        move.canceled += Move_canceled;
        jump.started += Jump_started;
        speedForm.started += SpeedForm_started;
        jumpForm.started += JumpForm_started;
        speedBoost.started += SpeedBoost_started;
        restart.started += Restart_started;
        quit.started += Quit_started;
    }

   
   

   

    private void SpeedBoost_started(InputAction.CallbackContext obj)
    {
        if(currentScene == 3)
        {
            if (speedBoostLimit > 0)
            {
                if (isSpeedFormEnabled)
                {
                    didSpeedBoostOccur = true;
                    if (SpeedBoostRef == null)
                    {
                        SpeedBoostRef = StartCoroutine(SpeedBoostTimer());
                        speedBoostLimit--;
                        string temp = speedBoostLimit.ToString();
                        
                        if(speedBoostLimit == 0)
                        {
                            timesLeftSpeedBoostText.text = "Reached Limit of Speed Boosts";
                        }
                        else
                        {
                            print("Times left player can use Speed Boost: <color=green>" + speedBoostLimit + "</color>");
                            AudioSource.PlayClipAtPoint(speedBoostClip, gameObject.transform.position, 0.8f);
                            timesLeftSpeedBoostText.text = "Times left you can Speed boost: <color=green>" + speedBoostLimit.ToString() + "</color>";
                        }
                        
                        
                        

                    }
                }
            }
          
            else
            {
                print("Reached max number of speed boosts you can do");
                timesLeftSpeedBoostText.text = "Reached Limit of Speed Boosts";
            }

            
        }
       
        
    }

    public IEnumerator SpeedBoostTimer()
    {
       // anim.Play("Player_Speed_Boost");
        yield return new WaitForSeconds(0.5f);
        SpeedBoostRef = null;
        didSpeedBoostOccur = false;
       // sprite.color = Color.green;


    }

    private void JumpForm_started(InputAction.CallbackContext obj)
    {
        if((!isJumpFormEnabled) && (jumpFormLimit != 0))
        {
            if(isSpeedFormEnabled)
            {
                isSpeedFormEnabled = false;
                speedFormLimit--;
                if(speedBoostLimit == 0)
                {
                    timesLeftSpeedText.text = "Reached Limit of Speed";
                }
                else
                {
                    timesLeftSpeedText.text = "Times Left in Speed: " + speedFormLimit.ToString();
                }
                
            }
            isJumpFormEnabled = true;
            print("Jump form is enabled");
            AudioSource.PlayClipAtPoint(jumpFormClip, gameObject.transform.position);
           // sprite.color = jumpColor;
            currentFormText.text = "Current Form: <color=orange> Jump </color>"; 
        }
        else if(isJumpFormEnabled)
        {
            isJumpFormEnabled = false;
            print("Jump form is disabled");
           // sprite.color = baseColor;
            jumpFormLimit--;
            if(jumpFormLimit == 0)
            {
                print("reached max number of times jump form can be used");
                timesLeftJumpText.text = "Reached Limit of Jump";
            }
            else
            {
                print("Times left you can use Jump Form: " +  jumpFormLimit);
                timesLeftJumpText.text = "Times Left in Jump: " + jumpFormLimit.ToString();
            }
            AudioSource.PlayClipAtPoint(baseFormClip, gameObject.transform.position);
            currentFormText.text = "Current Form: Base";
        }
        
    }

    private void SpeedForm_started(InputAction.CallbackContext obj)
    {
        if ((!isSpeedFormEnabled) && (speedFormLimit != 0))
        {
            if(isJumpFormEnabled)
            {
                isJumpFormEnabled= false;
                jumpFormLimit--;
                if(jumpFormLimit == 0)
                {
                    timesLeftJumpText.text = "Reached Limit of Jump";
                }
                else
                {
                    timesLeftJumpText.text = "Times Left in Jump: " + jumpFormLimit.ToString();
                }
               

            }
            isSpeedFormEnabled = true;
            print("Speed form is enabled");
           // sprite.color = Color.green;
           AudioSource.PlayClipAtPoint(speedFormClip, gameObject.transform.position);
            currentFormText.text = "Current Form: <color=green> Speed </color>";
            speedBoostLimit = 5;
            timesLeftSpeedBoostText.text = "Times left you can Speed boost: <color=green>" + speedBoostLimit.ToString() + "</color>";
        }
        else if (isSpeedFormEnabled)
        {
            isSpeedFormEnabled = false;
            print("Speed form is disabled");
           // sprite.color = baseColor;
            speedFormLimit--;
            if(speedFormLimit == 0)
            {
                print("reached max number of times speed form can be used");
                timesLeftSpeedText.text = "Reached Limit of Speed";
            }
            else
            {
                print("Times left you can use Speed Form: " + speedFormLimit);
                timesLeftSpeedText.text = "Times Left in Speed: " + speedFormLimit.ToString();
            }
            AudioSource.PlayClipAtPoint(baseFormClip, gameObject.transform.position);
            currentFormText.text = "Current Form: Base";
        }
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(currentScene);
    }

    private void Jump_started(InputAction.CallbackContext obj)
    {
        // Base form jump height

        if(didJumpOccur && !(isJumpFormEnabled))
        {
            Rb2D.velocity = Vector2.zero;
            jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * Rb2D.gravityScale) * -2) * Rb2D.mass;
            Rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioSource.PlayClipAtPoint(playerJumpClip, gameObject.transform.position);
        }

        // Jump Form jump height

        else if(didJumpOccur && isJumpFormEnabled)
        {
            Rb2D.velocity = Vector2.zero;
            jumpForce = 2f * (Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * Rb2D.gravityScale) * -2) * Rb2D.mass);
            Rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isInAir = true;
            AudioSource.PlayClipAtPoint(playerJumpClip, gameObject.transform.position);

        }

        // Double Jump
        else if((!didJumpOccur ) && (isJumpFormEnabled) && (isInAir))
        {
            if(currentScene >= 2)
            {
                Rb2D.velocity = Vector2.zero;
                jumpForce = 2f * (Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * Rb2D.gravityScale) * -2) * Rb2D.mass);
                Rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isInAir = false;
                AudioSource.PlayClipAtPoint(playerJumpClip, gameObject.transform.position);
                print("double jump occurred");
            }
            
        }

        didJumpOccur = false;
        
        
      
    }



    private void Move_canceled(InputAction.CallbackContext obj)
    {
        isPlayerMoving = false;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        isPlayerMoving = true;
    }

    public void OnDestroy()
    {
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        jump.started -= Jump_started;
        jumpForm.started -= JumpForm_started;
        speedForm.started -= SpeedForm_started;
        speedBoost.started -= SpeedBoost_started;
        restart.started -= Restart_started;
        quit.started -= Quit_started;
    }
}



