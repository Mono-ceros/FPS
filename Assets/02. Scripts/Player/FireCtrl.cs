using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UI ���� �̺�Ʈ ����
using UnityEngine.EventSystems;

[Serializable]
public struct PlayerSFX
{
    public AudioClip[] fire;
    public AudioClip[] reload;
} 
//����ü�� Ŭ������ ������ ����ϳ�
//Ŭ������ = ��, ����ü = ���� ������ �Ҵ�, �޸��Ҵ��� ����
//������ ������ �ܰ迡�� �̸� �ö�
//���ҽ��� ��� ���� ��(������) �ܰ迡�� �̸� ����
//������ �� ����

public class FireCtrl : MonoBehaviour
{
    //enum�� ������ ����
    //���� ���������� �迭ó�� ���� ������ Ÿ���� ��������
    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN
    }
    //�������� �̿��� ����, ���� ���Ⱑ �������� ����
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bulletPrefab; //�Ѿ� ������ 
    public Transform firePos; //�Ѿ� �߻���ġ 

    public ParticleSystem cartridge; //ź�� ��ƼŬ
    private ParticleSystem muzzleFlash; //�ѱ� ȭ��

    AudioSource audioSource;
    public PlayerSFX playerSFX;

    Shake shake;

    public Image magazineImg;
    public Text magazineText;

    public int maxBullet = 10;
    public int reamainingBullet = 10;

    public float reloadTime = 2f;
    bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;

    int enemyLayer; //������ ���̾�
    bool isFire = false;
    float nextFire;
    public float fireRate = 0.1f;

    int obstacleLayer; //��ֹ� ���� ���̾�
    int layerMask; //���̾� ���� ���� ����

    public void OnChangeWeapon()
    {
        currWeapon++;
        currWeapon = (WeaponType)((int)currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }

    void Start()
    {
        //firePos ������Ʈ�� �ڽ����� �ִ� ������ GetComponent��
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();

        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

        //��ϵ� ���̾�� �߿��� �ش��ϴ� �̸��� ���̾� ������ �����ͼ� ����
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        //enemyLayer �� obstacleLayer 2������ ����
        // | <<< ��Ʈ ������ ���
        layerMask = 1 << enemyLayer | 1 << obstacleLayer;
    }

    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);

        //UI������ Ŭ��/��ġ �߻��ϸ� true �ƴϸ� false
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        //(���� �߻���ġ, ����, �浹����, ��Ÿ�, ���� ���̾�)
        if(Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
            isFire = hit.collider.CompareTag("ENEMY");
        else
            isFire = false;

        if(!isReloading && isFire)
        {
            if(Time.time > nextFire)
            {
                reamainingBullet--;
                Fire();

                if (reamainingBullet == 0)
                    StartCoroutine(Reloading());

                nextFire = Time.time + fireRate;
            }
        }

        //���� ���� ���
        //0�� ��Ŭ�� 1�� ��Ŭ�� 
        if(!isReloading && Input.GetMouseButtonDown(0))
        {
            reamainingBullet--;

            //�Ѿ� �߻� �޼ҵ� ȣ��
            Fire();

            if(reamainingBullet == 0)
            {
                //������ �ڷ�ƾ �Լ� ȣ��
                StartCoroutine(Reloading());
            }
        }
    }

    void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        //�Ѿ� ������ ���� ���� 
        //Instantiate(bulletPrefab,firePos.position,firePos.rotation);
        var _bullet = GameManager.Instance.GetBullet();
        if(_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartridge.Play(); //ź�� ��ƼŬ ���
        muzzleFlash.Play(); //�ѱ� ȭ�� ���
        FireSfx(); //�ѼҸ� �޼ҵ� ȣ��

        magazineImg.fillAmount = reamainingBullet / (float)maxBullet;
        UpdateBulletText();
    }

    void FireSfx()
    {
        //enumŸ���� ���ؼ� ���� ������ ���� ������
        //������ ������ ����ó�� ���ǳ� �տ� int�� ���
        var sfx = playerSFX.fire[(int)currWeapon];
        //PlayOneShot(��� ����, ����� ����)
        audioSource.PlayOneShot(sfx, 1f);
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        audioSource.PlayOneShot(playerSFX.reload[(int)currWeapon], 1f);

        yield return new WaitForSeconds(playerSFX.reload[(int)currWeapon].length + 0.3f);

        isReloading = false;
        magazineImg.fillAmount = 1f;
        reamainingBullet = maxBullet;

        //�ؽ�Ʈ ǥ�� �Լ� ȣ��
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/10", reamainingBullet);
    }
}
