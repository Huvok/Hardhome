using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    public float speed = 6f;

    Animator anim;
    Rigidbody2D rb2d;
    Vector2 v2Mov;
    Vector2 v2ForceReceivedDirection;
    int intForceReceived;
    float fForceReceivedTimer = 0.0f;

    PolygonCollider2D attackCollider;
    DialogueManager dialogueManager;
    CutsceneManager cutsceneManager;

    public GameObject goInitialMap;
    HealthSystem healthSystem;
    InvincibilityTimer invincibilityTimer;
    public GameObject goOctahedronOfTranscendence;
    public int intOctahedronSpeed;
    GameObject goCurrentOctahedron;

    Vector2 v2ThrowingTo;

    Coroutine coroutineThrowingOctahedron;

    [SerializeField]
    GameObject goPlayerGhost;

    bool boolWaitingForThrow;
    public bool boolDisableControls;
    BoxCollider2D bc2d;
    bool boolAttacking;

    public AudioClip[] arrAudioClip;
    public AudioClip[] arrAudioClipSwordSlash;
    public AudioClip audioClipHealingPotion;
    AudioSource audioSourceFootsteps;
    AudioSource audioSourceSwordSlash;
    AudioSource audioSourceMisc;
    AudioSource audioSourceOctahedron;
    public AudioClip[] arrAudioClipOctahedron;

    void Start ()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        audioSourceFootsteps = transform.GetChild(1).GetComponent<AudioSource>();
        audioSourceSwordSlash = transform.GetChild(2).GetComponent<AudioSource>();
        audioSourceMisc = transform.GetChild(3).GetComponent<AudioSource>();
        audioSourceOctahedron = transform.GetChild(5).GetComponent<AudioSource>();

        attackCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;

        Camera.main.GetComponent<MainCamera>().subSetBounds(goInitialMap);
        dialogueManager = GameObject.FindGameObjectWithTag("Dialogue Manager").GetComponent<DialogueManager>();

        cutsceneManager = GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>();

        healthSystem = GetComponent<HealthSystem>();
        invincibilityTimer = GetComponent<InvincibilityTimer>();
        boolWaitingForThrow = false;
        MapManager.strCurrentMap = goInitialMap.name;
	}

	void Update ()
    {
        if (!DialogueManager.boolOnDialogue &&
            !boolDisableControls)
        {
            v2Mov = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (v2Mov != Vector2.zero)
            {
                anim.SetFloat("movX", v2Mov.x);
                anim.SetFloat("movY", v2Mov.y);
                anim.SetBool("boolPlayerWalking", true);
                if (!audioSourceFootsteps.isPlaying)
                {
                    audioSourceFootsteps.clip = arrAudioClip[Random.Range(0, arrAudioClip.Length)];
                    audioSourceFootsteps.Play();
                }
            }
            else
            {
                anim.SetBool("boolPlayerWalking", false);
            }

            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            boolAttacking = animStateInfo.IsName("Player Attack");

            if (Input.GetMouseButtonDown(0) && !boolAttacking)
            {
                anim.SetTrigger("Attack");
                audioSourceSwordSlash.clip = arrAudioClipSwordSlash[Random.Range(0, arrAudioClipSwordSlash.Length)];
                audioSourceSwordSlash.Play();
            }

            if (Input.GetMouseButtonDown(1))
            {
                v2ThrowingTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (ItemManager.octahedron_state == ItemManager.OCTAHEDRON_STATE.PLAYER)
                {
                    audioSourceOctahedron.clip = arrAudioClipOctahedron[0];
                    audioSourceOctahedron.Play();
                    boolWaitingForThrow = true;
                    ItemManager.octahedron_state = ItemManager.OCTAHEDRON_STATE.GOING;
                    anim.SetTrigger("triggerThrowOctahedron");
                }
                else if (ItemManager.octahedron_state == ItemManager.OCTAHEDRON_STATE.AWAY ||
                    ItemManager.octahedron_state == ItemManager.OCTAHEDRON_STATE.GOING)
                {
                    audioSourceOctahedron.clip = arrAudioClipOctahedron[1];
                    audioSourceOctahedron.Play();
                    subGoToOctahedron();
                }
            }

            if (v2Mov != Vector2.zero)
            {
                if (v2Mov.x == -1)
                {
                    attackCollider.offset = new Vector2(0f, 0f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, -90);
                }
                else if (v2Mov.x == 1)
                {
                    attackCollider.offset = new Vector2(0f, 0f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                else if (v2Mov.y == 1)
                {
                    attackCollider.offset = new Vector2(0f, -.3f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 180);
                }
                else if (v2Mov.y == -1)
                {
                    attackCollider.offset = new Vector2(0f, -.3f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }

            if (boolAttacking)
            {
                float numPlaybackTime = animStateInfo.normalizedTime;
                if (numPlaybackTime > .33f &&
                    numPlaybackTime < .66f)
                {
                    if (!attackCollider.enabled)
                        attackCollider.enabled = true;
                }
                else
                {
                    if (attackCollider.enabled)
                        attackCollider.enabled = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (ItemManager.intPotions > 0)
                {
                    audioSourceMisc.clip = audioClipHealingPotion;
                    audioSourceMisc.Play();
                    ItemManager.intPotions--;
                    healthSystem.subRecoverHP(ItemManager.intPotionStrength);
                    UIManager.subRedrawPotions();
                }
            }

            if (Input.GetKeyDown(KeyCode.E) &&
                (ItemManager.octahedron_state == ItemManager.OCTAHEDRON_STATE.AWAY ||
                ItemManager.octahedron_state == ItemManager.OCTAHEDRON_STATE.GOING))
            {
                audioSourceOctahedron.clip = arrAudioClipOctahedron[2];
                audioSourceOctahedron.Play();
                subRecoverOctahedron();
            }
        }
	}

    public void subDisableControls(bool boolDisable)
    {
        if (boolDisable)
        {
            v2Mov = Vector2.zero;
            boolDisableControls = true;
        }
    }

    public void subThrowOctahedron()
    {
        coroutineThrowingOctahedron = StartCoroutine(subMoveOctahedronToPoint(v2ThrowingTo));
    }

    private IEnumerator subMoveOctahedronToPoint(Vector2 v2To)
    {
        if (goCurrentOctahedron == null)
            Instantiate(goOctahedronOfTranscendence, transform.position, transform.rotation);
        goCurrentOctahedron = GameObject.FindGameObjectWithTag("Octahedron of Transcendence");
        boolWaitingForThrow = false;
        while ((v2To - (Vector2)goCurrentOctahedron.transform.position).sqrMagnitude > .1)
        {
            goCurrentOctahedron.transform.position = Vector2.MoveTowards(goCurrentOctahedron.transform.position, v2To, Time.deltaTime * intOctahedronSpeed);
            yield return null;
        }

        ItemManager.octahedron_state = ItemManager.OCTAHEDRON_STATE.AWAY;
    }

    public void subGoToOctahedron()
    {
        boolDisableControls = true;
        bc2d.enabled = false;
        if (boolAttacking)
        {
            boolAttacking = false;
            attackCollider.enabled = true;
        }

        StartCoroutine(subMoveToOctahedron());
    }

    private IEnumerator subMoveToOctahedron()
    {
        goCurrentOctahedron = GameObject.FindGameObjectWithTag("Octahedron of Transcendence");
        while (goCurrentOctahedron == null)
        {
            goCurrentOctahedron = GameObject.FindGameObjectWithTag("Octahedron of Transcendence");
            yield return null;
        }

        while ((transform.position - goCurrentOctahedron.transform.position).sqrMagnitude > .1)
        {
            Instantiate(goPlayerGhost, transform.position, transform.rotation);
            transform.position = Vector2.MoveTowards(transform.position, goCurrentOctahedron.transform.position, Time.deltaTime * intOctahedronSpeed);
            yield return null;
        }

        ItemManager.octahedron_state = ItemManager.OCTAHEDRON_STATE.PLAYER;
        Destroy(goCurrentOctahedron);
        bc2d.enabled = true;
        boolDisableControls = false;
        if (attackCollider.enabled)
            attackCollider.enabled = false;
    }

    private void subRecoverOctahedron()
    {
        ItemManager.octahedron_state = ItemManager.OCTAHEDRON_STATE.COMING;
        StartCoroutine(subMoveOctahedronToMe());
    }

    private IEnumerator subMoveOctahedronToMe()
    {
        while (boolWaitingForThrow)
        {
            yield return null;
        }

        goCurrentOctahedron = GameObject.FindGameObjectWithTag("Octahedron of Transcendence");

        while ((transform.position - goCurrentOctahedron.transform.position).sqrMagnitude > .01)
        {
            if (coroutineThrowingOctahedron != null)
                StopCoroutine(coroutineThrowingOctahedron);

            goCurrentOctahedron.transform.position = Vector2.MoveTowards(goCurrentOctahedron.transform.position, transform.position, Time.deltaTime * intOctahedronSpeed);
            yield return null;
        }
        
        if (goCurrentOctahedron != null)
        {
            Destroy(goCurrentOctahedron.transform.GetChild(0).gameObject, 1.0f);
            goCurrentOctahedron.transform.DetachChildren();
            Destroy(goCurrentOctahedron);
        }
            
        ItemManager.octahedron_state = ItemManager.OCTAHEDRON_STATE.PLAYER;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("Damage") &&
            !invincibilityTimer.boolIsInvincible)
        {
            invincibilityTimer.fInvincibleTime = 1.0f;
            invincibilityTimer.boolIsInvincible = true;
            string strSource = collision.name.Substring(collision.name.IndexOf("-") + 1);
            int intDamage = DamageManager.intGetDamage(strSource);
            healthSystem.subReceiveDamage(intDamage);
            anim.Play("Receive_Damage");

            v2ForceReceivedDirection = collision.transform.position - transform.position;
            v2ForceReceivedDirection = -v2ForceReceivedDirection.normalized;
            intForceReceived = DamageManager.intGetForce(strSource);
            fForceReceivedTimer = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + v2Mov * speed * Time.deltaTime);
        if (fForceReceivedTimer > 0.0f)
        {
            fForceReceivedTimer -= Time.deltaTime;
            rb2d.MovePosition(rb2d.position + v2ForceReceivedDirection * intForceReceived * Time.deltaTime);
        }
    }

    public void subTriggerDialogue(string strDialogue)
    {
        cutsceneManager.subTriggerDialogue(strDialogue);
    }

    public void subToggleSpriteOn(string str)
    {
        cutsceneManager.subToggleSpriteOn(str);
    }

    public void subToggleSpriteOff(string str)
    {
        cutsceneManager.subToggleSpriteOff(str);
    }

    public void subTriggerNextOnQueue()
    {
        cutsceneManager.subTriggerNextOnQueue();
    }

    public void subUnlockControls()
    {
        cutsceneManager.subUnlockControls();
    }

    public void subTriggerEvent(string str)
    {
        cutsceneManager.subTriggerEvent(str);
    }
}
