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
    int score;//�����{�b��ĴXlevel
    float scoreTime;//�����{�b�L�F�h�[�ɶ�
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
    void OnCollisionEnter2D(Collision2D other)  //other�O���I���쪺�F�� 
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("����Normal");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("����Nails");
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Ceiling")
        {
            Debug.Log("����Ѫ�O");
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
            Debug.Log("�A��F!");
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
        //�qHp�}�l�]��Hp[9]
        {
            if (Hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
                //�ѩ�i�O�q�s�}�l�]�A�ҥH���]��q���@���ܡA�N�n���L��ܤ@��A�N�O��HpBar���U�l���󪺲Ĺs�Ӥl�������
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
                //SetActive(false)�����
            }
        }
    }
    void UpdateScore()
    {
        scoreTime += Time.deltaTime;//��Time.deltaTime�ӭp��ɶ��A�]�����O�C��Update��k�Q�I�s�쪺���j�ɶ�
        if (scoreTime > 2f)  //�u�nscoreTime�W�L2��A�N�����+1��s���r�W�A���k�s�ɶ�
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
