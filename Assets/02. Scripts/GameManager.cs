using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글턴 패턴을 위한 스태틱 변수
    public static GameManager Instance = null;

    [Header("Enemy 생성 관련 변수")]
    public Transform[] points;
    public GameObject enemy;
    public float creatTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    [Header("오브젝트 풀")]
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
        //씬 변경 등이 일어나도 파괴되지않음
        DontDestroyOnLoad(gameObject);

        //오브젝트 풀 생성함수 호출
        CreatePooling();
    }

    public GameObject GetBullet()
    {
        for(int i  = 0; i < pool.Count; i++)
        {
            //풀에 있는 총알들 중에서 비활성화 된 놈을 가져옴
            if (!pool[i].activeSelf) 
            {
                return pool[i];
            }
        } 
        return null;
    }

    public void CreatePooling()
    {
        //하이어라키에서 빈 오브젝트 생성하는것과 동일
        GameObject objectPools = new GameObject("ObjectPools");

        for(int i =0; i<maxPool; i++)
        {
            //동적생성하면서 위에서 지정한 ObjectPools의 자식으로 넣는것
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false); //미리 생성된 오브젝트 비활성화
            pool.Add(obj); //오브젝트 풀에 생성된 오브젝트 추가
        }
    }

    void Start()
    {
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length > 0 )
        {
            //적 생성 코루틴 함수 호출
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

        //Time Scale 값은 0 = 정지, 1 = 기본, 2 = 2배속 ...
        Time.timeScale = (isPaused) ? 0.25f : 1f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        //플레이어 오브젝트가 지니고 있는 모든 스크립트 불러오기
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach( var script in scripts )
        {
            //모든 스크립트 비활성화
            script.enabled = !isPaused;
        }

        var canvasGroup = GameObject.Find("Panel_Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }

}
