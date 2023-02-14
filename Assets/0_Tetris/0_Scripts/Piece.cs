using UnityEngine;
public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }    
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }    

    static Piece _instance;
    public static Piece Instance { get => _instance;}

    public float stepDelay = 5f;
    public float lockDelay = 0.25f;

    private float _stepTime;
    private float _lockTime;

    bool _isClicked = false;
    bool _isMovedLeft = false;
    bool _isMovedRight = false;
    bool _isMovedDrop = false;

    Tetromino dataClone1;
    
    private void Awake()
    {
        Piece._instance = this;
    }
    public void Initialize(Board board,Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

        this._stepTime = Time.time + this.stepDelay;
        this._lockTime = 0f;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length]; //4 blocks
        }

        for(int i = 0; i< data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int) data.cells[i];
        }
    }    

    public void Update()
    {
        this.SpawnGhost();
    }
    void SpawnGhost()
    {
        this.board.Clear(this);

        this._lockTime += Time.deltaTime;

        if (Time.timeScale == 1)
        {
            this.Movement();
            this.RandomRotate();           
        }

        if (Time.time >= this._stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }
    

    private void Step()
    {
        this._stepTime = Time.time + this.stepDelay;
               
        HardDrop();

        if(this._lockTime >= this.lockDelay)
        {
            Lock();            
        }
    }
    void Lock()
    {
        TimeBar.Instance.ResetAnimaBar();
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }
    public void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;            
        }
        Lock();
    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this,newPosition);

        if (valid)
        {
            this.position = newPosition;
            this._lockTime = 0f;
        }
        return valid;
    }
    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex += Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex,direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }
    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.data.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            dataClone1 = this.data.tetromino;
            switch (dataClone1)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i=0; i < this.data.wallKicks.GetLength(1); i++) //5 test
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex,i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }
    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));//9 situation
    }

    private void Movement()
    {
        //rotate
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }*/
        //movement
        if(_isMovedLeft == true)
        {
            Move(Vector2Int.left);
            _isMovedLeft = false;
        }
        if(_isMovedRight == true)
        {
            Move(Vector2Int.right);
            _isMovedRight = false;
        }
        if(_isMovedDrop == true)
        {
            this.HardDrop();
            _isMovedDrop = false;
        }
        

        /*if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }*/
    }

    public void CheckClicked()
    {
        _isClicked = true;
    } 
    public void CheckMovedLeft()
    {
        _isMovedLeft = true;
    } 
    public void CheckMovedRight()
    {
        _isMovedRight = true;
    } 
    public void CheckMovedDrop()
    {
        _isMovedDrop = true;
    }   
    void RandomRotate()
    {
        if (_isClicked == true)
        {
            float a = Random.Range(-1f, 1f);
            if (a < 0)
            {
                a = -1;
            }
            else
            {
                a = 1;
            }
            Rotate((int)a);

            _isClicked = false;
        }
    }


}
