using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;


public class PrizeEffect : MonoBehaviour
{
    
    //public Transform[] patrolPoints;
    public TrailRenderer trail;

    TrailRenderer spawnedtrail;

    private void Start()
    {
        
       
    }

    public void SpawnTrail(SlotItem item)
    {
        spawnedtrail = Instantiate(trail, item.transform.parent);
        Debug.Log(item.gameObject.name, item);
        Debug.Log(item.patrolPoints.Length);
        spawnedtrail.transform.position = item.patrolPoints[0].transform.position;
        Debug.Log(item.patrolPoints[0].gameObject.name, item.patrolPoints[0].gameObject);
        var sequence = DOTween.Sequence();

        sequence.Append(spawnedtrail.transform.DOMove(item.patrolPoints[1].transform.position, 0.4f));
        sequence.Append(spawnedtrail.transform.DOMove(item.patrolPoints[2].transform.position, 0.4f));
        sequence.Append(spawnedtrail.transform.DOMove(item.patrolPoints[3].transform.position, 0.4f));
        sequence.Append(spawnedtrail.transform.DOMove(item.patrolPoints[4].transform.position, 0.4f));

        sequence.SetLoops(-1, LoopType.Restart);

    }

    public void DestroySpawnTrail()
    {
        if(spawnedtrail!=null)
            Destroy(spawnedtrail.gameObject);
    }

}
