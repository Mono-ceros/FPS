using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    AudioSource audioSource;
    Animator animator;
    Transform playerTr;
    Transform enemyTr;

    readonly int hashFire = Animator.StringToHash("Fire");
    readonly int hashReload = Animator.StringToHash("Reload");


    float nextFire = 0f;
    readonly float fireRate = 0.1f;
    readonly float damping = 10f;

    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip reloadSfx;

    readonly float reloadTime = 2f;
    readonly int maxBullet = 10;
    int currBullet = 10;
    bool isReload = false;

    WaitForSeconds wsReload;

    public GameObject bullet;
    public Transform firePos;

    public MeshRenderer muzzleFlash;


    void Start()
    {
        muzzleFlash.enabled = false;

        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);
    }
    

    void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time >= nextFire)
            {
                //���� �Լ� ȣ��
                Fire();
                //���� �߻� �ð� ���
                nextFire = Time.time + fireRate + Random.Range(0f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);

        }    
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audioSource.PlayOneShot(fireSfx, 1f);

        StartCoroutine(ShowMuzzleFlash());

        GameObject _bullet = Instantiate(bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 3f);

        currBullet--;
        isReload = (currBullet % maxBullet == 0);

        if(isReload)
        {
            //������ �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;

        animator.SetTrigger(hashReload);
        audioSource.PlayOneShot(reloadSfx, 1f);

        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;

        //360�� ����
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;

        //���� ũ�� ~ 2�� ũ����� ����
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1f, 2f);

        //�ؽ�ó offset ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

        muzzleFlash.enabled = false;
    }
}
