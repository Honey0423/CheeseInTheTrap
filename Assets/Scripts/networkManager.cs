using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Unity.VisualScripting;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Cockpit;

public class networkManager : MonoBehaviourPunCallbacks
{
    [Header("Mycharacter")]
    public static int Mycharacter;
    public static int Mycharacter2;
    public static int MySkill;
    public GameObject 쥐;
    public GameObject 고양이;

    public Text NickName;
    public GameObject LoginPanel;
    public GameObject LobbyPanel;
    public GameObject RoomPanel;
    public static GameObject RoomPanel2;

    public GameObject 로그인중;
    public Text 로그인중text;
    private Coroutine loadingTextCoroutine;

    [Header("LobbyPanel")]
    public static GameObject Lobby캔버스;
    public InputField RoomInput;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public GameObject HomePanel;
    public GameObject PlayPanel;
    public GameObject 내정보Panel;
    public GameObject 상점Panel;
    public GameObject[] GameChar1;
    public GameObject[] GameChar1고양이;
    public GameObject[] charSlotPanel;
    public GameObject[] charSlot상점Panel;
    public GameObject 구매확인Panel;
    public Button[] CharactorBTN;
    public Button[] SkillBtn;
    public Image[] skillImage;
    public GameObject 캐릭터Panel;
    public GameObject 스킬Panel;
    public GameObject 상점캐릭터Panel;
    public GameObject 상점스킬Panel;
    public GameObject 설정Panel;
    public GameObject 이미보유Panel;
    public GameObject Money;

    public GameObject[] mouseCheck;
    public GameObject[] catCheck;
    public GameObject[] skillCheck;

    [Header("RoomPanel")]
    public Text PlayerName;
    public Text RoomName;

    /* 수정 부분 */
    public Image countImage;
    public Image startImage;
    public Sprite[] numberSprites;

