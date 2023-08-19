using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageManager;

public class StageManager : MonoBehaviour
{
    public enum EDir { Front, Back, Left, Right }
    public enum EFloorState { Start, Normal, Gold, Boss }

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

        StartCoroutine("GenerateMapCoroutine");
    }

    private IEnumerator GenerateMapCoroutine()
    {
        GameObject mapGo = null;
        foreach(SRoomPos room in listRoom)
        {
            if (room.rank == 0)
                mapGo = Instantiate(floorPrefabs[(int)EFloorState.Start], transform);
            else if (room.rank < goldRank)
                mapGo = Instantiate(floorPrefabs[(int)EFloorState.Normal], transform);
            else if (room.rank < bossRank)
                mapGo = Instantiate(floorPrefabs[(int)EFloorState.Gold], transform);
            else
                mapGo = Instantiate(floorPrefabs[(int)EFloorState.Boss], transform);

            float xPos = (room.x - (arrayLength >> 1)) * paddingX;
            float zPos = (room.y - (arrayLength >> 1)) * paddingZ;

            mapGo.transform.position = new Vector3(xPos, 0f, zPos);

            yield return new WaitForSeconds(0.1f);

            SetBridge(room.x, room.y, mapGo);

            listRoomGo.Add(mapGo);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResetMap()
    {
        foreach (GameObject go in listRoomGo)
            Destroy(go);

        arrayLength = Mathf.Clamp((int)(minRoomCnt * 0.7f), 10, 31);
        arrayRoom = new int[arrayLength, arrayLength];
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
        goldRank = 0;
        bossRank = 0;

        // 시작지점 초기화
        listRoom.Clear();
        System.Array.Clear(arrayRoom, 0, arrayRoom.Length);

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
                if (MyMathf.CheckRange(listRoom[i].x, 1, arrayLength - 1) &&
                    MyMathf.CheckRange(listRoom[i].y, 1, arrayLength - 1))
                    tempRoomCnt += SetRoomPos(listRoom[i].x, listRoom[i].y, prevRank + 1, ref randomRoom);
            }

            prevRoomCnt = ttlRoomCnt;
            ttlRoomCnt += tempRoomCnt;
            ++prevRank;
        }

        goldRank = prevRank + 1;
        bossRank = prevRank + 2;

        tempRoomCnt = 0;

        for (int i = 0; i < ttlRoomCnt; ++i)
        {
            if (MyMathf.CheckRange(listRoom[i].x, 1, arrayLength - 1) &&
                MyMathf.CheckRange(listRoom[i].y, 1, arrayLength - 1))
                tempRoomCnt += SetRoomPos(listRoom[i].x, listRoom[i].y, goldRank, ref randomRoom, 4, true);
            if (tempRoomCnt > 1)
                break;
        }
        if (tempRoomCnt != 2)
            return false;

        tempRoomCnt = 0;
        for (int i = 0; i < ttlRoomCnt; ++i)
        {
            if (MyMathf.CheckRange(listRoom[i].x, 1, arrayLength - 1) &&
                MyMathf.CheckRange(listRoom[i].y, 1, arrayLength - 1))
                tempRoomCnt += SetRoomPos(listRoom[i].x, listRoom[i].y, bossRank, ref randomRoom, 4, true);
            if (tempRoomCnt > 0)
                break;
        }
        if (tempRoomCnt != 1)
            return false;

        return true;

        // 랭크 1에 해당하는 방을 생성한다.
        // 2번째로 생기는 방은 반드시 2~4개의 방이 생성됨.


        // 최대 뻗어나갈 수 있는 방의 랭크 개수를 가지고 계산하자.
        // 만약 누군가가 4르 ㄹ입력한다면 배열은 4의 2배 + 3 크기로 만들자.( 좌 우 끝에 비우기 위함)
    }

    private int SetRoomPos(int _x, int _y, int _rank, ref int _randomRoom, int _roomCnt = 2, bool isSpecialRoom = false)
    {
        List<SRoomPos> listRoomPos = new List<SRoomPos>();
        SRoomPos roomPos;

        if (_randomRoom >= 4 && _roomCnt == 2)
        {
            _roomCnt = 3;
            _randomRoom = -1;
        }


        if (arrayRoom[_x, _y + 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y + 1, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --_roomCnt;

        if (arrayRoom[_x, _y - 1].Equals(0))
        {
            roomPos = new SRoomPos(_x, _y - 1, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --_roomCnt;

        if (arrayRoom[_x - 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x - 1, _y, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --_roomCnt;

        if (arrayRoom[_x + 1, _y].Equals(0))
        {
            roomPos = new SRoomPos(_x + 1, _y, _rank, true);
            listRoomPos.Add(roomPos);
        }
        else
            --_roomCnt;


        while (listRoomPos.Count > _roomCnt && listRoomPos.Count > 0)
            listRoomPos.RemoveAt(Random.Range(0, listRoomPos.Count));

        if (listRoomPos.Count > 0)
        {
            if(isSpecialRoom)
            {
                // 다시 이 반복문을 돌려서 새로 만들어질 그 방이 하나의 방과 연결되어있는지 확인.
                for (int i = 0; i < listRoomPos.Count; ++i)
                {
                    if (!IsRoomIsolated(listRoomPos[0].x, listRoomPos[0].y))
                        continue;
                    else
                    {
                        arrayRoom[listRoomPos[i].x, listRoomPos[i].y] = 1 + _rank;
                        listRoom.Add(listRoomPos[i]);
                        return 1;
                    }
                }

                return 0;
            }

            for (int i = 0; i < listRoomPos.Count; ++i)
                arrayRoom[listRoomPos[i].x, listRoomPos[i].y] = 1 + _rank;

            listRoom.AddRange(listRoomPos.ToArray());
        }
        ++_randomRoom;

        return listRoomPos.Count;
    }

    private bool IsRoomIsolated(int _x, int _y)
    {
        int tempRoomCnt = 0;
        if (_y < arrayLength - 1)
        {
            if (arrayRoom[_x, _y + 1] > 0)
                ++tempRoomCnt;
        }
        if (_y > 0)
        {
            if(arrayRoom[_x, _y - 1] > 0)
                ++tempRoomCnt;
        }
        if (_x < arrayLength - 1)
        {
            if (arrayRoom[_x + 1, _y] > 0)
                ++tempRoomCnt;
        }
        if (_x > 0)
        {
            if (arrayRoom[_x - 1, _y] > 0)
                ++tempRoomCnt;
        }

        if (tempRoomCnt > 1)
            return false;

        return true;
    }

    private void SetBridge(int _x, int _y, GameObject _parentGo)
    {
        GameObject tempGo = null;

        tempGo = SelectWallOrDoor(_x, _y + 1, EDir.Front, _parentGo);
        tempGo.transform.localPosition = Vector3.zero;

        tempGo = SelectWallOrDoor(_x, _y - 1, EDir.Back, _parentGo);
        tempGo.transform.localPosition = Vector3.zero;

        tempGo = SelectWallOrDoor(_x - 1, _y, EDir.Left, _parentGo);
        tempGo.transform.localPosition = Vector3.zero;

        tempGo = SelectWallOrDoor(_x + 1, _y, EDir.Right, _parentGo);
        tempGo.transform.localPosition = Vector3.zero;
    }

    private GameObject SelectWallOrDoor(int _x, int _y, EDir _dir, GameObject _parentGo)
    {
        if (!MyMathf.CheckRange(_x, 0, arrayLength) || !MyMathf.CheckRange(_y, 0, arrayLength))
            return Instantiate(wallPrefabs[(int)_dir], _parentGo.transform);

        if (arrayRoom[_x, _y] > 0)
            return Instantiate(doorPrefabs[(int)_dir], _parentGo.transform);
        else
            return Instantiate(wallPrefabs[(int)_dir], _parentGo.transform);
    }


    private void Awake()
    {
        arrayLength = Mathf.Clamp((int)(minRoomCnt * 0.7f), 10, 31);
        arrayRoom = new int[arrayLength, arrayLength];
        System.Array.Clear(arrayRoom, 0, arrayRoom.Length);
        listRoom = new List<SRoomPos>();
        listRoomGo = new List<GameObject>();
    }



    [SerializeField]
    private GameObject[] floorPrefabs = null;
    [SerializeField]
    private GameObject[] wallPrefabs = null;
    [SerializeField]
    private GameObject[] doorPrefabs = null;
    [SerializeField]
    private int minRoomCnt = 0;
    [SerializeField]
    private float paddingX = 0f;
    [SerializeField]
    private float paddingZ = 0f;

    private int arrayLength = 0;
    private int bossRank = 0;
    private int goldRank = 0;

    private int[,] arrayRoom = null;

    private List<SRoomPos> listRoom = null;
    private List<GameObject> listRoomGo = null;
}
