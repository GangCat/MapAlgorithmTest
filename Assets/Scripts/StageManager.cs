using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public struct SRoomPos
    {
        public int x;
        public int y;
        public int rank;
        public bool isRoom;

        public SRoomPos(int _x, int _y, int _rank, bool _isRoom)
        {
            x = _x;
            y = _y;
            rank = _rank;
            isRoom = _isRoom;
        }
    }

    public void GenerateMap()
    {
        while (!SetRoomPosition())
            ResetMap();

        for (int i = 0; i < arrayLength; ++i)
        {
            for (int j = 0; j < arrayLength; ++j)
            {
                if (arrayRoom[i, j].Equals(0)) continue;

                GameObject mapGo = Instantiate(mapPrefab, transform);

                float xPos = (j - (arrayLength >> 1)) * paddingX;
                float zPos = (i - (arrayLength >> 1)) * paddingZ;

                mapGo.transform.position = new Vector3(xPos, 0f, zPos);
                listRoomGo.Add(mapGo);
            }
        }

    }

    public void ResetMap()
    {
        foreach (GameObject go in listRoomGo)
            Destroy(go);

        System.Array.Clear(arrayRoom, 0, arrayRoom.Length);
        listRoom.Clear();
        listRoomGo.Clear();
    }

    private bool SetRoomPosition()
    {
        int ttlRoomCnt = 0;
        int prevRoomCnt = 0;
        int tempRoomCnt = 0;
        int prevRank = 0;
        int randomRoom = 0;

        // 시작지점 초기화
        SRoomPos roomPos = new SRoomPos(arrayLength >> 1, arrayLength >> 1, 0, true);
        arrayRoom[arrayLength >> 1, arrayLength >> 1] = 1;
        listRoom.Add(roomPos);

        ++ttlRoomCnt;

        while (ttlRoomCnt < minRoomCnt)
        {
            if (prevRank > minRoomCnt && ttlRoomCnt < minRoomCnt)
                return false;

            tempRoomCnt = 0;
            for (int i = prevRoomCnt; i < ttlRoomCnt; ++i)
            {
                if (MyMathf.CheckRange(listRoom[i].x, 1, arrayLength - 2) &&
                    MyMathf.CheckRange(listRoom[i].y, 1, arrayLength - 2))
                    tempRoomCnt += SetRoomPos(listRoom[i].x, listRoom[i].y, prevRank + 1, ref randomRoom);
            }

            prevRoomCnt = ttlRoomCnt;
            ttlRoomCnt += tempRoomCnt;
            ++prevRank;
        }

        return true;

        // 랭크 1에 해당하는 방을 생성한다.
        // 2번째로 생기는 방은 반드시 2~4개의 방이 생성됨.


        // 최대 뻗어나갈 수 있는 방의 랭크 개수를 가지고 계산하자.
        // 만약 누군가가 4르 ㄹ입력한다면 배열은 4의 2배 + 3 크기로 만들자.( 좌 우 끝에 비우기 위함)
    }

    private int SetRoomPos(int _x, int _y, int _rank, ref int _randomRoom)
    {
        List<SRoomPos> listRoomPos = new List<SRoomPos>();
        SRoomPos roomPos;
        //int maxRange = Mathf.Clamp(5 - (_rank>>1), 1, 5);
        int roomCnt = Random.Range(1, 3);
        if (_randomRoom == 5)
        {
            roomCnt = 3;
            _randomRoom = -1;
        }
        

        if (arrayRoom[_x, _y + 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y + 1, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --roomCnt;

        if (arrayRoom[_x, _y - 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y - 1, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --roomCnt;

        if (arrayRoom[_x + 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x + 1, _y, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --roomCnt;

        if (arrayRoom[_x - 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x - 1, _y, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --roomCnt;

        while (listRoomPos.Count > roomCnt && listRoomPos.Count > 0)
            listRoomPos.RemoveAt(Random.Range(0, listRoomPos.Count));

        if (listRoomPos.Count > 0)
        {
            for (int i = 0; i < listRoomPos.Count; ++i)
                arrayRoom[listRoomPos[i].x, listRoomPos[i].y] = 1;

            listRoom.AddRange(listRoomPos.ToArray());
        }
        ++_randomRoom;

        return listRoomPos.Count;
    }

    private void Awake()
    {
        arrayLength = (int)(minRoomCnt * 2);
        arrayRoom = new int[arrayLength, arrayLength];
        System.Array.Clear(arrayRoom, 0, arrayRoom.Length);
        listRoom = new List<SRoomPos>();
        listRoomGo = new List<GameObject>();
    }



    [SerializeField]
    private GameObject mapPrefab = null;
    [SerializeField]
    private int minRoomCnt = 0;
    [SerializeField]
    private float paddingX = 0f;
    [SerializeField]
    private float paddingZ = 0f;

    private int arrayLength = 0;

    private int[,] arrayRoom = null;

    private List<SRoomPos> listRoom = null;
    private List<GameObject> listRoomGo = null;
}
