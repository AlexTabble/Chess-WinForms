using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Bishop : Pieces
    {
        public Bishop(int y, int x, PieceType p, PlayerType f)
            : base(y, x, p, f)
        {
            Row = y;
            Col = x;
            Player = f;
            Piecetype = PieceType.Bishop;
        }

        public override Pieces Copy()
        {
            return new Bishop(this.Row, this.Col, this.Piecetype, this.Player);
        }

        private void BishopMoves(PlayerType opponent, Board from, Board[,] type)
        {
            CheckLineMoves(from, opponent, "upleft", from, type);
            CheckLineMoves(from, opponent, "upright", from, type);
            CheckLineMoves(from, opponent, "downleft", from, type);
            CheckLineMoves(from, opponent, "downright", from, type);
        }


        public override void CalculatePossibleMoves(Board from, Board[,] type)
        {
            if (Gameflow.Turn == PlayerType.Black || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.Black)
                    BishopMoves(PlayerType.White, from, type);
            }
            if (Gameflow.Turn == PlayerType.White || Gameflow.IsTestRun)
            {
                if (Player == PlayerType.White)
                    BishopMoves(PlayerType.Black, from, type);
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
