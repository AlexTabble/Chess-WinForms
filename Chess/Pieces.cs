using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Chess
{
    public enum PieceType
    {
        Pawn,  Bishop, Knight, Rook, Queen,King
    }
    public abstract class Pieces
    {

        public PieceType Piecetype { get; set; }
        public PlayerType Player { get;  set; }
        public Image Image { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
       
        private static List<Pieces> AllPieces = new List<Pieces>();
        public Pieces(int y, int x, PieceType e, PlayerType f)
        {
            Player = f;
            Piecetype = e;
            Row = y;
            Col = x;
        }
       
        public Board CurrentSquare(Board[,] type)
        {
            foreach(Board b in type)
            {
                if(b.Piece != null)
                {
                    if (b.Piece.Piecetype == this.Piecetype && b.Piece.Player == this.Player)
                        return b;
                }
            }

            return null;
        }
        //Will be useful when making gameflow logic
        public static List<Pieces> GetAllPieces()
        {
            return AllPieces;
        }
        
        public static void AddPiece(Pieces p)
        {
            AllPieces.Add(p);
        }

        public abstract Pieces Copy();

        public void ShowLegalMoves()
        {
            Move.ClearLegalMoves();
            Gameflow.DetermineGameState();

            if (Gameflow.Turn == Player)
            {
                 Gameflow.FilterLegalMoves(this);
            }
            
            HighlightLegalMoves(Move.GetLegalMoves());
        }
       
        public abstract void CalculatePossibleMoves(Board from, Board[,] type);
        
        //verify if a move is possible
       protected void CheckPossibleMove(Board b, PlayerType opp)
        {
            if (b == null) //impossible moves are null from the movement class
            {
                return;
            }

            if (!Gameflow.IsTestRun)
            {
                if (b.Piece == null) //if there is no piece on the board but the move is possible
                {
                    Move.AddLegalMove(b);

                }
                else if (b.Piece.Player == opp) //ensures opponent square is also possible
                {
                    Move.AddLegalMove(b);
                }
            }
            else if (Gameflow.IsTestRun)
            {
                if (b.Piece == null) 
                    Move.AddLegalMove(b);

                if (b.Piece != null && b.Piece.Player == opp)
                {
                    Move.AddLegalMove(b);
                }
            }

        }
        private void HighlightLegalMoves(List<Board> legalmoves)
        {
            foreach(Board b in legalmoves)
            {
                b.Panel.BackColor = Color.Violet;
            }
        }
       protected void CheckLineMoves(Board b, PlayerType opp, string dir,Board from,Board[,] type)
        {
            int i = 0;
            while(b != null) //only loops if a move is possible
            {
                Board posSquare = null; 
                i++;
              
                switch (dir) //determines type of line to check and inrements per square
                {
                    case "up":
                        posSquare = Movement.Up(from, i,type);
                        break;
                    case "down":
                        posSquare = Movement.Down(from, i,type);
                        break;
                    case "left":
                        posSquare = Movement.Left(from, i,type);
                        break;
                    case "right":
                        posSquare = Movement.Right(from, i,type);
                        break;
                    case "upleft":
                        posSquare = Movement.UpLeft(from, i, i,type);
                        break;
                    case "upright":
                        posSquare = Movement.UpRight(from, i, i,type);
                        break;
                    case "downleft":
                        posSquare = Movement.DownLeft(from, i, i,type);
                        break;
                    case "downright":
                        posSquare = Movement.DownRight(from, i, i,type);
                        break;
                }
               
                if(posSquare == null) //if a move is out of bounds, it breaks
                {
                    break;
                }

                CheckPossibleMove(posSquare, opp);

                if (posSquare.Piece != null) //if an opponent blocks the line, it breaks
                {
                    break;
                }
                
            }
        }

        public abstract bool CanPromote();
        public abstract bool CheckCastlingQS(PlayerType player);
        public abstract bool CheckCastlingKS(PlayerType player);
        public abstract bool IsEnpassantible(List<GameMoves> Moves);
    }
}
