using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class FENstrings // Used for threefold repetition
    {
        private StringBuilder sFEN = new StringBuilder();

        public string Fen { get; private set; }
        public FENstrings(Board[,] board)
        {
            AddPieceData(board);
            sFEN.Append(' ');
            AddCurrentPlayer();
            sFEN.Append(' ');
            CastlingRights();
            sFEN.Append(' ');
            AddEnPassantData(Board.GetBoard(), Gameflow.GetMoves());

            Fen = sFEN.ToString();
        }

        private char PieceChar(Pieces piece)
        {
            char cPiece = '-';
            PieceType type = piece.Piecetype;
            switch (type)
            {
                case PieceType.Pawn:
                    cPiece = 'p';
                    break;
                case PieceType.Bishop:
                    cPiece = 'b';
                    break;
                case PieceType.Knight:
                    cPiece = 'k';
                    break;
                case PieceType.Rook:
                    cPiece = 'r';
                    break;
                case PieceType.Queen:
                    cPiece = 'q';
                    break;
                case PieceType.King:
                    cPiece = 'k';
                    break;
            }

            if(Gameflow.Turn == PlayerType.White)
            {
                char.ToUpper(cPiece);
            }
            return cPiece;
        }
        private void AddRow(Board[,] Board, int row)
        {
            int iEmptySquares = 0;

            for(int col = 0; col < 8; col++)
            {
                if (!Board[row, col].IsPieceOnSquare)
                {
                    iEmptySquares++;
                    continue;
                }

                if(iEmptySquares > 0)
                {
                    sFEN.Append(iEmptySquares.ToString());
                    iEmptySquares = 0;
                }

                sFEN.Append(PieceChar(Board[row, col].Piece));

                if(iEmptySquares > 0)
                {
                    sFEN.Append(iEmptySquares.ToString());
                }
            }
        }

        private void AddPieceData(Board[,] Board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0) sFEN.Append('/');

                AddRow(Board, row);
            }
        }
        
        private void AddCurrentPlayer()
        {
            if(Gameflow.Turn == PlayerType.White)
            {
                sFEN.Append('w');
            }
            else
            {
                sFEN.Append('b');
            }
        }

        private void CastlingRights()
        {
            Board BKing = Gameflow.FindKing(PlayerType.Black, Board.GetBoard());
            Board WKing = Gameflow.FindKing(PlayerType.White, Board.GetBoard());

            bool WhiteCastlingQS = WKing.Piece.CheckCastlingQS(PlayerType.White);
            bool WhiteCastlingKS = WKing.Piece.CheckCastlingKS(PlayerType.White);

            bool BlackCastlingQS = BKing.Piece.CheckCastlingQS(PlayerType.Black);
            bool BlackCastlingKS = BKing.Piece.CheckCastlingKS(PlayerType.Black);

            if(!(WhiteCastlingKS || WhiteCastlingQS || BlackCastlingKS || BlackCastlingQS))
            {
                sFEN.Append('-');
                return;
            }

            if (WhiteCastlingKS)
            {
                sFEN.Append('K');
            }
            if (WhiteCastlingQS)
            {
                sFEN.Append("Q");
            }
            if (BlackCastlingKS)
            {
                sFEN.Append('k');
            }
            if (BlackCastlingQS)
            {
                sFEN.Append('q');
            }
        }
        private void AddEnPassantData(Board[,] board, List<GameMoves> Moves)
        {
            List<bool> isEnpassant = new List<bool>(64);
            foreach(Board square in board)
            {
                if (!square.IsPieceOnSquare || square.Piece.Piecetype != PieceType.Pawn) continue;

                if (square.Piece.IsEnpassantible(Moves))
                {
                    isEnpassant.Add(true);
                    GameMoves TargetMove = Moves.Where(move => move.ToSquare == square).First();
                    if(square.Piece.Player == PlayerType.White)
                    {
                        sFEN.Append(TargetMove.FromSquare.Row - 1 + '.' + TargetMove.FromSquare.Col);
                    }
                    else
                    {
                        sFEN.Append(TargetMove.FromSquare.Row + 1 + '.' + TargetMove.FromSquare.Row);
                    }
                }
                else
                {
                    isEnpassant.Add(false);
                }

                if (!isEnpassant.Contains(true))
                {
                    sFEN.Append('-');
                }
            }
        }

    }
}
