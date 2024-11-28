using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StarterAssets;

public class InGameNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] Player;
    public Text Time;
    private GameObject Mycharactor;
    public GameObject ���ٴ�;
    public Text ġ���Text;
    private int ġ��� = 4;
    private int ���� = 2;
    private int count = 0;
    private List<Vector3> spawnPositions = new List<Vector3> {
        new Vector3(-44.56f, 6.227f, -18.27f),
        new Vector3(-57.4f, 6.227f, -18.27f),
        new Vector3(-30f, 6.227f, -18.27f),
        new Vector3(-44.56f, 6.227f, -10) };
    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        /* if (networkManager.Mycharacter == 0)
         {
             Mycharactor = PhotonNetwork.Instantiate("��1", new Vector3(-0, 0, -0), Quaternion.identity);
         }

         else if (networkManager.Mycharacter == 1)
         {
             Mycharactor = PhotonNetwork.Instantiate("����", new Vector3(-0, 0, -0), Quaternion.identity);
         }*/


        if (PhotonNetwork.IsMasterClient)
        {
            if (networkManager.Mycharacter2 == 0)
            {
                Mycharactor = PhotonNetwork.Instantiate("Cat1", new Vector3(-45, 87, 1), Quaternion.identity);//���Ŀ� ������� NickName�� ������ �ʰ� �����Ѵ�...
            }
            else if (networkManager.Mycharacter2 == 1)
            {
                Mycharactor = PhotonNetwork.Instantiate("Tom1", new Vector3(-45, 87, 1), Quaternion.identity);
            }

        }
        else
        {
            if (networkManager.Mycharacter == 0)
            {
                Mycharactor = PhotonNetwork.Instantiate("��1", new Vector3(-40, 87, 1), Quaternion.identity);
            }

            else if (networkManager.Mycharacter == 1)
            {
                Mycharactor = PhotonNetwork.Instantiate("����1", new Vector3(-40, 87, 1), Quaternion.identity);
            }
        }


        photonView.RPC("LoadComplete", RpcTarget.MasterClient);
        ġ���Text.text = "ġ���:" + ġ���.ToString();
    }
    void Start()
    {

    }

    void Update()
    {

    }


    public void ġ���()
    {
        photonView.RPC("ġ���RPC", RpcTarget.All);
    }
    [PunRPC]
    void ġ���RPC()
    {
        ġ���--;
        ġ���Text.text = "ġ���:" + ġ���.ToString();
        if (ġ��� == 0)
        {
            //���� �����ִ� ����;
        }
    }

    [PunRPC]
    void LoadComplete()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            count++;
            if (PhotonNetwork.PlayerList.Length == count)
            {
                int ����� = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);
                for (int i = 0; i < spawnPositions.Count; i++)
                {
                    Vector3 temp = spawnPositions[i];
                    int randomIndex = UnityEngine.Random.Range(i, spawnPositions.Count);//��ġ�� �̷��� UnityEngine.Random���� �������ָ� ��.
                    spawnPositions[i] = spawnPositions[randomIndex];
                    spawnPositions[randomIndex] = temp;
                }

                // �� �÷��̾�� ������ ���� ��ġ �Ҵ�
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if(i>0)
                    {
                        photonView.RPC("PlayerSet", RpcTarget.All, i);
                    }
                    photonView.RPC("GameStart", PhotonNetwork.PlayerList[i], spawnPositions[i]);
                }
            }
        }
    }

    [PunRPC]
    void GameStart(Vector3 spawnPositioni)
    {
        Debug.Log($"spawnPositioni - X: {spawnPositioni.x}, Y: {spawnPositioni.y}, Z: {spawnPositioni.z}");

        StartCoroutine(CountStart(spawnPositioni));
        
    }

    [PunRPC]
    void PlayerSet(int num)
    {
        Player[num-1].SetActive(true);
        Player[num-1].transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[num].NickName;
        Player[num - 1].transform.GetChild(2).GetComponent<Text>().text = "X"+����.ToString();
    }

    public void ����Update(string NickName,int count)
    {
        photonView.RPC("����UpdateRPC", RpcTarget.All,NickName,count);
    }
    [PunRPC]
    void ����UpdateRPC(string NickName, int count)
    {
        for(int i=1; i<PhotonNetwork.PlayerList.Length; i++)
        {
            if (Player[i-1].transform.GetChild(0).GetComponent<Text>().text==NickName)
            {
                Player[i-1].transform.GetChild(2).GetComponent<Text>().text= "X" + count.ToString();
                break;
            }
        }
    }

    IEnumerator CountStart(Vector3 spawnPositioni)
    {
        for(int i=10; i>=0; i--)
        {
            Time.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        Time.gameObject.SetActive(false);
        //���ٴ�.SetActive(false);
        print("�����̵���");
        ThirdPersonController temp = Mycharactor.GetComponent<ThirdPersonController>();
        temp.�����̵�(spawnPositioni);
        
        
    }
}
