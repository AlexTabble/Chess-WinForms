using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    static class Move       //logic for when a move happens is here
    {
        private static List<Board> LegalMoves = new List<Board>();

        public static List<Board> GetLegalMoves()
        {
            return LegalMoves;
        }
        public static void NewLegalMoves(List<Board> overwrite) //filteredlegalmoves
        {
            LegalMoves.Clear();
            LegalMoves = new List<Board>(overwrite);

        }
        public static void AddDicMoves(Dictionary<Pieces, List<Board>> moves, Pieces piece)
        {

            List<Board> legalmoves = new List<Board>(Move.GetLegalMoves());
            moves.Add(piece, legalmoves);
            Move.GetLegalMoves().Clear();
        }

        public static void AddLegalMove(Board b)
        {
            LegalMoves.Add(b);
        }

        public static void ClearLegalMoves()    //after a piece has moved or another piece or tile is clicked
        {
            LegalMoves.Clear();
        }

        public static void MovePiece(Board from, Board to)  //if a legal square is clicked
        {
            if (to.Piece != null)
                KillPiece(from, to);

            to.Panel.BackgroundImage = from.Piece.Image;                                
            to.Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            LongCastle(from, to);
            ShortCastle(from, to);
            EnPassant(from, to);

            to.Piece = from.Piece;                                                      

            to.Piece.Row = to.Row;
            to.Piece.Col = to.Col;

            from.Piece = null;                                                          
            from.Panel.BackgroundImage = null;   
            
            if (from == Position.FromPos) Position.DeletePosition();                                                                                                   

        }
        public static Board MovePiece(Board from, Board to, Board[,] copy)
        {
            // Move the piece to the new position on the board copy

            if (to.Piece != null)
            {
                KillPiece(from, to, copy);
            }
            else
            {
                copy[to.Row, to.Col].Piece = copy[from.Row, from.Col].Piece;
                copy[to.Row, to.Col].Piece.Row = to.Row;
                copy[to.Row, to.Col].Piece.Col = to.Col;
            }


            copy[from.Row, from.Col].Piece = null;

            return copy[to.Row, to.Col];
        }


        private static void KillPiece(Board from, Board to)
        {
            Pieces.GetAllPieces().Remove(to.Piece);
            to.Piece = null;
            to.Panel.BackgroundImage = null;
        }
        private static void KillPiece(Board from, Board to, Board[,] copy)
        {
            copy[to.Row, to.Col].Piece = null;
            copy[to.Row, to.Col].Piece = copy[from.Row, from.Col].Piece;

            copy[to.Row, to.Col].Piece.Row = to.Row;
            copy[to.Row, to.Col].Piece.Col = to.Col;

            copy[from.Row, from.Col].Piece = null;
        }
        private static void LongCastle(Board from, Board to)
        {
            if (from.Piece.Piecetype == PieceType.King && to.Col == 2)
            {
                Board rook = Board.GetBoard()[from.Row, 0];
                Board toRook = Board.GetBoard()[to.Row, to.Col + 1];

                MovePiece(rook, toRook);

            }
        }
        private static void ShortCastle(Board from, Board to)
        {
            if (from.Piece.Piecetype == PieceType.King && to.Col == 6)
            {
                Board rook = Board.GetBoard()[from.Row, 7];
                Board toRook = Board.GetBoard()[to.Row, to.Col - 1];
                MovePiece(rook, toRook);

            }
        }

        private static void BlackEnPassant(Board from, Board to)
        {
            if (Gameflow.GetMoves().Count() != 0 || Movement.IsInsideBoard(to.Row - 1, to.Col))
            {
                return;
            }

            Board PassingPawn = Board.GetBoard()[to.Row - 1, to.Col];
            if (PassingPawn.Piece != null && PassingPawn.Piece.Player == PlayerType.White)
            {
                PieceType IsPawnPassed = PassingPawn.Piece.Piecetype;
                if (from.Piece.Piecetype == PieceType.Pawn && IsPawnPassed == PieceType.Pawn)
                    KillPiece(from, PassingPawn);
            }
        }
        private static void WhiteEnPassant(Board from, Board to)
        {

            if (Gameflow.GetMoves().Count() == 0 || !Movement.IsInsideBoard(to.Row + 1, to.Col))
            {
                return;
            }

            Board PassingPawn = Board.GetBoard()[to.Row + 1, to.Col];
            if (PassingPawn.Piece != null && PassingPawn.Piece.Player == PlayerType.Black)
            {
                PieceType IsPawnPassed = PassingPawn.Piece.Piecetype;
                if (from.Piece.Piecetype == PieceType.Pawn && IsPawnPassed == PieceType.Pawn)
                    KillPiece(from, PassingPawn);
            }


        }
        private static void EnPassant(Board from, Board to)
        {
            
            if (from.Piece.Player == PlayerType.White) 
                WhiteEnPassant(from, to);

            if (from.Piece.Player == PlayerType.Black) 
                BlackEnPassant(from, to);
        }
    }
}
