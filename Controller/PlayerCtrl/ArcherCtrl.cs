using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArcherCtrl : MonoBehaviour
{
    public static ArcherCtrl s_instance;
    public static ArcherCtrl Instance
    {
        get
        {
            if (s_instance == null) return null;
            return s_instance;
        }

    }
    void Awake()
    {
        if (Instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    // 컴포넌트 변수
    Animator anim;
    Transform tr;
    SpriteRenderer spr;

    // 이동 관련 변수
    float h;
    float doubleJumpCount = 0;

    // 화살 속도 
    public float arrowSpeed = 50;

    // 불 변수
    bool isGround = true;
    bool isUnHit = false;
    bool isJumping;
    public bool moveAble = true;

    // 플레이어 레벨업시 증가 스탯변수
    private float playerHP = 100;
    private float playerMP = 100;
    private float playerDef = 5;
    private float playerAtk = 10;
    public float playerSpeed = 3f;
    public float playerCritProb = 10;
    public float playerCritDmg = 150f;
    // 플레이어 아이템 장착 스탯
    public float itemHP = 0;
    public float itemMP = 0;
    public float itemDef = 0;
    public float itemAtk = 0;
    public float itemCritProb = 0;
    public float itemCritDmg = 0;
    public float itemSpeed = 0;

    // 최종 스탯
    public float initHP;
    public float initMP;
    public float currHP = 100;
    public float currMP = 100;
    public float finalDef = 5;
    public float finalSpeed;
    public float finalCritProb;
    public float finalCritDmg;

    public float minDmg = 8;
    public float maxDmg = 12;
    protected float atkDmg;
    
    public float criticalDmg;


    private float arDelay; //  스킬 고정 딜레이
    private float atkDelay; // 어택 딜레이 
    private float buffCoolTime=0;
    public  float buffHoldingTime=0;


    public int level = 1;
    public float currExp = 0;
    public float maxExp = 10;

    //이미지 (체력, 마나 , 경험치)
    public Image currHpImage;
    public Image currMpImage;
    public Image currExpImage;

    //텍스트
    public Text currHpText;
    public Text currMpText;
    public Text currExpText;
    public Text levelText;
    public Text nameText;
    public Text atkText;
    public Text defText;
    public Text speedText;
    public Text criticalProbText;
    public Text criticalDmgText;

    // 게임오브젝트(UI)
    public GameObject StatusUI;
    public GameObject PlayerDmgTextPref;
    public GameObject PlayerDieUI;

    //UI버튼
    public Button ReviveAtDeathPosBtn;
    public Button ReviveAtStartPosBtn;

    //위치저장 벡터
    Vector2 StartPos = new Vector2(-6f, -2.4f);
    Vector2 EndPos = new Vector2(11, -2.5f);
    Vector2 DeathPos;
    public Vector2 SetArrowPos;
    public Vector3 SetArcherPos;
    float asPosY = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        spr = GetComponent<SpriteRenderer>();
        transform.position = StartPos;
        StatusUI.SetActive(false);
        PlayerDieUI.SetActive(false);
        ReviveAtDeathPosBtn.onClick.AddListener(OnReviveAtDeathPosClick);
        ReviveAtStartPosBtn.onClick.AddListener(OnReviveAtStartPosClick);

    }

    // Update is called once per frame
    void Update()
    {
        SetArcherPos = transform.position;
        PlayerFinalStat();
        StatusInfo();
        HpBar();
        MpBar();
        ExpBar();
        LevelUp();
        
        
        if (moveAble == true)
        {
            Move();
            AttackProcess();
            Jump();
            AbleSceneChange();
            ArrowStorm();
            BuffProcess();
        }
    }
    void BuffProcess()
    {
        if (Input.GetKeyDown(KeyCode.Q) )
        {
            if (buffCoolTime <= 0)
            {
                buffHoldingTime = 20;
                buffCoolTime = 30;
                float buffEffectScale = tr.localScale.x > 0 ? 1f : -1f;

                var buffEffect = ObjectPoolManager.instance.GetObject("BuffEffect");
                buffEffect.transform.position = tr.position * tr.localScale.x;
                buffEffect.transform.localScale = new Vector3(buffEffectScale, 1, 1);

                buffCoolTime -= Time.deltaTime;
                buffHoldingTime -= Time.deltaTime;
                if (buffHoldingTime >= 0f)
                {
                    minDmg += 20;
                    maxDmg += 20;
                }
                else if (buffHoldingTime == 0f)
                {
                    minDmg -= 20;
                    maxDmg -= 20;
                }
            }

            else { Debug.LogError(" 아직 쿨타임입니다 ! "); }
        }
    }
    void PlayerFinalStat()
    {
        initHP = playerHP + itemHP;
        initMP = playerMP + itemMP;
        minDmg = Mathf.RoundToInt((playerAtk + itemAtk) * 0.8f);
        maxDmg = Mathf.RoundToInt((playerAtk + itemAtk) * 1.2f);
        finalDef = playerDef + itemDef;
        finalSpeed = playerSpeed + itemSpeed;   
        finalCritProb = playerCritProb + itemCritProb;
        finalCritDmg = playerCritDmg + itemCritDmg;
    }
    void LevelUp()
    {
        levelText.text = $"Lv : {level}";
        if (currExp >= maxExp)
        {
            level += 1;
            currExp = 0;
            maxExp += 30 * level;
            playerHP += 20* level;
            playerMP += 10 * level;
            playerAtk += 3 + (2 * level);
            playerDef += 1 * level;
            PlayerFinalStat();
            Vector3 EffectPos = tr.position + new Vector3(0, 1f, 0);
            var LvUpEffect = ObjectPoolManager.instance.GetObject("LevelUp");
            LvUpEffect.transform.position = EffectPos;
            currHP = initHP;
            currMP = initMP;

        }
    }
    #region PlayerAtkValue
    public int AtkDamage()
    {
        atkDmg = Random.Range(minDmg, maxDmg);
        return Mathf.RoundToInt(atkDmg);
    }
    public bool CriticalCalc(float _monCritResistance)
    {
        float rand = Random.Range(0, 100);
        if (rand <= playerCritProb - _monCritResistance)
        {
            return true;
        }
        else { return false; }
    }
    public int CriticalAtkDamage()
    {
        criticalDmg = Mathf.RoundToInt(Random.Range(minDmg * finalCritDmg, maxDmg * finalCritDmg) / 100);
        return Mathf.RoundToInt(criticalDmg);
    }
    #endregion
    #region PlayerUI
    void HpBar()
    {
        float HpAmount = currHP / initHP;
        currHpImage.fillAmount = HpAmount;
        currHpText.text = $"{currHP}/{initHP}";
    }

    void MpBar()
    {
        float MpAmount = currMP / initMP;
        currMpImage.fillAmount = MpAmount;
        currMpText.text = $"{currMP}/{initMP}";
    }
    void ExpBar()
    {
        float ExpAmount = currExp / maxExp;
        currExpImage.fillAmount = ExpAmount;
        currExpText.text = $"{currExp}/{maxExp}";
    }
    void OnReviveAtDeathPosClick()
    {
        currHP = initHP;
        transform.position = DeathPos;
        PlayerDieUI.SetActive(false);
        gameObject.SetActive(true);
    }
    void OnReviveAtStartPosClick()
    {
        currHP = initHP;
        transform.position = StartPos;
        PlayerDieUI.SetActive(false);
        gameObject.SetActive(true);
    }
    
    void StatusInfo()
    {
        nameText.text = $"이름 : 미정";
        atkText.text = $"공격력 : {minDmg}({Mathf.RoundToInt(playerAtk * 0.8f)}+{itemAtk})~{maxDmg}({Mathf.RoundToInt(playerAtk*1.2f)}+{itemAtk})";
        defText.text = $"방어력 : {finalDef} ({playerDef}+{itemDef})";
        speedText.text = $"이동속도 : {finalSpeed} ({finalSpeed-itemSpeed}+{itemSpeed})";
        criticalProbText.text = $"크확 : {finalCritProb}({playerCritProb}+{itemCritProb})%";
        criticalDmgText.text = $"크증뎀: {finalCritDmg}({playerCritDmg}+{itemCritDmg})%";
        criticalDmgText.fontSize = 130;
        if (Input.GetKeyDown(KeyCode.M) && !StatusUI.activeSelf)
        {
            StatusUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && StatusUI.activeSelf)
        {
            StatusUI.SetActive(false);
        }
    }
    
    #endregion
    #region PlayerMoveProcess

    void Move()
    {
        if (Input.GetButton("Horizontal"))
        {
            h = Input.GetAxisRaw("Horizontal");
            tr.Translate(Vector2.right * h * Time.deltaTime * finalSpeed);
            tr.position = new Vector2(Mathf.Clamp(tr.position.x, -6.7f, 12.7f), tr.position.y);
            anim.SetFloat("Move",1);
            if (h > 0)
            {
                tr.localScale = new Vector3(1, 1, 1);
            }
            else if (h < 0)
            {
                tr.localScale = new Vector3(-1, 1, 1);
            }
        }
        else { anim.SetFloat("Move", 0); }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGround == true && !isJumping)
        {
            isGround = false;

            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 200);
            anim.SetTrigger("Attack");
            isJumping = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && doubleJumpCount <= 1 && currMP > 0)
        {
            currMP -= 2;
            float jumpEffectScale = tr.localScale.x > 0 ? 1f : -1f;

            Vector3 SpawnJumpEffectPos = tr.right / 20 * tr.localScale.x;
            var JumpEffect = ObjectPoolManager.instance.GetObject("JumpEffect");
            JumpEffect.transform.localScale = new Vector3(jumpEffectScale, 0.5f, 1);
            JumpEffect.transform.position = tr.position + SpawnJumpEffectPos;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * h, 0.5f) * 200);
            doubleJumpCount++;
        }
    }
    void AbleSceneChange()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (GameManager.Instance.isNextSceneChange == true)
            {
                GameManager.Instance.SceneNum++;
                MonSpawnManager.monCount = 0;
                gameObject.transform.position = StartPos;
                SceneManager.LoadScene(GameManager.Instance.SceneNum);
            }
            if (GameManager.Instance.isBackSceneChange == true)
            {
                MonSpawnManager.monCount = 0;
                GameManager.Instance.SceneNum--;
                gameObject.transform.position = EndPos;
                SceneManager.LoadScene(GameManager.Instance.SceneNum);
            }
        }
    }
    #endregion
    #region PlayerAttackProcess
    void ArrowStormEffect()
    {
        float EffectScale = tr.localScale.x > 0 ? 1f : -1f;
        Vector3 SpawnEffectPos = tr.right/1.5f * tr.localScale.x;

        var skillEffect = ObjectPoolManager.instance.GetObject("SkillEffect");
        skillEffect.transform.position = tr.position + SpawnEffectPos;
        skillEffect.transform.localScale = new Vector3(EffectScale, 1, 1);
    }

    void ArrowStorm()
    {
        arDelay += Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Z) /*&& level >= 2*/ && arDelay >= 0.15f/*&&currMp>0*/)
        {
            ArrowStormEffect();
            anim.SetTrigger("Attack");
            currMP -= 1;
            SetArrowPos = tr.position;
            
            if (asPosY== 0) { asPosY += 0.15f; }
            else if (asPosY == 0.15f) { asPosY -= 0.15f; }
            
            float ARSclae = tr.localScale.x > 0 ? 1f : -1f;
            
            Vector3 SpawnARPos = tr.right/5 * tr.localScale.x;
            Vector3 _offset = new Vector3(0, 0.075f, 0);
            Vector3 setPosY = new Vector3(0, asPosY, 0);

            var ArrowStorm = ObjectPoolManager.instance.GetObject("ArrowStorm");
            ArrowStorm.transform.localScale = new Vector3(ARSclae, 1, 1);
            ArrowStorm.transform.position = tr.position + SpawnARPos - setPosY + _offset;
            
            ArrowStorm.GetComponent<Rigidbody2D>().AddForce(SpawnARPos.normalized  * arrowSpeed * 1.5f, ForceMode2D.Impulse);
            arDelay = 0;
        }
    }
    void AttackEffectPooling()
    {
        float attackEffectScale = tr.localScale.x > 0 ? -1f : 1f;
        Vector3 SpawnAttackEffectPos = tr.right / 20 * tr.localScale.x;

        var atkEffect = ObjectPoolManager.instance.GetObject("AtkEffect");

        atkEffect.transform.localScale = new Vector3(attackEffectScale, 1, 1);
        atkEffect.transform.position = tr.position + SpawnAttackEffectPos;
    }
    void AttackPooling()
    {
        float arrowScale = tr.localScale.x > 0 ? 1f : -1f;
        Vector3 SpawnArrowPos = tr.right / 5 * tr.localScale.x;
        var arrow = ObjectPoolManager.instance.GetObject("Arrow");
        arrow.transform.localScale = new Vector3(arrowScale, 1, 1);
        arrow.transform.position = tr.position + SpawnArrowPos;
        arrow.GetComponent<Rigidbody2D>().AddForce(SpawnArrowPos.normalized * arrowSpeed, ForceMode2D.Impulse); // 화살  오브젝트 풀링
    }
    void AttackProcess()
    {
        atkDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftControl)&&atkDelay >=0.5f)
        {
            anim.SetTrigger("Attack");
            SetArrowPos = tr.position;
             // 공격이펙트 오브젝트풀링
            AttackEffectPooling();
            //  화살 오브젝트 풀링
            AttackPooling();
            atkDelay = 0f;
        }
    }
    #endregion
    #region PlayerGetItem
    //
    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            isGround = true;
            isJumping = false;
            doubleJumpCount = 0;
        }
    }
    public void OnDamage(float causerAtk)
    {
       
        float HitDmg = causerAtk - finalDef;
        if (HitDmg <= 0) { HitDmg = 0; }
        currHP -= HitDmg;
        if (currHP > 0)
        {
            if (HitDmg <= 0)
            {
                GameObject PlayerMissText = Instantiate(PlayerDmgTextPref, tr.transform.position, Quaternion.identity);
                PlayerMissText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
                PlayerMissText.GetComponent<DamageText>().damage = "Miss";
            }
            else
            {
                GameObject PlayerDmgText = Instantiate(PlayerDmgTextPref, tr.transform.position, Quaternion.identity);
                PlayerDmgText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
                PlayerDmgText.GetComponent<DamageText>().damage = HitDmg.ToString();
            }
            isUnHit = true;
            StartCoroutine(UnHitable());
        }
        else if (currHP <=0) { currHP = 0; HpBar(); DeathPos = transform.position; gameObject.SetActive(false); PlayerDieUI.SetActive(true);  }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Monster") && isUnHit == false)
        {
            OnDamage(Monster.Atk);
        }
        if (collision.CompareTag("Monster_Pink") && isUnHit == false)
        {
            OnDamage(MonsterPink.Atk);
        }

    }
    IEnumerator UnHitable()
    {
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
            {
                spr.color = new Color32(255, 255, 255, 90);
            }
            else { spr.color = new Color32(255, 255, 255, 200); }
            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        spr.color = new Color32(255, 255, 255, 255);
        isUnHit = false;
        yield return null;
    }
}
