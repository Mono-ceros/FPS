using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject expEffect; //폭발 효과 프리팹

    int hitCount = 0; //총알 맞은 갯수
    Rigidbody rb;

    public Mesh[] meshes; //찌그러진 모양 메쉬
    MeshFilter meshFilter; //메쉬 제어하는 메쉬필터 컴포넌트

    public Texture[] textures; //드럼통 이미지
    MeshRenderer meshRenderer;

    public float expRadius = 10f; //폭발 반경

    AudioSource audioSource;
     
    public AudioClip expSfx;

    Shake shake;
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.mainTexture = textures[Random.Range(0,textures.Length)];

        audioSource = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            hitCount++; //맞은 횟수 증가

            if(hitCount == 3)
            {
                //드럼통 폭발 메소드 호출
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        //Instantiate로 동적생성된 오브젝트를
        //아래와 같이 객체 변수로 제어할 수 있음
        GameObject effect = Instantiate(expEffect,
                                        transform.position,
                                        Quaternion.identity);
        Destroy(effect, 2f);
        //rb.mass = 1f; //가볍게만들기
        //rb.AddForce(Vector3.up * 500f); //위로 힘주기
        //변경된 폭발 메소드 호출할거라 주석처리함
        IndirectDamage(transform.position);


        meshFilter.sharedMesh = meshes[Random.Range(0, meshes.Length)];

        audioSource.PlayOneShot(expSfx, 1f); 
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    void IndirectDamage(Vector3 pos)
    {
        //오버랩스페어 메소드는 가상의 콜라이더를 생성
        //(위치, 반경, 검출 레이어)
        //해당위치에서 반경만큼 구형 모양의 콜라이더 생성
        //검출 레이어에 해당하는 오브젝트만 추출
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            //AddExplosionForce(횡 폭발력, 위치, 반경, 종 폭발력)
            _rb.AddExplosionForce(600f, pos, expRadius, 500f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
