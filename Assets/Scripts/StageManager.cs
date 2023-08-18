using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public struct SRoomPos
    {
        public int x;
        public int y;

        public int GetX => x;
        public int GetY => y;

        public SRoomPos(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }


    public void GenerateMap()
    {
        //Instantiate(mapPrefab, transform);
    }

    private void SetRoomPosition()
    {
        // 시작지점 초기화
        arrayRoom[4, 4] = 1;
        SRoomPos roomPos = new SRoomPos(4, 4);
        listRoom0.Add(roomPos);

        // 랭크 1에 해당하는 방을 생성한다.
        List<SRoomPos> listEmptyRoom = new List<SRoomPos>();
        listEmptyRoom = (GetEmptyRoomPos(listRoom0[0].x, listRoom0[0].y).ToList<SRoomPos>());
        
    }

    private SRoomPos[] GetEmptyRoomPos(int _x, int _y)
    {
        List<SRoomPos> listRoomPos = new List<SRoomPos>();
        SRoomPos roomPos;
        if (arrayRoom[_x, _y + 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y + 1);
            listRoomPos.Add(roomPos);
        }

        if (arrayRoom[_x, _y - 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y - 1);
            listRoomPos.Add(roomPos);
        }

        if (arrayRoom[_x + 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x + 1, _y);
            listRoomPos.Add(roomPos);
        }

        if (arrayRoom[_x - 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x - 1, _y);
            listRoomPos.Add(roomPos);
        }
        return listRoomPos.ToArray();
    }

    private void Start()
    {
        // 배열을 0으로 초기화한다.
        System.Array.Clear(arrayRoom, 0, arrayRoom.Length);
    }


    [SerializeField]
    private GameObject mapPrefab = null;

    private int[,] arrayRoom = new int[10, 10];

    private List<SRoomPos> listRoom0 = new List<SRoomPos>();
    private List<SRoomPos> listRoom1 = new List<SRoomPos>();
    private List<SRoomPos> listRoom2 = new List<SRoomPos>();
    private List<SRoomPos> listRoom3 = new List<SRoomPos>();
}
