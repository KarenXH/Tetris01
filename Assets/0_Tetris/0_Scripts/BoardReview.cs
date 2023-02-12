using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardReview : Singleton<BoardReview>
{
    public Tilemap tilemap { get; private set; }
    public PieceReview activePiece { get; private set; }
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(7, 17);
       
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

   

    public int nextFirst;
    public int nextSecond;
    public int nextThird;
    [SerializeField] private BoardReview _instance;
    public BoardReview Instance { get => _instance; set => _instance = value; }


    public void Awake()
    {
        if (_instance == null)
            _instance = FindObjectOfType(typeof(BoardReview)) as BoardReview;

        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<PieceReview>();

        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
        this.NextPiece();
    }


    public void LateUpdate()
    {        
        this.SpawnPiece();
    }
    void NextPiece()
    {
        nextFirst = Random.Range(0, this.tetrominos.Length);
        nextSecond = Random.Range(0, this.tetrominos.Length);
        nextThird = Random.Range(0, this.tetrominos.Length);        
    }
    public void ResetNextPiece()
    {
        nextFirst = nextSecond;
        nextSecond = nextThird;
        nextThird = Random.Range(0, this.tetrominos.Length);
        this.GameOver();
    }

    public void SpawnPiece()
    {       
        TetrominoData data1 = this.tetrominos[nextFirst];
        TetrominoData data2 = this.tetrominos[nextSecond];
        TetrominoData data3 = this.tetrominos[nextThird];
                
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-0, 0), data1);
        Set(this.activePiece);
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-4, 0), data2);
        Set(this.activePiece);
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-8, 0), data3);
        Set(this.activePiece);




    }
    

    public void Set(PieceReview piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void Clear(PieceReview piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(PieceReview piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }


    public void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }

}

