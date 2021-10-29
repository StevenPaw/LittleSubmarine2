using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public class MovingBlock : MonoBehaviour, IPushable
{
    [SerializeField] private Transform movePoint;
    [Range(0.0f, 10.0f)] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PushableTypes blockType;
    [SerializeField] private Animator anim;
    [SerializeField] private HistoryManager history;
    [SerializeField] private Collider2D selfCollider;

    public Transform MovePoint
    {
        get => movePoint;
        set => movePoint = value;
    }

    public Collider2D SelfCollider
    {
        get => selfCollider;
        set => selfCollider = value;
    }

    public HistoryManager History
    {
        get => history;
        set => history = value;
    }

    public Animator Anim
    {
        get => anim;
        set => anim = value;
    }
    
    private void Start()
    {
        movePoint.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void UndoPush()
    {
        Command lastCommand = history.getUndoMove();
        lastCommand.Undo();
    }
    
    public bool Push(Vector2 direction)
    {
        if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SOLID))
        {
            return false;
        }
        else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SPECIALTILES))
        {
            SpecialTile tile = Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SPECIALTILES)
                .GetComponent<SpecialTile>();
            if (tile.GetTileType() != SpecialTileTypes.PUSHABLE)
            {
                if (TileHelper.CanUseSpecialTile(tile, direction, true, this))
                {
                    return true;
                }
            }
        }
        else
        {
            Command move = new Move(this, direction);
            if (move.Execute())
            {
                return true;
            }
        }

        return false;
    }

    public PushableTypes GetPushableType()
    {
        return blockType;
    }
}
}