using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }    
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(8, 17);

    int firstSpawn;
    TetrominoData data;
    public GameObject gameover;
    public GameObject touchRange;
    

    static Board _instance;
    public static Board Instance { get => _instance; }
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }
       
    public void Awake()
    {       
        this.InitData();
    }
    public void Start()
    {
        this.GetFirstSpawn();        
    }
    void InitData()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    void GetFirstSpawn()
    {
        firstSpawn = Random.Range(0, this.tetrominos.Length);
        data = this.tetrominos[firstSpawn];

        this.SpawnPiece();
    }       
    
    public void SpawnPiece()
    {
        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition)){
            TimeBar.Instance.AnimaBar();

            Set(this.activePiece);
            
            BoardReview.Instance.ResetNextPiece();
            data = this.tetrominos[BoardReview.Instance.nextFirst];
        }
        else
        {
            Time.timeScale = 0;
            gameover.SetActive(true);
            touchRange.SetActive(false);
        }              
    }
    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {           
            Vector3Int tilePosition = piece.cells[i] + new Vector3Int(piece.position.x, piece.position.y, 0);
            this.tilemap.SetTile(tilePosition, piece.data.tile);

            /*Vector3Int tilePosition2 = piece.cells[i] + new Vector3Int(piece.position.x -3, piece.position.y - 3, 0);
            this.tilemap.SetTile(tilePosition2, piece.data.tile);
            Vector3Int tilePosition3 = piece.cells[i] + new Vector3Int(piece.position.x+3, piece.position.y - 7, 0);
            this.tilemap.SetTile(tilePosition3, piece.data.tile);*/
        }
    }
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition,null);
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i =0; i < piece.cells.Length; i++)
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

        while(row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row +1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row ++;
        }
    }

}
