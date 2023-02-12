using UnityEngine;

public class PieceReview : MonoBehaviour
{
    public BoardReview boardReview { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

/*    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float _stepTime;
    private float _lockTime;*/

    public void Initialize(BoardReview board, Vector3Int position, TetrominoData data)
    {
        this.boardReview = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

       /* this._stepTime = Time.time + this.stepDelay;
        this._lockTime = 0f;*/

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length]; //4 blocks
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    /*public void Update()
    {
       // this.boardReview.Clear(this);

        //this._lockTime += Time.deltaTime;

        //this.Movement();

        *//*if (Time.time >= this._stepTime)
        {
            Step();
        }*//*

        //this.boardReview.Set(this);
    }*/
    /*private void Step()
    {
       // this._stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        *//*if (this._lockTime >= this.lockDelay)
        {
            Lock();
        }*//*
    }*/
    void Lock()
    {
        this.boardReview.Set(this);
        this.boardReview.ClearLines();
        this.boardReview.SpawnPiece();
    }
    private void Movement()
    {
        //rotate
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }
        //movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
    }
    private void HardDrop()
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

        bool valid = this.boardReview.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            //this._lockTime = 0f;
        }
        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex += Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
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

            switch (this.data.tetromino)
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

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++) //5 test
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

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
}
