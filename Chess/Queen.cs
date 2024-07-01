using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Queen : Pieces
    {
        public Queen(int y, int x, PieceType p, PlayerType f)
            : base(y, x, p, f)
        {
            Row = y;
            Col = x;
            Player = f;
            Piecetype = PieceType.Queen;
        }

        public override Pieces Copy()
        {
            return new Queen(this.Row, this.Col, this.Piecetype, this.Player);
        }
        private void QueenMoves(PlayerType opponent, Board from, Board[,] type)
        {
            CheckLineMoves(from, opponent, "up", from, type);
            CheckLineMoves(from, opponent, "down", from, type);
            CheckLineMoves(from, opponent, "left", from, type);
            CheckLineMoves(from, opponent, "right", from, type);
            CheckLineMoves(from, opponent, "upleft", from, type);
            CheckLineMoves(from, opponent, "upright", from, type);
            CheckLineMoves(from, opponent, "downright", from, type);
            CheckLineMoves(from, opponent, "downleft", from, type);
        }

        public override void CalculatePossibleMoves(Board from, Board[,] type)
        {
            if (Gameflow.Turn == PlayerType.Black || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.Black) QueenMoves(PlayerType.White, from, type);
            }
            if (Gameflow.Turn == PlayerType.White || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.White) QueenMoves(PlayerType.Black, from, type);
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
