using UnityEngine;
using Firebase.Firestore;
[FirestoreData]//Firestore���� �ν��ϱ� ���� �� ���������.
public struct PlayerInfo
{
    [FirestoreProperty]
    public string NickName { get; set; }
    [FirestoreProperty]
    public int Gold { get; set; }
    [FirestoreProperty]
    public int WinCount { get; set; }
    [FirestoreProperty]
    public int LoseCount { get; set; }
    [FirestoreProperty]
    public int Level { get; set; }
    [FirestoreProperty]
    public int []Character { get; set; }
    [FirestoreProperty]
    public int []Item { get; set; }

}