    public Button[] playerBtn;
    public GameObject[] playerchar;
    public GameObject character;
    public Text[] ChatText;
    public InputField ChatInput;
    public GameObject[] GameChar2;
    public GameObject[] GameChar2고양이;
    public GameObject[] PlayerChar;
    public GameObject MakeRoomPanel;
    public Button ReadyBTN;
    public Text ReadyText;
    public Button ExitBTN;
    


    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    bool[] playerReady = new bool[8];
    int[] playercharint = new int[8];
    private int slotNum=0;
    // Start is called before the first frame update
    void Start()
    {
        Lobby캔버스= GameObject.Find("LobbyCanvas");
        RoomPanel2 = RoomPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InputField temp;
    public void Connect(string nickName)
    {
        로그인중.SetActive(true);
        loadingTextCoroutine = StartCoroutine(UpdateLoadingText(로그인중text, "로그인"));
        PhotonNetwork.ConnectUsingSettings();//시작 버튼을 누르면 연결
        PhotonNetwork.LocalPlayer.NickName = nickName;
        //PhotonNetwork.LocalPlayer.NickName = temp.text;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        print("서버접속완료");
    }
    private bool isCreatingRoom = false;
    public void CreateRoom()
    {
        if(isCreatingRoom)
        {
            print("이미 생성중");
        }
        else
        {
            isCreatingRoom = true;
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "isGameStarted", false } },
                CustomRoomPropertiesForLobby = new string[] { "isGameStarted" }
            };
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + UnityEngine.Random.Range(0, 100) : RoomInput.text, roomOptions);
        }
       
    }

    public override void OnCreatedRoom()
    {
        isCreatingRoom = false;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnJoinedRoom()
    {
        print("방 접속 완료");
        RoomPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        PlayerName.text = PhotonNetwork.LocalPlayer.NickName;
        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            SetMynumRPC(PhotonNetwork.NickName,Mycharacter2+2);//마스터클라이언트는 고양이

            UpdateGameState(playerReady, playercharint,0);
            ReadyBTN.GetComponent<Image>().color = new Color(0f / 255f, 191f / 255f, 255f / 255f);
            ReadyText.text = "START";
        }
        else
        {
            photonView.RPC("SetMynumRPC", RpcTarget.All, PhotonNetwork.NickName, Mycharacter);//들어오면 자신이 선택한 캐릭터 정보를 뿌린다.
            ReadyBTN.GetComponent<Image>().color = new Color(87f / 255f, 255f / 255f, 0f / 255f);
            ReadyText.text = "READY";
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public void LeftRoom()
    {
        photonView.RPC("ReSetMynumRPC", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.LeaveRoom();
    }

    bool roomToLobby = false;
    public override void OnLeftRoom()
    {
        roomToLobby = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>", newPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateGameState", RpcTarget.AllBuffered, playerReady, playercharint,0);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>", otherPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateGameState", RpcTarget.AllBuffered, playerReady, playercharint,0);
        }
    }

    public override void OnJoinedLobby()//로비에 접속하면??
    {
        if(roomToLobby==false)
        {
            로그인중.SetActive(false);
            LoginPanel.SetActive(false);
            LobbyPanel.SetActive(true);
            StopCoroutine(loadingTextCoroutine);//코루틴 중단
            NickName.text = PhotonNetwork.LocalPlayer.NickName + "님";//닉네임 text를 로컬 플레이어 닉네임으로 설정
            print(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            roomToLobby = false;
            for (int i = 0; i < 4; i++)//방을 떠나면서 방 정보 초기화
            {
                playerBtn[i].interactable = false;
                PlayerChar[i].transform.GetChild(playercharint[i]).gameObject.SetActive(false);
                playerBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";
                playerBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);  // RGB: 255, 255, 255
            }
            for (int i = 0; i < 9; i++)
            {
                ChatText[i].text = " ";
            }
            LobbyPanel.SetActive(true);
            MakeRoomPanel.SetActive(false);
            ClickPlayBTN();
            RoomPanel.SetActive(false);
        }
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HandleNewMasterClient();
        }

        void HandleNewMasterClient()
        {
            SetMynumRPC(PhotonNetwork.NickName, Mycharacter2 + 2);
            ReadyBTN.GetComponent<Image>().color = new Color(0f / 255f, 191f / 255f, 255f / 255f);
            ReadyText.text = "START";
        }
    }

    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        {
            var roomInfo = myList[multiple + num];
            bool isGameStarted = roomInfo.CustomProperties.ContainsKey("isGameStarted") && (bool)roomInfo.CustomProperties["isGameStarted"];
            if(!isGameStarted)
            {
                PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            }
            else
            {
                print("게임이 이미 시작 됨");
            }
            
        }
       
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            if (multiple + i < myList.Count)
            {
                var roomInfo = myList[multiple + i];
                bool isGameStarted = roomInfo.CustomProperties.ContainsKey("isGameStarted") && (bool)roomInfo.CustomProperties["isGameStarted"];

                CellBtn[i].interactable = !isGameStarted; // 게임이 시작되었으면 비활성화
                CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = roomInfo.Name;
                CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            }
            else
            {
                CellBtn[i].interactable = false;
                CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";
                CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }

          
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
        /*if (PlayPanel.activeSelf == true)
        {
            
        }*/
        
    }

    [PunRPC]
    void ReSetMynumRPC(string name)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length && PhotonNetwork.PlayerList[i].NickName == name)
            {
                playercharint[i] = 0;
                for (int j = i; j < PhotonNetwork.PlayerList.Length; j++)
                {
                    playerReady[j] = playerReady[j + 1];
                    playercharint[j] = playercharint[j + 1];
                }
            }
            
        }
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateGameState", RpcTarget.AllBuffered, playerReady, playercharint,1);
        }

    }

    [PunRPC]
    void SetMynumRPC(string name, int num)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length && PhotonNetwork.PlayerList[i].NickName == name)
            {
                playercharint[i] = num;
            }
        }
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateGameState", RpcTarget.AllBuffered, playerReady, playercharint,0);
        }
        
    }

    [PunRPC]
    void UpdateGameState(bool[] playerReady2, int[] playercharint2, int leaveSig)//레디 상황과 선택한 캐릭터를 활성화 시키는 함수
    {
        playerReady = playerReady2;
        playercharint = playercharint2;
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length- leaveSig)
            {
                playerBtn[i].interactable = true;
                playerBtn[i].transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                if (playerReady2[i])
                {
                    if(i!=0)
                    {
                        playerBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 0f);  // RGB: 255, 255, 0
                    }
                }
                else
                {
                    playerBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);  // RGB: 255, 255, 255
                }
                for(int j=0; j<4;j++)//겹치는 문제 해결
                {
                    PlayerChar[i].transform.GetChild(j).gameObject.SetActive(false);
                }
                PlayerChar[i].transform.GetChild(playercharint[i]).gameObject.SetActive(true);//플레이어가 선택한 캐릭터를 활성화해라

            }
            else
            {
                playerBtn[i].interactable = false;
                PlayerChar[i].transform.GetChild(playercharint[i]).gameObject.SetActive(false);
                playerBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";
                playerBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);  // RGB: 255, 255, 255
            }
        }
    }

    public void Ready()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length >= 1)
            {
                bool sig = true;
                for (int i = 1; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (!playerReady[i])
                    {
                        sig = false;
                    }
                }

                if (sig)
                {
                    photonView.RPC("GameStartRPC", RpcTarget.All);
                }
            }
        }
        else
        {
            int num = 0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
                {
                    num = i;
                    break;
                }
            }
            photonView.RPC("ReadyRPC", RpcTarget.All, num);
        }
        
        
    }

    [PunRPC]
    void ReadyRPC(int num)
    {
        if (!playerReady[num])
        {
            playerReady[num] = true;
            playerBtn[num].GetComponent<Image>().color = new Color(1f, 1f, 0f);  // RGB: 255, 255, 0
        }
        else
        {
            playerReady[num] = false;
            playerBtn[num].GetComponent<Image>().color = new Color(1f, 1f, 1f);  // RGB: 255, 255, 255
        }
    }
    

    [PunRPC]
    void GameStartRPC()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        ReadyBTN.interactable = false;
        ExitBTN.interactable = false;
        countImage.gameObject.SetActive(true);
        startImage.gameObject.SetActive(false);

        countImage.sprite = numberSprites[3];
        yield return new WaitForSeconds(1f);

        countImage.sprite = numberSprites[2];
        yield return new WaitForSeconds(1f);

        countImage.sprite = numberSprites[1];
        yield return new WaitForSeconds(1f);

        countImage.gameObject.SetActive(false);
        startImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        startImage.gameObject.SetActive(false);

        // 씬 로드 시작
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
        SceneManager.LoadScene("Game Scene");
        ReadyBTN.interactable = true;
        ExitBTN.interactable = true;
        if(PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable gameStartedProps = new ExitGames.Client.Photon.Hashtable { { "isGameStarted", true } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(gameStartedProps);
        }
    }

    public static void 방복귀()
    {
        ExitGames.Client.Photon.Hashtable gameStartedProps = new ExitGames.Client.Photon.Hashtable { { "isGameStarted", false } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(gameStartedProps);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Game Scene이 로드되었을 때 실행
        if (scene.name == "Game Scene")
        {
            for(int i=0; i<4; i++)
            {
                playerReady[i] = false;
                playerBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
            Lobby캔버스.SetActive(false); // Lobby Canvas 비활성화
            countImage.gameObject.SetActive(false); // countImage 비활성화

            SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 해제
        }
    }

    IEnumerator UpdateLoadingText(Text temp, string S)
    {
        while (true)
        {
            Debug.Log("되는중");
            temp.text = S + ".";
            yield return new WaitForSeconds(0.2f);
            temp.text = S + "..";
            yield return new WaitForSeconds(0.2f);
            temp.text = S + "...";
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Send()
    {
        string senderName = PhotonNetwork.NickName;
        photonView.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text, senderName);
        ChatInput.text = "";
        ChatInput.ActivateInputField();
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg, string senderName)
    {
        if (senderName == PhotonNetwork.NickName)
        {
            msg = "<color=yellow>" + msg + "</color>";
        }
        else
        {
            msg = "<color=white>" + msg + "</color>";
        }
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }


    public void ClickPlayBTN()
    {
        PlayPanel.SetActive(true);
        HomePanel.SetActive(false);
        상점Panel.SetActive(false);
        설정Panel.SetActive(false);
        내정보Panel.SetActive(false);
        Money.SetActive(false);
        MyListRenewal();
    }

    public void ClickHomeBTN()
    {
        HomePanel.SetActive(true);
        Money.SetActive(true);
        PlayPanel.SetActive(false);
        내정보Panel.SetActive(false);
        상점Panel.SetActive(false);
        설정Panel.SetActive(false);
        MakeRoomPanel.SetActive(false);
    }

    public void ClickEnter내정보()//firebase의 데이터를 읽고 활성화 여부를 결정
    {
        상점Panel.SetActive(false);
        설정Panel.SetActive(false);
        MakeRoomPanel.SetActive(false);
        Money.SetActive(true);
        내정보Panel.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (firebaseLogin.playerInfo.Character[i] == 0)
            {
                CharactorBTN[i].interactable = false;
            }
            else
            {
                CharactorBTN[i].interactable = true;
            }
        }

        for(int i=0; i<2; i++)
        {
            if (firebaseLogin.playerInfo.Item[i] == 0)
            {
                SkillBtn[i].interactable = false;
            }
            else
            {
                SkillBtn[i].interactable = true;
            }
        }
    }

    public void ClickExit내정보()
    {
        내정보Panel.SetActive(false);
    }

    public void ClickEnter상점()
    {
        내정보Panel.SetActive(false);
        설정Panel.SetActive(false);
        MakeRoomPanel.SetActive(false);
        상점Panel.SetActive(true);
        Money.SetActive(true);
    }

    public void ClickExit상점()
    {
        상점Panel.SetActive(false);
        내정보Panel.SetActive(false);
        설정Panel.SetActive(false);
        MakeRoomPanel.SetActive(false);
        이미보유Panel.SetActive(false);
        Money.SetActive(true);
    }

    public void ClickCharactorImage(int num)//쥐를 선택하는 함수
    {
        if (firebaseLogin.playerInfo.Character[2*num] ==1)
        {
            for (int i = 0; i < GameChar1.Length; i++)
            {
                GameChar1[i].SetActive(false);
                GameChar2[i].SetActive(false);

            }
            GameChar1[num].SetActive(true);
            GameChar2[num].SetActive(true);
            Mycharacter = num;
            for(int i=0; i< mouseCheck.Length; i++)
            {
                mouseCheck[i].SetActive(false);
            }
            mouseCheck[num].SetActive(true);

        }
    }

    public void ClickCharactorImage2(int num)//고양이를 선택하는 함수
    {
        if (firebaseLogin.playerInfo.Character[2*num+1] == 1)
        {
            for (int i = 0; i < GameChar1고양이.Length; i++)
            {
                GameChar1고양이[i].SetActive(false);
                GameChar2고양이[i].SetActive(false);

            }
            GameChar1고양이[num].SetActive(true);
            GameChar2고양이[num].SetActive(true);
            Mycharacter2 = num;

            for (int i = 0; i < catCheck.Length; i++)
            {
                catCheck[i].SetActive(false);
            }
            catCheck[num].SetActive(true);
        }
    }


    public void ClickSkillImage(int num)
    {
        for(int i=0; i<skillImage.Length; i++)
        {
            skillImage[i].gameObject.SetActive(false);
        }
        skillImage[num-1].gameObject.SetActive(true);
        MySkill = num;
        print(num);
        for(int i=0; i<skillCheck.Length; i++)
        {
            skillCheck[i].SetActive(false);
        }
        skillCheck[num-1].SetActive(true);

    }

    public void ClickCharPlus(int num)
    {
        if(num==0&& slotNum>0)
        {
            slotNum--;
        }
        else if(num==1&& slotNum<1)
        {
            slotNum++;
        }

        for (int i = 0; i < 2; i++)
        {
            charSlotPanel[i].SetActive(false);
            charSlot상점Panel[i].SetActive(false);
        }
        charSlotPanel[slotNum].SetActive(true);
        charSlot상점Panel[slotNum].SetActive(true);
    }

    public void Click상점Charactor(int num)
    {
        구매확인Panel.SetActive(true);
    }
    public void Click상점CharactorExit()
    {
        구매확인Panel.SetActive(false);
    }

    public void 내정보스킬Click()
    {
        캐릭터Panel.SetActive(false);
        스킬Panel.SetActive(true);
        상점캐릭터Panel.SetActive(false);
        상점스킬Panel.SetActive(true);
    }

    public void 내정보캐릭터Click()
    {
        캐릭터Panel.SetActive(true);
        스킬Panel.SetActive(false);
        상점캐릭터Panel.SetActive(true);
        상점스킬Panel.SetActive(false);
        Money.SetActive(true);
    }

    public void 설정Click()
    {
        내정보Panel.SetActive(false);
        상점Panel.SetActive(false);
        MakeRoomPanel.SetActive(false);
        설정Panel.SetActive(true);
    }

    public void MakeRoomPanelClick()
    {
        상점Panel.SetActive(false);
        내정보Panel.SetActive(false);
        설정Panel.SetActive(false);
        MakeRoomPanel.SetActive(true);
    }
    public void 이미보유PanelExit()
    {
        이미보유Panel.SetActive(false);
    }
}
