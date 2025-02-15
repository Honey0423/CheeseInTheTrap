using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chees : MonoBehaviourPun, IPunObservable
{
    private InGameNetworkManager inGameNetworkManager;
    public GameObject 발전기TEXT;
    public Slider 발전기;
    float 게이지=0;
    bool flag=true;

    bool 개인flag = false;
    // Start is called before the first frame update
    void Start()
    {
        inGameNetworkManager=FindObjectOfType<InGameNetworkManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("mouse")&&flag)
        {
            PhotonView temp = other.gameObject.GetComponent<PhotonView>();
            if(temp != null && temp.IsMine)
            {
                /*PhotonView photonView = gameObject.GetComponent<PhotonView>();
                int viewID = photonView.ViewID;
                photonView.RPC("치즈삭제", RpcTarget.MasterClient, viewID);*/
                발전기TEXT.SetActive(true);
                발전기.gameObject.SetActive(true);
                발전기.value = 게이지;
                개인flag = true;
            }
            
        }
        print("더ㅚㅁ");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("mouse") && flag)
        {
            PhotonView temp = other.gameObject.GetComponent<PhotonView>();
            if (temp!=null&&temp.IsMine)
            {
                발전기.value = 게이지;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("mouse") && flag)
        {
            PhotonView temp = other.gameObject.GetComponent<PhotonView>();
            if (temp != null && temp.IsMine)
            {
                발전기TEXT.SetActive(false);
                발전기.gameObject.SetActive(false);
                개인flag = false;
            }
                
        }
        print("더ㅚㅁ");
    }

    [PunRPC]
    void 치즈삭제(int viewID)
    {
        if(flag)
        {
            inGameNetworkManager.치즈감소();
            PhotonView view = PhotonView.Find(viewID);
            PhotonNetwork.Destroy(view.gameObject);
            flag = false;
        }
        
    }

    public void 게이지증가()
    {
        photonView.RPC("게이지증가RPC", RpcTarget.MasterClient);
        발전기.value = 게이지;
        if(게이지>=발전기.maxValue)
        {
            발전기TEXT.SetActive(false);
            발전기.gameObject.SetActive(false);
            PhotonView photonView = gameObject.GetComponent<PhotonView>();
            int viewID = photonView.ViewID;
            photonView.RPC("치즈삭제", RpcTarget.MasterClient, viewID);
        }
    }
    [PunRPC]
    public void 게이지증가RPC()
    {
        게이지 += Time.deltaTime * 9;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(게이지); 
        }
        else
        {
            게이지 = (float)stream.ReceiveNext();
        }
    }

    private void OnDestroy()
    {
        if(개인flag)
        {
            발전기TEXT.SetActive(false);
            발전기.gameObject.SetActive(false);
        }
    }
}
