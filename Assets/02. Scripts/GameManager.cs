using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ���� ����ƽ ����
    public static GameManager Instance = null;

    [Header("Enemy ���� ���� ����")]
    public Transform[] points;
    public GameObject enemy;
    public float creatTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    [Header("������Ʈ Ǯ")]
    public GameObject bulletPrefab;
    public int maxPool = 10;
    public List<GameObject> pool = new List<GameObject>();

    public CanvasGroup invetoryCG;

    public void OnInventoryOpen(bool isOpened)
    {
        invetoryCG.alpha = (isOpened ? 1 : 0);
        invetoryCG.interactable = isOpened;
        invetoryCG.blocksRaycasts = isOpened;
    }

    private void Awake()
    {
        if(Instance == null )
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //�� ���� ���� �Ͼ�� �ı���������
        DontDestroyOnLoad(gameObject);

        //������Ʈ Ǯ �����Լ� ȣ��
        CreatePooling();
    }

    public GameObject GetBullet()
    {
        for(int i  = 0; i < pool.Count; i++)
        {
            //Ǯ�� �ִ� �Ѿ˵� �߿��� ��Ȱ��ȭ �� ���� ������
            if (!pool[i].activeSelf) 
            {
                return pool[i];
            }
        } 
        return null;
    }

    public void CreatePooling()
    {
        //���̾��Ű���� �� ������Ʈ �����ϴ°Ͱ� ����
        GameObject objectPools = new GameObject("ObjectPools");

        for(int i =0; i<maxPool; i++)
        {
            //���������ϸ鼭 ������ ������ ObjectPools�� �ڽ����� �ִ°�
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false); //�̸� ������ ������Ʈ ��Ȱ��ȭ
            pool.Add(obj); //������Ʈ Ǯ�� ������ ������Ʈ �߰�
        }
    }

    void Start()
    {
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length > 0 )
        {
            //�� ���� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("ENEMY").Length;

            if(enemyCount < maxEnemy )
            {
                yield return new WaitForSeconds( creatTime );

                int idx = Random.Range( 1, points.Length );
                Instantiate( enemy, points[idx].position, points[idx].rotation );
            }
            else
                yield return null;
        }
    }

    bool isPaused;

    public void OnPauseClick()
    {
        isPaused = !isPaused;

        //Time Scale ���� 0 = ����, 1 = �⺻, 2 = 2��� ...
        Time.timeScale = (isPaused) ? 0.25f : 1f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        //�÷��̾� ������Ʈ�� ���ϰ� �ִ� ��� ��ũ��Ʈ �ҷ�����
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach( var script in scripts )
        {
            //��� ��ũ��Ʈ ��Ȱ��ȭ
            script.enabled = !isPaused;
        }

        var canvasGroup = GameObject.Find("Panel_Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }

}
