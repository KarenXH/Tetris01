using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardReview : MonoBehaviour
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

    private static BoardReview _instance;
    public static BoardReview Instance { get => _instance; }

    public int nextFirst;
    public int nextSecond;
    public int nextThird;
   
    public void Awake()
    {
        this.InitData();
    }

    void InitData()
    {
        BoardReview._instance = this;

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
    public void NextPiece()
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
                
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-8, 0), data1);
        Set(this.activePiece);
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-4, 0), data2);
        Set(this.activePiece);
        this.activePiece.Initialize(this, new Vector3Int(this.spawnPosition.x, this.spawnPosition.y-0, 0), data3);
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
    public void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }

}

