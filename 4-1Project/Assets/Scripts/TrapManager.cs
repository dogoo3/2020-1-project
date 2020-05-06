using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour
{
    public static TrapManager instance;

    public List<FirePillar> firepillars;
    public List<PatrolFireball> patrolFireballs;
    public GameObject bombAreaA;
    public GameObject bombAreaB;

    private float _firePillartime; // 프로펠러 불기둥 시간재기
    private float _bombAreatime; // 구역폭발 트랩 시간
    
    private void Awake()
    {
        instance = this;
    }

    public void ResolveTrapData()
    {
        // 프로펠러 불기둥 시간재기

        // 불공 왔다갔다 트랩 시간재기

        // 구역폭발 트랩 시간재기
        _bombAreatime += 0.2f;
        if (_bombAreatime >= 8.0f)
            _bombAreatime = 0;
     }

    private void Update()
    {
        ResolveFirePliiar(); // 프로펠러 불기둥
        ResolvePatrolFireball(); // 불공 왔다갔다
        ResolveBombArea(); // 구역별 폭발
    }

    private void ResolveFirePliiar()
    {

    }

    private void ResolvePatrolFireball()
    {

    }

    private void ResolveBombArea()
    {
        if (_bombAreatime >= 2.0f && _bombAreatime <= 4.0f ||
            _bombAreatime >= 6.0f && _bombAreatime <= 8.0f) // 위치 이동 시간을 줌
        {
            bombAreaA.gameObject.SetActive(false);
            bombAreaB.gameObject.SetActive(false);
        }
        else
        {
            if (_bombAreatime < 2.0f) // 폭파구역 둘 중 하나는 실행
            {
                // A구역 폭파 ON
                bombAreaA.gameObject.SetActive(true);
                bombAreaB.gameObject.SetActive(false);
            }
            else
            {
                // B구역 폭파 ON
                bombAreaA.gameObject.SetActive(false);
                bombAreaB.gameObject.SetActive(true);
            }
        }
    }
}
