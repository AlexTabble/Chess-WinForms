using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Knight : Pieces
    {
        public Knight(int y, int x, PieceType p, PlayerType f)
            : base(y, x, p, f)
        {
            Row = y;
            Col = x;
            Player = f;
            Piecetype = PieceType.Knight;
        }

        public override Pieces Copy()
        {
            return new Knight(this.Row, this.Col, this.Piecetype, this.Player);
        }

        private void KnightMoves(PlayerType opponent, Board from, Board[,] type)
        {
            CheckPossibleMove(Movement.UpLeft(from, 1, 2, type), opponent);
            CheckPossibleMove(Movement.UpLeft(from, 2, 1, type), opponent);
            CheckPossibleMove(Movement.DownLeft(from, 1, 2, type), opponent);
            CheckPossibleMove(Movement.DownLeft(from, 2, 1, type), opponent);
            CheckPossibleMove(Movement.UpRight(from, 1, 2, type), opponent);
            CheckPossibleMove(Movement.UpRight(from, 2, 1, type), opponent);
            CheckPossibleMove(Movement.DownRight(from, 1, 2, type), opponent);
            CheckPossibleMove(Movement.DownRight(from, 2, 1, type), opponent);
        }

        public override void CalculatePossibleMoves(Board from, Board[,] type)
        {
            if (Gameflow.Turn == PlayerType.Black || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.Black) KnightMoves(PlayerType.White, from, type);
            }
            if (Gameflow.Turn == PlayerType.White || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.White) KnightMoves(PlayerType.Black, from, type);
            }


        }

        public override bool CanPromote()
        {
            throw new System.NotImplementedException();
        }
        public override bool CheckCastlingKS(PlayerType player)
        {
            throw new NotImplementedException();
        }
        public override bool CheckCastlingQS(PlayerType player)
        {
            throw new NotImplementedException();
        }
        public override bool IsEnpassantible(List<GameMoves> Moves)
        {
            throw new NotImplementedException();
        }
    }
}
