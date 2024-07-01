using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class King : Pieces
    {
        private bool _CanCastle = true;
        private List<Board> _CastlingRow = new List<Board>();
        public bool CanCastle
        {
            get { return _CanCastle; }
            set
            {
                _CanCastle = value;
            }
        }
        private bool WasMoved { get; set; }
        public King(int y, int x, PieceType p, PlayerType f)
            : base(y, x, p, f)
        {
            Row = y;
            Col = x;
            Player = f;
            Piecetype = PieceType.King;
        }

        public override Pieces Copy()
        {
            return new King(this.Row, this.Col, this.Piecetype, this.Player);
        }
       
        private void KingMoves(PlayerType oppenent, Board from, Board[,] type)
        {
            CheckPossibleMove(Movement.Up(from, 1, type), oppenent);
            CheckPossibleMove(Movement.Down(from, 1, type), oppenent);
            CheckPossibleMove(Movement.Left(from, 1, type), oppenent);
            CheckPossibleMove(Movement.Right(from, 1, type), oppenent);
            CheckPossibleMove(Movement.UpLeft(from, 1, 1, type), oppenent);
            CheckPossibleMove(Movement.UpRight(from, 1, 1, type), oppenent);
            CheckPossibleMove(Movement.DownLeft(from, 1, 1, type), oppenent);
            CheckPossibleMove(Movement.DownRight(from, 1, 1, type), oppenent);
            Castling(oppenent);
        }

        private void LongCastle(PlayerType opponent, List<Board> castling)
        {
            Board[] castlingRow = castling.ToArray();

            if (CheckCastlingQS(opponent))
            {
                CheckPossibleMove(castlingRow[2], opponent);
            }

        }
        private void ShortCastle(PlayerType opponent, List<Board> castling)
        {
            Board[] castlingRow = castling.ToArray();
            if (CheckCastlingKS(opponent))
            {
                CheckPossibleMove(castlingRow[6], opponent);

            }
        }
        private void Castling(PlayerType opponent)
        {

            if (Player == PlayerType.White)
            {
                if (Row != 7 || Col != 4) WasMoved = true;
            }
            if (Player == PlayerType.Black)
            {
                if (Row != 0 || Col != 4) WasMoved = true;
            }

            if (Gameflow.GameState == GameState.Check || WasMoved == true) CanCastle = false;

            if (Piecetype == PieceType.King && CanCastle)
            {

                for (int i = 0; i < Board.GetBoard().GetLength(1); i++)
                {
                    Board square = Board.GetBoard()[CurrentSquare(Board.GetBoard()).Row, i];
                    _CastlingRow.Add(square);
                }

                LongCastle(opponent, _CastlingRow);
                ShortCastle(opponent, _CastlingRow);
            }
        }
        public override void CalculatePossibleMoves(Board from, Board[,] type)
        {
            if (Gameflow.Turn == PlayerType.Black || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.Black) KingMoves(PlayerType.White, from, type);
            }
            if (Gameflow.Turn == PlayerType.White || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.White) KingMoves(PlayerType.Black, from, type);
            }

        }
        private void GetCastlingRow()
        {
            for (int i = 0; i < Board.GetBoard().GetLength(1); i++)
            {
                Board square = Board.GetBoard()[CurrentSquare(Board.GetBoard()).Row, i];
                _CastlingRow.Add(square);
            }
        }

        public override bool CheckCastlingQS(PlayerType player)
        {
            if (!CanCastle || WasMoved) return false;

            GetCastlingRow();
            Board[] CastleRow = _CastlingRow.ToArray();

            if (!CastleRow[1].IsPieceOnSquare && !CastleRow[2].IsPieceOnSquare && !CastleRow[3].IsPieceOnSquare)
            {
                return true;
            }

            return false;
        }

        public override bool CheckCastlingKS(PlayerType player)
        {
            if (!CanCastle || WasMoved) return false;

            GetCastlingRow();
            Board[] CastleRow = _CastlingRow.ToArray();

            if(!CastleRow[5].IsPieceOnSquare && !CastleRow[6].IsPieceOnSquare)
            {
                return true;
            }
            return false;
        }

        public override bool CanPromote()
        {
            throw new System.NotImplementedException();
        }
        public override bool IsEnpassantible(List<GameMoves> Moves)
        {
            throw new NotImplementedException();
        }

    }
}
