using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Pawn : Pieces
    {
        bool _CanPromote;
        private bool PawnCanPromote
        {
            get
            {
                if (Gameflow.IsTestRun != true)
                {
                    if (Player == PlayerType.White && Row == 0) return _CanPromote = true;
                    if (Player == PlayerType.Black && Row == 7) return _CanPromote = true;
                    else return _CanPromote = false;
                }
                else return _CanPromote = false;

            }
            set
            {
                _CanPromote = value;
            }
        }

        public Pawn(int y, int x, PieceType p, PlayerType f)
            : base(y, x, p, f)
        {
            Row = y;
            Col = x;
            Player = f;
            Piecetype = PieceType.Pawn;
        }

        public override Pieces Copy() //if a boardpanel is not null, invoke this
        {
            return new Pawn(this.Row, this.Col, this.Piecetype, this.Player);
        }

        public override bool CanPromote()
        {
            return PawnCanPromote;
        }

        private void PawnUp(Board from, Board[,] type) // checks both pawn possibilities; fuck en passant
        {
            Board forwardone = Movement.Up(from, 1, type);

            if (forwardone != null && !forwardone.IsPieceOnSquare)
            {
                CheckPossibleMove(forwardone, PlayerType.Black);
            }
            Board forwardtwo = Movement.Up(from, 2, type);
            if (from.Row == 6 && !forwardtwo.IsPieceOnSquare && !forwardone.IsPieceOnSquare)
            {
                CheckPossibleMove(forwardtwo, PlayerType.Black);
            }
        }
        private void PawnDown(Board from, Board[,] type)
        {
            Board DownOne = Movement.Down(from, 1, type);
            if (DownOne != null && !DownOne.IsPieceOnSquare)
            {
                CheckPossibleMove(DownOne, PlayerType.White);
            }
            Board DownTwo = Movement.Down(from, 2, type);
            if (from.Row == 1 && !DownTwo.IsPieceOnSquare && !DownOne.IsPieceOnSquare)
            {
                CheckPossibleMove(DownTwo, PlayerType.White);
            }
        }
        private void PawnUpLeft(Board from, Board[,] type)
        {
            Board UpLeft = Movement.UpLeft(from, 1, 1, type);
            PawnDiagonalCheck(UpLeft, PlayerType.Black);
        }
        private void PawnUpRight(Board from, Board[,] type)
        {
            Board UpRight = Movement.UpRight(from, 1, 1, type);
            PawnDiagonalCheck(UpRight, PlayerType.Black);
        }
        private void PawnDownLeft(Board from, Board[,] type)
        {
            Board DownLeft = Movement.DownLeft(from, 1, 1, type);
            PawnDiagonalCheck(DownLeft, PlayerType.White);
        }
        private void PawnDownRight(Board from, Board[,] type)
        {
            Board DownRight = Movement.DownRight(from, 1, 1, type);
            PawnDiagonalCheck(DownRight, PlayerType.White);
        }
        private void PawnDiagonalCheck(Board diag, PlayerType opponent)
        {
            if (diag != null && diag.IsPieceOnSquare)
            {
                if (diag.Piece.Player == opponent)
                {
                    CheckPossibleMove(diag, opponent);
                }
            }
        }

        public override bool IsEnpassantible(List<GameMoves> Moves)
        {

            int LastMoveIndex = Moves.Count() - 1;
            if (Moves.Count() != 0)
            {
                GameMoves LastMove = Moves.ElementAt(LastMoveIndex);



                if (Player == PlayerType.White && Piecetype == PieceType.Pawn)
                {
                    if (LastMove.FromSquare.Row == 1 && LastMove.ToSquare.Row == 3)
                    {
                        if (Row == LastMove.ToSquare.Row && (Col == LastMove.ToSquare.Col - 1 || Col == LastMove.ToSquare.Col + 1))
                            return true;
                    }
                }
                else if (Player == PlayerType.Black && Piecetype == PieceType.Pawn)
                {
                    if (LastMove.FromSquare.Row == 6 && LastMove.ToSquare.Row == 4)
                    {
                        if (Row == LastMove.ToSquare.Row && (Col == LastMove.ToSquare.Col - 1 || Col == LastMove.ToSquare.Col + 1))
                            return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void EnPassant(Board[,] type)
        {
            GameMoves Square;
            int LeftSquareCol = 0;
            int RightSquareCol = 0;
            List<GameMoves> CurrentMoves = Gameflow.GetMoves();
            if (CurrentMoves.Count() != 0)
            {
                Square = CurrentMoves.ElementAt(CurrentMoves.Count() - 1);
                LeftSquareCol = Square.ToSquare.Col + 1;
                RightSquareCol = Square.ToSquare.Col - 1;
            }


            if (IsEnpassantible(CurrentMoves))
            {
                if (Player == PlayerType.White)
                {
                    if (Col == LeftSquareCol)
                    {
                        Board ToSquare = Movement.UpLeft(Position.FromPos, 1, 1, type);
                        CheckPossibleMove(ToSquare, PlayerType.Black);
                    }

                    if (Col == RightSquareCol)
                    {
                        Board ToSquare = Movement.UpRight(Position.FromPos, 1, 1, type);
                        CheckPossibleMove(ToSquare, PlayerType.Black);
                    }
                }
                if (Player == PlayerType.Black)
                {
                    if (Col == LeftSquareCol)
                    {
                        Board ToSquare = Movement.DownLeft(Position.FromPos, 1, 1, type);
                        CheckPossibleMove(ToSquare, PlayerType.White);
                    }

                    if (Col == RightSquareCol)
                    {
                        Board ToSquare = Movement.DownRight(Position.FromPos, 1, 1, type);
                        CheckPossibleMove(ToSquare, PlayerType.White);
                    }
                }
            }

        }

        public override void CalculatePossibleMoves(Board from, Board[,] type)
        {

            if (Gameflow.Turn == PlayerType.Black || Gameflow.IsTestRun)   //Checks if its black's move
            {
                if (Player == PlayerType.Black)
                {
                    //Basic Pawn Movement
                    PawnDown(from, type);

                    //black pawn taking a piece possibility
                    PawnDownLeft(from, type);
                    PawnDownRight(from, type);

                    EnPassant(type);
                }
            }
            if (Gameflow.Turn == PlayerType.White || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.White)
                {
                    //Basic Pawn Movement
                    PawnUp(from, type);

                    //white pawn taking a piece possibility
                    PawnUpLeft(from, type);
                    PawnUpRight(from, type);

                    EnPassant(type);
                }

            }

        }
        public override bool CheckCastlingKS(PlayerType player)
        {
            throw new NotImplementedException();
        }
        public override bool CheckCastlingQS(PlayerType player)
        {
            throw new NotImplementedException();
        }
    }
}
