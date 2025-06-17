using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Monster : PoolAble
{
    public Transform target;
    
    BoxCollider2D col;
    Animator anim;
    Transform tr;
    public float stopDistance;
    //public float AttackDistance;
    public float speed = 2f;
    Vector3 StartPos;
    public float initHp = 500;
    public float currHp = 500;
    
    bool isPatrol = true;
    bool isTrace = false;
    bool isAttack = false;
   
    public float dir = 1;
    public float atk;
    public static float Atk;
    public float def = 2;
    public float critResist = 2;
    public GameObject MonsterUIPref;
    public GameObject CurrHpBarPref;
    public GameObject InitHpBarPref;
    public GameObject DmgTextPref;
    public GameObject CriticalDmgTextPref;
    public GameObject[] DropPortionPref;
    public GameObject[] DropEquipmentItemPref;
    //public GameObject monName;
    RectTransform currHpBarRect;
    RectTransform initHpBarRect;

    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartPos = transform.position;
        initHpBarRect=Instantiate(InitHpBarPref, MonsterUIPref.transform).GetComponent <RectTransform>();
        currHpBarRect = Instantiate(CurrHpBarPref, MonsterUIPref.transform).GetComponent<RectTransform>();
        SetRectSize(currHpBarRect,150, 15);
        SetRectSize(initHpBarRect, 150, 15);
    }
    void Update()
    {
       
        Patrol();
        
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
        if (isTrace == true)
        {
            transform.Translate((TracePos - transform.position).normalized * speed * Time.deltaTime);
            if (TracePos.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (TracePos.x >= transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (transform.position.x <= StartPos.x - 3)
            {
                dir *= -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    void Attack()
    {
        float AttackRange = Vector2.Distance(target.transform.position, this.transform.position);

        if (AttackRange <= col.size.x * 1.2f && isAttack == true)
        {
            anim.SetBool("isAttack", true);

        }
        else if (AttackRange >= col.size.x * 1.2f && isAttack == true)
        {
            anim.SetBool("isAttack", false);
        }
    }
    void MonDie(int dropExp)
    {
        isTrace = false;
        isAttack = false;
        isPatrol = true;
        ReleaseObject();
        ArcherCtrl.s_instance.currExp += dropExp;
        MonSpawnManager.monCount--;
        currHp = initHp;
    }
    public void DropItem(int _itemID)
    {
        for (int i = 0; i < DatabaseManager.instance.itemList.Count; i++)
        {
            if (_itemID == DatabaseManager.instance.itemList[i].itemID)
            {

            } 
        }
                        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            isTrace =true;
            isAttack = true;
            isPatrol = false;
            anim.SetTrigger("Hit");
            if (currHp > 0)
            {
                if ( ArcherCtrl.s_instance.CriticalCalc(critResist))
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
                MonDie(5);
                Vector3 itemDropPos = new Vector3(tr.position.x, tr.position.y - 0.3f, tr.position.z);
                int dropEquipItemProb = Random.Range(0, 10); 
                int dropPortionNum = Random.Range(0, DropPortionPref.Length); // 0= HP포션, 1=MP포션
                int dropPortionProb = Random.Range(0, 5);
                if (dropPortionProb <= 1)
                {
                    Instantiate(DropPortionPref[dropPortionNum], itemDropPos, Quaternion.identity);
                }
                if (dropEquipItemProb <= 8)
                {
                    int equipDropNum = Random.Range(0, DropEquipmentItemPref.Length); 
                    Instantiate(DropEquipmentItemPref[equipDropNum], itemDropPos, Quaternion.identity);
                    Debug.Log($"드랍된 장비 : {DropEquipmentItemPref[equipDropNum]}");
                }

               
            }
        }
    }
  
}
