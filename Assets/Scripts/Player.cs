using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int Hp;
    GameObject currentFloor;
    [SerializeField] GameObject HpBar;
    [SerializeField] TextMeshProUGUI scoreText;
    int score;//紀錄現在到第幾level
    float scoreTime;//紀錄現在過了多久時間
    Animator anim;
    SpriteRenderer render;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;
    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0;
        anim= GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed*Time.deltaTime, 0, 0);
            render.flipX = false;
            anim.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpeed*Time.deltaTime, 0, 0);
            render.flipX = true;
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
        UpdateScore();

    }
    void OnCollisionEnter2D(Collision2D other)  //other是指碰撞到的東西 
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("撞到Normal");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("撞到Nails");
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Ceiling")
        {
            Debug.Log("撞到天花板");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathLine")
        {
            Debug.Log("你輸了!");
            Die();
        }
    }
    void ModifyHp(int num)
    {
        Hp += num;
        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
        UpdateHpBar();
    }
    void UpdateHpBar()
    {
        for (int i = 0; i < HpBar.transform.childCount; i++)
        //從Hp開始跑到Hp[9]
        {
            if (Hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
                //由於i是從零開始跑，所以假設血量為一的話，就要讓他顯示一格，就是讓HpBar底下子物件的第零個子物件顯示
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
                //SetActive(false)表不顯示
            }
        }
    }
    void UpdateScore()
    {
        scoreTime += Time.deltaTime;//用Time.deltaTime來計算時間，因為它是每次Update方法被呼叫到的間隔時間
        if (scoreTime > 2f)  //只要scoreTime超過2秒，就把分數+1更新到文字上，並歸零時間
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "Level " + score.ToString();
        }
    }
    void Die()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }
    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
