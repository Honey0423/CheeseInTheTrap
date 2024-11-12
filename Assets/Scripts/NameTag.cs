using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameTag : MonoBehaviourPunCallbacks
{
    private string playerName; // �г����� ������ ����

    private TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void Start()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("SetNickName", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        }
        
    }

    [PunRPC]
    void SetNickName(string name)
    {
        textMesh.text = name;
    }

    private void Update()
    {
        Vector3 lookDirection = Camera.main.transform.forward; // ī�޶��� �ٶ󺸴� ����
        lookDirection.y = 0; // Y�� ������ 0���� �����Ͽ� �������θ� ȸ��
        transform.rotation = Quaternion.LookRotation(lookDirection); // ���ο� �������� ȸ��
    }
}
