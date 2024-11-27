using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonController parentScript = GetComponentInParent<ThirdPersonController>();
        if (other.CompareTag("mouse"))
        {
            print("����");
            PhotonView otherPhotonView = other.GetComponent<PhotonView>();

            if (otherPhotonView != null && otherPhotonView != parentScript.photonView) // �ڱ� �ڽ��� ����
            {
                if (parentScript.photonView.IsMine && parentScript.isAttacking && parentScript.live)
                {
                    ThirdPersonController temp = other.GetComponent<ThirdPersonController>();
                    if (temp.live)
                    {
                        temp.���ݹ���();
                        //gmObject.Send(parentScript.NickName, temp.NickName);
                    }
                }
            }
        }
    }
}
