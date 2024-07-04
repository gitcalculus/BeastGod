using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 当玩家点击时，检查点击位置是否在指定区域内，区域用Tilemap GridPos标识，并使用Tilemap获得点击位置
/// 然后创建一个Food对象
/// </summary>
public class PlayerInputHandle : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector2Int LeftBottom;
    public Vector2Int RightTop;
    public GameObject FoodPrefab;
    public GameObject WallPrefab;
    private static PlayerInputHandle _inst;

    public static PlayerInputHandle Inst
    {
        get
        {
            return _inst;
        }
    }

    public void Awake()
    {
        _inst = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            if (IsInArea(gridPos))
            {
                CreateObject(gridPos, FoodPrefab, 10f);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            if (IsInArea(gridPos))
            {
                CreateObject(gridPos, WallPrefab, 5f);
            }
        }
    }

    private bool IsInArea(Vector3Int gridPos)
    {
        return gridPos.x >= LeftBottom.x && gridPos.x <= RightTop.x && gridPos.y >= LeftBottom.y && gridPos.y <= RightTop.y;
    }

    public Vector3 RandomPos()
    {
        return tilemap.GetCellCenterWorld(
            new Vector3Int(
                UnityEngine.Random.Range(LeftBottom.x, RightTop.x),
                UnityEngine.Random.Range(LeftBottom.y, RightTop.y), 0));
    }

    private void CreateObject(Vector3Int gridPos, GameObject prefab, float lifeTime)
    {
        GameObject food = Instantiate(prefab, tilemap.GetCellCenterWorld(gridPos), Quaternion.identity);
        Destroy(food, lifeTime);
    }
}