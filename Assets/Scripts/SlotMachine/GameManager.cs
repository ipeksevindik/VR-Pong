using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using System;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<SlotItem> Rows_1 = new List<SlotItem>();
    public List<SlotItem> Rows_2 = new List<SlotItem>();
    public List<SlotItem> Rows_3 = new List<SlotItem>();
    public List<SlotItem> Rows_4 = new List<SlotItem>();
    public List<SlotItem> Rows_5 = new List<SlotItem>();

    public List<SlotItem> SelectedItemsRow1 = new List<SlotItem>();
    public List<SlotItem> SelectedItemsRow2 = new List<SlotItem>();
    public List<SlotItem> SelectedItemsRow3 = new List<SlotItem>();
    public List<SlotItem> SelectedItemsRow4 = new List<SlotItem>();
    public List<SlotItem> SelectedItemsRow5 = new List<SlotItem>();

    public List<TrailRenderer> LineList = new List<TrailRenderer>();
         
    public int StopedRowCount = 0;

    public float Final_prize;

    public TextMeshProUGUI Prize_txt;
    public bool isStoped;
    private PhotonView photonView;

    public int rowspin1;
    public int rowspin2;
    public int rowspin3;
    public int rowspin4;
    public int rowspin5;

    public Action OnHandlePulled;
    public Action OnRowStoped;


    AudioManager audioManager;
    public TrailRenderer line;
    TrailRenderer spawnedline;
    public ParticleSystem confetti;
    public GameObject winEffect;
    public GameObject winText;

    private void OnEnable()
    {
        OnHandlePulled += CallHandle;
    }

    private void OnDisable()
    {
        OnHandlePulled -= CallHandle;
    }

    private void Start()
    {
        audioManager = GetComponentInChildren<AudioManager>();
        photonView = GetComponent<PhotonView>();
        isStoped = true;

    }

    [ContextMenu("callhandle")]
    public void CallHandle()
    {
        isStoped = false;

        audioManager.PlayPullHandle();

        SelectRandomItemRow1();
        SelectRandomItemRow2();
        SelectRandomItemRow3();
        SelectRandomItemRow4();
        SelectRandomItemRow5();



        photonView.RPC(nameof(RowLoop), RpcTarget.AllBuffered);
    }

    public void SelectRandomItemRow1()
    {
        int row1_1 = UnityEngine.Random.Range(0, Rows_1.Count);
        int row1_2 = UnityEngine.Random.Range(0, Rows_1.Count);
        int row1_3 = UnityEngine.Random.Range(0, Rows_1.Count);
        int randomspin = SelectRandomRowSpin();

        if(row1_1 == row1_2 || row1_1 == row1_3 || row1_2 == row1_3)
        {
            SelectRandomItemRow1();
        }
        else
            photonView.RPC(nameof(SetItemIndex1), RpcTarget.AllBuffered, row1_1, row1_2, row1_3, randomspin);

    }
    public void SelectRandomItemRow2()
    {
        int row2_1 = UnityEngine.Random.Range(0, Rows_2.Count);
        int row2_2 = UnityEngine.Random.Range(0, Rows_2.Count);
        int row2_3 = UnityEngine.Random.Range(0, Rows_2.Count);
        int randomspin = SelectRandomRowSpin();

        if (row2_1 == row2_2 || row2_1 == row2_3 || row2_2 == row2_3)
        {
            SelectRandomItemRow2();
        }
        else
            photonView.RPC(nameof(SetItemIndex2), RpcTarget.AllBuffered, row2_1, row2_2, row2_3, randomspin);
    }
    public void SelectRandomItemRow3()
    {
        int row3_1 = UnityEngine.Random.Range(0, Rows_3.Count);
        int row3_2 = UnityEngine.Random.Range(0, Rows_3.Count);
        int row3_3 = UnityEngine.Random.Range(0, Rows_3.Count);
        int randomspin = SelectRandomRowSpin();

        if (row3_1 == row3_2 || row3_1 == row3_3 || row3_2 == row3_3)
        {
            SelectRandomItemRow3();
        }
        else
            photonView.RPC(nameof(SetItemIndex3), RpcTarget.AllBuffered, row3_1, row3_2, row3_3, randomspin);
    }

    public void SelectRandomItemRow4()
    {
        int row4_1 = UnityEngine.Random.Range(0, Rows_4.Count);
        int row4_2 = UnityEngine.Random.Range(0, Rows_4.Count);
        int row4_3 = UnityEngine.Random.Range(0, Rows_4.Count);
        int randomspin = SelectRandomRowSpin();

        if (row4_1 == row4_2 || row4_1 == row4_3 || row4_2 == row4_3)
        {
            SelectRandomItemRow4();
        }
        else
            photonView.RPC(nameof(SetItemIndex4), RpcTarget.AllBuffered, row4_1, row4_2, row4_3, randomspin);
    }
    public void SelectRandomItemRow5()
    {
        int row5_1 = UnityEngine.Random.Range(0, Rows_5.Count);
        int row5_2 = UnityEngine.Random.Range(0, Rows_5.Count);
        int row5_3 = UnityEngine.Random.Range(0, Rows_5.Count);
        int randomspin = SelectRandomRowSpin();

        if (row5_1 == row5_2 || row5_1 == row5_3 || row5_2 == row5_3)
        {
            SelectRandomItemRow5();
        }
        else
            photonView.RPC(nameof(SetItemIndex5), RpcTarget.AllBuffered, row5_1, row5_2, row5_3, randomspin);
    }


    [PunRPC]
    public void SetItemIndex1(int index1, int index2, int index3, int randomspin)
    {
        ResetSelectedData1();

        rowspin1 = randomspin;
        SelectedItemsRow1.Add(Rows_1[index1]);
        SelectedItemsRow1.Add(Rows_1[index2]);
        SelectedItemsRow1.Add(Rows_1[index3]);

    }
    [PunRPC]
    public void SetItemIndex2(int index1, int index2, int index3, int randomspin)
    {
        ResetSelectedData2();

        rowspin2 = randomspin;
        SelectedItemsRow2.Add(Rows_2[index1]);
        SelectedItemsRow2.Add(Rows_2[index2]);
        SelectedItemsRow2.Add(Rows_2[index3]);

    }
    [PunRPC]
    public void SetItemIndex3(int index1, int index2, int index3, int randomspin)
    {
        ResetSelectedData3();

        rowspin3 = randomspin;
        SelectedItemsRow3.Add(Rows_3[index1]);
        SelectedItemsRow3.Add(Rows_3[index2]);
        SelectedItemsRow3.Add(Rows_3[index3]);
    }

    [PunRPC]
    public void SetItemIndex4(int index1, int index2, int index3, int randomspin)
    {
        ResetSelectedData4();

        rowspin4 = randomspin;
        SelectedItemsRow4.Add(Rows_4[index1]);
        SelectedItemsRow4.Add(Rows_4[index2]);
        SelectedItemsRow4.Add(Rows_4[index3]);
    }

    [PunRPC]
    public void SetItemIndex5(int index1, int index2, int index3, int randomspin)
    {
        ResetSelectedData5();

        rowspin5 = randomspin;
        SelectedItemsRow5.Add(Rows_5[index1]);
        SelectedItemsRow5.Add(Rows_5[index2]);
        SelectedItemsRow5.Add(Rows_5[index3]);
    }

    public void ResetSelectedData1()
    {
        Final_prize = 0;
        Prize_txt.text = " ";
        // ResetRow Hep SelectedItems.Clear() dan önce çalýþsýn
        ResetRow(SelectedItemsRow1);
        SelectedItemsRow1.Clear();
    }
    public void ResetSelectedData2()
    {
        Final_prize = 0;
        Prize_txt.text = " ";
        ResetRow(SelectedItemsRow2);
        SelectedItemsRow2.Clear();
    }
    public void ResetSelectedData3()
    {
        Final_prize = 0;
        Prize_txt.text = " ";
        ResetRow(SelectedItemsRow3);
        SelectedItemsRow3.Clear();
    }
    public void ResetSelectedData4()
    {
        Final_prize = 0;
        Prize_txt.text = " ";
        ResetRow(SelectedItemsRow4);
        SelectedItemsRow4.Clear();
    }
    public void ResetSelectedData5()
    {
        Final_prize = 0;
        Prize_txt.text = " ";
        ResetRow(SelectedItemsRow5);
        SelectedItemsRow5.Clear();
    }

    public void CheckRowsStoped()
    {
        StopedRowCount++;
        if (StopedRowCount >= 5)
        {
            audioManager.PlayRowStoped();
            Prize();
            StopedRowCount = 0;
        }
    }

    public void Prize()
    {
        CheckLinePrize(SelectedItemsRow1, SelectedItemsRow2, SelectedItemsRow3, SelectedItemsRow4, SelectedItemsRow5);
        isStoped = true;
     

    }

    public void CheckLinePrize(List<SlotItem> list1, List<SlotItem> list2, List<SlotItem> list3, List<SlotItem> list4, List<SlotItem> list5)
    {
        CheckLines(list1[0], list2[0], list3[0], list4[0], list5[0]);
        CheckLines(list1[1], list2[1], list3[1], list4[1], list5[1]);
        CheckLines(list1[2], list2[2], list3[2], list4[2], list5[2]);
        CheckLines(list1[2], list2[1], list3[0], list4[1], list5[2]);
        CheckLines(list1[0], list2[1], list3[2], list4[1], list5[0]);
        CheckLines(list1[2], list2[2], list3[1], list4[2], list5[2]);
        CheckLines(list1[0], list2[0], list3[1], list4[0], list5[0]);
        CheckLines(list1[1], list2[0], list3[0], list4[0], list5[1]);
        CheckLines(list1[1], list2[2], list3[2], list4[2], list5[1]);
        CheckLines(list1[1], list2[2], list3[1], list4[2], list5[1]);
        CheckLines(list1[1], list2[0], list3[1], list4[0], list5[1]);
        CheckLines(list1[2], list2[1], list3[2], list4[1], list5[2]); // 12
        CheckLines(list1[0], list2[1], list3[0], list4[1], list5[0]);
        CheckLines(list1[1], list2[1], list3[2], list4[1], list5[1]);
        CheckLines(list1[1], list2[1], list3[0], list4[1], list5[1]); // 15
        CheckLines(list1[2], list2[1], list3[1], list4[1], list5[2]); // 16
        CheckLines(list1[0], list2[1], list3[1], list4[1], list5[0]); // 17
        CheckLines(list1[2], list2[0], list3[2], list4[0], list5[2]); // 18
        CheckLines(list1[2], list2[0], list3[0], list4[0], list5[2]); // 19
        CheckLines(list1[0], list2[2], list3[2], list4[2], list5[0]); // 20
        CheckLines(list1[0], list2[2], list3[0], list4[2], list5[0]); // 21
        CheckLines(list1[2], list2[2], list3[0], list4[2], list5[2]); // 22
        CheckLines(list1[0], list2[0], list3[2], list4[0], list5[0]); // 23
        CheckLines(list1[2], list2[0], list3[1], list4[0], list5[2]);
        CheckLines(list1[0], list2[2], list3[1], list4[2], list5[0]);


        Prize_txt.text = "Total Win: " + Final_prize.ToString() + "$";
    }


    public void CheckLines(SlotItem item1, SlotItem item2, SlotItem item3, SlotItem item4, SlotItem item5)
    {

        if(item1.ItemId == item2.ItemId)
        {
            if (item2.ItemId == item3.ItemId)
            {
                item3.GetComponent<PrizeEffect>().SpawnTrail(item3);
                item2.GetComponent<PrizeEffect>().SpawnTrail(item2);
                item1.GetComponent<PrizeEffect>().SpawnTrail(item1);
                Final_prize += item1.ItemPrize + item2.ItemPrize + item3.ItemPrize;


                if (item3.ItemId == item4.ItemId)
                {
                    item4.GetComponent<PrizeEffect>().SpawnTrail(item4);
                    Final_prize += item4.ItemPrize;

                    if (item4.ItemId == item5.ItemId)
                    {
                        Final_prize += item5.ItemPrize;
                        Final_prize *= 10;
                        item5.GetComponent<PrizeEffect>().SpawnTrail(item5);
                        JackpotLine(item1, item2, item3, item4, item5);
                        

                        ConfettiPlay();
                        WinEffect();

                    }
                    else
                    {
                        JackpotLine(item1, item2, item3, item4, item5);


                    }

                }
                else
                {
                    JackpotLine(item1, item2, item3, item4, item5);


                }
            }
        }

    }

    [ContextMenu(nameof(Test))]
    public void Test()
    {
        ConfettiPlay();
        WinEffect();
    }


    public void JackpotLine(SlotItem item1, SlotItem item2, SlotItem item3, SlotItem item4, SlotItem item5)
    {
        spawnedline = Instantiate(line);
        LineList.Add(spawnedline);

        spawnedline.transform.localPosition = item1.patrolPoints[5].transform.position;
        var sequence = DOTween.Sequence();

        sequence.Append(spawnedline.transform.DOLocalMove(item2.patrolPoints[5].transform.position, 0.4f));
        sequence.Append(spawnedline.transform.DOLocalMove(item3.patrolPoints[5].transform.position, 0.4f));
        sequence.Append(spawnedline.transform.DOLocalMove(item4.patrolPoints[5].transform.position, 0.4f));
        sequence.Append(spawnedline.transform.DOLocalMove(item5.patrolPoints[5].transform.position, 0.4f));

        sequence.SetLoops(-1, LoopType.Yoyo);

    }

    public void DestroyJackpotLine()
    {
        if (LineList.Count > 0)
        {
            for (int i = 0; i < LineList.Count; i++)
            {
                Destroy(LineList[i].gameObject);
            }
            LineList.Clear();
        }
    }


    [PunRPC]
    public void RowLoop()
    {
        audioManager.PlayRowMove();
        MoveNext(Rows_1, SelectedItemsRow1[0], SelectedItemsRow1[1], SelectedItemsRow1[2], rowspin1);
        MoveNext(Rows_2, SelectedItemsRow2[0], SelectedItemsRow2[1], SelectedItemsRow2[2], rowspin2);
        MoveNext(Rows_3, SelectedItemsRow3[0], SelectedItemsRow3[1], SelectedItemsRow3[2], rowspin3);
        MoveNext(Rows_4, SelectedItemsRow4[0], SelectedItemsRow4[1], SelectedItemsRow4[2], rowspin4);
        MoveNext(Rows_5, SelectedItemsRow5[0], SelectedItemsRow5[1], SelectedItemsRow5[2], rowspin5);
        
    }

    [PunRPC]
    public int SelectRandomRowSpin()
    {
        int index = UnityEngine.Random.Range(3, 5);
        return index;
    }

    public void ResetRow(List<SlotItem> list)
    {
        DestroyJackpotLine();
       

        foreach (var item in list)
        {
            item.transform.DOLocalMoveY(25, 0);
            item.GetComponent<PrizeEffect>().DestroySpawnTrail();


        }
    }

    public void MoveNext(List<SlotItem> list, SlotItem index1, SlotItem index2, SlotItem index3, int rowspin)
    {
        var sequence = DOTween.Sequence();
        float time= 0;
        foreach (SlotItem item in list)
        {
            sequence.Insert(time, item.transform.DOLocalMoveY(-30, 0.3f).SetEase(Ease.Linear));
            sequence.Append(item.transform.DOLocalMoveY(24, 0));
            time += 0.190f;
        }
       
        sequence.SetLoops(rowspin, LoopType.Restart).OnComplete(
            () => index1.transform.DOLocalMoveY(-17.5f, 0.2f).OnComplete(
                () => index2.transform.DOLocalMoveY(-4f, 0.2f).OnComplete(
                    () => index3.transform.DOLocalMoveY(11f, 0.2f).OnComplete(
                        () => CheckRowsStoped()))));

    }

    async Task ConfettiPlay()
    {
        confetti.Play();

        await Task.Delay(3000);

        confetti.Stop();

    }
        
    public void WinEffect()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(winEffect.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 1f));
        sequence.Insert(2, winEffect.transform.DOScale(new Vector3(0, 0, 0), 1f));

        sequence.Insert(0, winText.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1f));
        sequence.Insert(2f, winText.transform.DOScale(new Vector3(0, 0, 0), 1f));

    }

}
