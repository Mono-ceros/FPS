using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UI 관련 이벤트 제어
using UnityEngine.EventSystems;

[Serializable]
public struct PlayerSFX
{
    public AudioClip[] fire;
    public AudioClip[] reload;
} 
//구조체와 클래스의 사용법은 비슷하나
//클래스는 = 힙, 구조체 = 스택 영역에 할당, 메모리할당의 차이
//스택은 컴파일 단계에서 미리 올라감
//리소스의 경우 실행 전(컴파일) 단계에서 미리 고지
//성능이 더 좋음

public class FireCtrl : MonoBehaviour
{
    //enum은 열거형 변수
    //단일 변수이지만 배열처럼 여러 종류의 타입을 설정가능
    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN
    }
    //열거형을 이용한 변수, 현재 무기가 무엇인지 저장
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bulletPrefab; //총알 프리팹 
    public Transform firePos; //총알 발사위치 

    public ParticleSystem cartridge; //탄피 파티클
    private ParticleSystem muzzleFlash; //총구 화염

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

    int enemyLayer; //적감지 레이어
    bool isFire = false;
    float nextFire;
    public float fireRate = 0.1f;

    int obstacleLayer; //장애물 감지 레이어
    int layerMask; //레이어 병합 정보 저장

    public void OnChangeWeapon()
    {
        currWeapon++;
        currWeapon = (WeaponType)((int)currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }

    void Start()
    {
        //firePos 오브젝트의 자식으로 있는 머즐을 GetComponent함
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();

        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

        //등록된 레이어들 중에서 해당하는 이름의 레이어 정보를 가져와서 저장
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        //enemyLayer 와 obstacleLayer 2가지를 병합
        // | <<< 비트 연산자 사용
        layerMask = 1 << enemyLayer | 1 << obstacleLayer;
    }

    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);

        //UI위에서 클릭/터치 발생하면 true 아니면 false
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        //(레이 발사위치, 방향, 충돌정보, 사거리, 적용 레이아)
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

        //수동 공격 방식
        //0이 좌클릭 1이 우클릭 
        if(!isReloading && Input.GetMouseButtonDown(0))
        {
            reamainingBullet--;

            //총알 발사 메소드 호출
            Fire();

            if(reamainingBullet == 0)
            {
                //재장전 코루틴 함수 호출
                StartCoroutine(Reloading());
            }
        }
    }

    void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        //총알 프리팹 동적 생성 
        //Instantiate(bulletPrefab,firePos.position,firePos.rotation);
        var _bullet = GameManager.Instance.GetBullet();
        if(_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartridge.Play(); //탄피 파티클 재생
        muzzleFlash.Play(); //총구 화염 재생
        FireSfx(); //총소리 메소드 호출

        magazineImg.fillAmount = reamainingBullet / (float)maxBullet;
        UpdateBulletText();
    }

    void FireSfx()
    {
        //enum타입을 통해서 현재 무기의 값을 가져옴
        //열거형 변수는 정수처럼 사용되나 앞에 int를 명시
        var sfx = playerSFX.fire[(int)currWeapon];
        //PlayOneShot(재생 음원, 재생시 볼륨)
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

        //텍스트 표시 함수 호출
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/10", reamainingBullet);
    }
}
