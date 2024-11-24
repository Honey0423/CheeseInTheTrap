using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chees : MonoBehaviourPun
{
    private InGameNetworkManager inGameNetworkManager;
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
        if (other.CompareTag("mouse"))
        {
            PhotonView photonView = gameObject.GetComponent<PhotonView>();
            int viewID = photonView.ViewID;
            photonView.RPC("ġ�����", RpcTarget.MasterClient, viewID);
        }
        print("���ʤ�");
    }

    [PunRPC]
    void ġ�����(int viewID)
    {
        inGameNetworkManager.ġ���();
        PhotonView view = PhotonView.Find(viewID);
        PhotonNetwork.Destroy(view.gameObject);
    }
}
