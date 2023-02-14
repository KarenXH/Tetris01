using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board mainBoard;
    public Piece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();        
        cells = new Vector3Int[4];
    }
   
    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();       
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
        /*for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition1 = cells[i] + new Vector3Int(position.x - 3, position.y, 0);
            tilemap.SetTile(tilePosition1, null);
        }
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition2 = cells[i] + new Vector3Int(position.x +3 , position.y, 0);
            tilemap.SetTile(tilePosition2, null);
        }*/
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = trackingPiece.cells[i];
        }
    }

    public void Drop()
    {
        Vector3Int position = trackingPiece.position;

        int currentRow = position.y;
        int bottom = -mainBoard.boardSize.y / 2 - 1;

        mainBoard.Clear(trackingPiece);

        for (int row = currentRow; row >= bottom; row--)
        {
            position.y = row;

            if (mainBoard.IsValidPosition(trackingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        mainBoard.Set(trackingPiece);
        Set();
        /*Set1();
        Set2();*/
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, tile);
        }
    }
    /*private void Set1()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition1 = cells[i] + new Vector3Int(position.x - 3, position.y, 0);
            tilemap.SetTile(tilePosition1, tile);
        }
    }
    private void Set2()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition2 = cells[i] + new Vector3Int(position.x + 3, position.y, 0);
            tilemap.SetTile(tilePosition2, tile);
        }
    }*/
}
