using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject expEffect; //���� ȿ�� ������

    int hitCount = 0; //�Ѿ� ���� ����
    Rigidbody rb;

    public Mesh[] meshes; //��׷��� ��� �޽�
    MeshFilter meshFilter; //�޽� �����ϴ� �޽����� ������Ʈ

    public Texture[] textures; //�巳�� �̹���
    MeshRenderer meshRenderer;

    public float expRadius = 10f; //���� �ݰ�

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
            hitCount++; //���� Ƚ�� ����

            if(hitCount == 3)
            {
                //�巳�� ���� �޼ҵ� ȣ��
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        //Instantiate�� ���������� ������Ʈ��
        //�Ʒ��� ���� ��ü ������ ������ �� ����
        GameObject effect = Instantiate(expEffect,
                                        transform.position,
                                        Quaternion.identity);
        Destroy(effect, 2f);
        //rb.mass = 1f; //�����Ը����
        //rb.AddForce(Vector3.up * 500f); //���� ���ֱ�
        //����� ���� �޼ҵ� ȣ���ҰŶ� �ּ�ó����
        IndirectDamage(transform.position);


        meshFilter.sharedMesh = meshes[Random.Range(0, meshes.Length)];

        audioSource.PlayOneShot(expSfx, 1f); 
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    void IndirectDamage(Vector3 pos)
    {
        //����������� �޼ҵ�� ������ �ݶ��̴��� ����
        //(��ġ, �ݰ�, ���� ���̾�)
        //�ش���ġ���� �ݰ游ŭ ���� ����� �ݶ��̴� ����
        //���� ���̾ �ش��ϴ� ������Ʈ�� ����
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            //AddExplosionForce(Ⱦ ���߷�, ��ġ, �ݰ�, �� ���߷�)
            _rb.AddExplosionForce(600f, pos, expRadius, 500f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
