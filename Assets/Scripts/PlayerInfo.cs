using UnityEngine;
using Firebase.Firestore;
[FirestoreData]//Firestore���� �ν��ϱ� ���� �� ���������.
public struct PlayerInfo
{
    [FirestoreProperty]
    public string NickName { get; set; }
}
