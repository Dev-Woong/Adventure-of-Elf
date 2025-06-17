using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterPink: PoolAble
{
    public Transform target;
    
    BoxCollider2D col;
    Animator anim;
    Transform tr;
    public float speed = 2f;
   
    Vector3 StartPos;
    public float initHp = 500;
    public float currHp = 500;
    bool isPatrol = true;
    bool isTrace = false;
    bool isAttack = false;
    public float dir = 1;
    public float atk;
    float atkCoolTime = 0;
    float rockSpeed = 3;
    public static float Atk;
    public float def = 5;
    public GameObject MonsterUIPref;
    public GameObject currHpBarPref;
    public GameObject initHpBarPref;

    public GameObject DmgTextPref;
    public GameObject CriticalDmgTextPref;
    //public GameObject monName;
    RectTransform currHpBarRect;
    RectTransform initHpBarRect;

    public static Vector3 SetRockPos;
    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartPos = transform.position;
        initHpBarRect=Instantiate(initHpBarPref, MonsterUIPref.transform).GetComponent <RectTransform>();
        currHpBarRect = Instantiate(currHpBarPref, MonsterUIPref.transform).GetComponent<RectTransform>();
        SetRectSize(currHpBarRect,150, 15);
        SetRectSize(initHpBarRect, 150, 15);
        MonAtk();
    }
    void Update()
    {
        atkCoolTime += Time.deltaTime;
        Patrol();
        //SearchPlayer();
        TracePlayer();
        Attack();
        MonAtk();
        MonHpBar();
        //MonName();
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6.7f, 12.7f), transform.position.y);
        
    }
    void SetRectSize(RectTransform Rect,float width, float height)
    {
        Rect.sizeDelta = new Vector2(width, height);
    }
    void MonHpBar()
    {

        Vector3 HpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x,transform.position.y+0.5f,0));
        
        initHpBarRect.transform.position = HpBarPos;
        currHpBarRect.transform.position = HpBarPos;
        float monHpAmount = currHp / initHp;
        currHpBarRect.GetComponent<Image>().fillAmount = monHpAmount;
    }
    void MonName()
    {
        //monName.text = $"Lv.1 가라테 울프";
    }
    void MonAtk()
    {
        Atk = atk;
    }
    void TracePlayer() // 플레이어 추적로직
    {
        Vector3 TracePos = new Vector3(target.position.x, transform.position.y, transform.position.z);
        float distRange = target.position.x - transform.position.x;
        if (isTrace == true)
        {
            if (distRange >= 5f || distRange <= -5f)
            {
                
                transform.Translate((TracePos - transform.position).normalized * speed * Time.deltaTime);
                if (TracePos.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                }
                else if (TracePos.x >= transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    void Patrol()
    {
        if (isPatrol == true)
        {
            anim.SetBool("isPatrol", true);
            transform.Translate(Vector3.left * dir * Time.deltaTime * speed);
            if (transform.position.x >= StartPos.x + 3)
            {
                dir *= -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x <= StartPos.x - 3)
            {
                dir *= -1;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    void Attack()
    {
        if (isAttack == true)
        {
            float AttackRange = Vector2.Distance(target.transform.position, transform.position);
            if ((AttackRange <=  5f || -AttackRange >= 5f)  )
            {
                isTrace = false;
                if (atkCoolTime >= 2f)
                {
                    anim.SetTrigger("Attack");
                    if (target.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    float rockScale = transform.localScale.x > 0 ? 1f : -1f;
                    Vector3 SpawnRockPos = transform.right / 5 * transform.localScale.x;
                    var rockThrow = MonsterPoolManager.instance.GetObject("Rock");
                    rockThrow.transform.position = transform.position;
                    rockThrow.GetComponent<Rigidbody2D>().AddForce(SpawnRockPos.normalized * rockSpeed, ForceMode2D.Impulse);
                    atkCoolTime = 0;
                }
            }
            else if (AttackRange > 6f)
            {
                isTrace = true;
                anim.SetBool("isAttack", false);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            float rand = Random.Range(0, 100);
            isTrace = true;
            isAttack = true;
            isPatrol = false;
            anim.SetTrigger("Hit");
            if (currHp > 0)
            {
                if (rand <= ArcherCtrl.s_instance.playerCritProb)
                {
                    float finalCriDmg = ArcherCtrl.s_instance.CriticalAtkDamage() - def;
                    currHp -= finalCriDmg;

                    GameObject CriticalDmgText = Instantiate(CriticalDmgTextPref, MonsterUIPref.transform);
                    CriticalDmgText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
                    CriticalDmgText.GetComponent<DamageText>().damage = finalCriDmg.ToString();
                }
                else
                {
                    float finalDmg = ArcherCtrl.s_instance.AtkDamage() - def;
                    currHp -= finalDmg;

                    GameObject DmgText = Instantiate(DmgTextPref, MonsterUIPref.transform);
                    DmgText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
                    DmgText.GetComponent<DamageText>().damage = finalDmg.ToString();
                }
            }
            else
            {
                isTrace = false;
                isAttack = false;
                isPatrol = true;
                ReleaseObject();
                ArcherCtrl.s_instance.currExp += 15;
                MonSpawnManager.monCount--;
                currHp = initHp;
            }
        }
    }
}
