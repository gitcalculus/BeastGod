using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;

/// <summary>
/// </summary>
public class BeastBrain : MonoBehaviour
{
    public AIBase moveBase;
    public List<Transform> targets;
    public Transform target;
    public SpriteRenderer Renderer;
    public float EndDistance = 0.5f;
    public int FoodCountToLevelUp = 1;
    public int LevelToSpawnBeast = 5;
    public int DestroyLevelDiff = 2;
    public GameObject BeastPrefab;
    private int _eatFoodCount = 0;
    private Vector3 _randomTargetPos;
    public int Level = 0;
    // 白绿蓝紫橙
    public Color[] LevelColor = {
        new Color(1, 1, 1, 1),
        new Color(0.5f, 1, 0.5f, 1),
        new Color(0.5f, 0.5f, 1, 1),
        new Color(1, 0.5f, 1, 1),
        new Color(1, 0.5f, 0.5f, 1),
    };

    public void Start()
    {
        targets = new List<Transform>();
        Renderer.color = LevelColor[Level];
        _randomTargetPos = this.transform.position;
        HUDViewMono.Inst.RegisterBeast(this);
    }

    public void OnDestroy()
    {
        HUDViewMono.Inst.UnRegisterBeast(this);
    }

    public void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) < EndDistance)
            {
                EatFood();
            }
        }
        else if (targets.Count > 0)
        {
            FindTarget();
        }
        else
        {
            // stop move
            // moveBase.destination = transform.position;
            // random move
            if (null != PlayerInputHandle.Inst)
            {
                if (Vector3.Distance(transform.position, _randomTargetPos) < EndDistance)
                {
                    _randomTargetPos = PlayerInputHandle.Inst.RandomPos();
                    moveBase.destination = _randomTargetPos;
                }
            }
        }
    }

    public void EatFood()
    {
        if (null != target)
        {
            _eatFoodCount++;
            if (_eatFoodCount >= FoodCountToLevelUp)
            {
                _eatFoodCount = 0;
                Level++;
                if (Level >= LevelToSpawnBeast)
                {
                    Level = 0;
                    SpawnBeast();
                }
                Renderer.color = LevelColor[Level];
            }
            Destroy(target.gameObject);
        }
        target = null;
        FindTarget();
    }

    public void SpawnBeast()
    {
        var beast = Instantiate(BeastPrefab);
        beast.transform.position = transform.position;
        beast.name = "Beast";
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            if (targets.Contains(other.transform))
            {
                return;
            }
            targets.Add(other.transform);
            FindTarget();
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int otherLevel = other.gameObject.GetComponent<BeastBrain>().Level;
            if (Level - otherLevel >= DestroyLevelDiff)
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void FindTarget()
    {
        int index = GetClosestFoodIndex();
        while (null == target && targets.Count > 0 && index >= 0)
        {
            target = targets[index];
            targets.RemoveAt(index);
        }
        if (null != target)
        {
            moveBase.endReachedDistance = EndDistance;
            moveBase.destination = target.position;
        }
        else
        {
            _randomTargetPos = transform.position;
        }
    }

    public int GetClosestFoodIndex()
    {
        int nearestFoodIndex = -1;
        float minDistance = float.MaxValue;
        foreach (var food in targets)
        {
            if (null == food)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, food.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestFoodIndex = targets.IndexOf(food);
            }
        }
        return nearestFoodIndex;
    }
}