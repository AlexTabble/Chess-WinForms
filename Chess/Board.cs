using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace Chess
{

    public class Board
    {
        private bool _IsPieceOnSquare;
        private Pieces _Piece;
        public Pieces Piece
        {
            get { return _Piece; }
            set
            {
                _Piece = value;
            }
        }
        private static Board[,] BoardPanels = new Board[8, 8];
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Panel Panel { get; private set; }
        public bool IsPieceOnSquare
        {
            get { if (Piece != null && this != null) return true;
                else return false;
            }
            set { _IsPieceOnSquare = value; }
        }
        public Board(int y, int x, Panel b) //note (Row,Col) = (y,x) / used in adding squares at runtime
        {
            Row = y;
            Col = x;
            Panel = b;

        }

        public Board(int y, int x, Pieces p) //used in adding board pieces
        {
            Row = y;
            Col = x;
            Piece = p;
        }

        private Board(Board ActualSquare)
        {
            Row = ActualSquare.Row;
            Col = ActualSquare.Col;

            if (ActualSquare.Piece != null)
            {
                Piece = ActualSquare.Piece.Copy();
                Piece.Row = Row;
                Piece.Col = Col;
            }

        }

        //Used to populate Board Array at runtime
        public static void AddBoardPanel(int y, int x, Board panel)
        {
            BoardPanels[y, x] = panel;

        }
        //Retrieves Board array
        public static Board[,] GetBoard()
        {
            return BoardPanels;
        }


        //Logic for when a square is clicked within here
        public void ClickedPanel(object sender, EventArgs e)
        {
            
            if (Piece != null)
            {
                if (Move.GetLegalMoves().Count == 0) // If a piece is clicked and no legal moves are currently shown
                {
                    RevertSquareColor(); // Undo previous legal move squares
                    Position.FromPos = this; // Set selected square as memory
                    Piece.ShowLegalMoves(); // Get legal moves using the selected square

                }
                else if (Move.GetLegalMoves().Contains(this)) // If a piece is clicked and it's a legal move
                {
                    Position.ToPos = this;

                    Gameflow.RecordMove(Position.FromPos, this);
                    Move.MovePiece(Position.FromPos, this); // Move the piece

                    
                    PawnCanPromote();
                    Gameflow.ChangeTurn();
                    Gameflow.DetermineGameState();

                    RevertSquareColor();
                    Move.ClearLegalMoves();

                }
                else // If a piece is clicked but it's not a legal move
                {
                    Position.ToPos = null;

                    RevertSquareColor();
                    Move.ClearLegalMoves(); // Reset legal moves
                }
            }
            else if (Move.GetLegalMoves().Contains(this)) // If an empty square that is a legal move is chosen
            {
                Position.ToPos = this; // Set chosen square in memory

                Gameflow.RecordMove(Position.FromPos, this);
                Move.MovePiece(Position.FromPos, this); // Move the piece

                
                PawnCanPromote();
                Gameflow.ChangeTurn();
                Gameflow.DetermineGameState();

                RevertSquareColor();
                Move.ClearLegalMoves();
                

            }
            else // If an empty square that's not a legal move is chosen
            {
                Position.ToPos = null;

                RevertSquareColor();
                Move.ClearLegalMoves(); // Reset legal moves
            }
        }
        private void RevertSquareColor()
        {
            foreach (Board b in BoardPanels)               //Reset pink red squares if a piece is unselected
            {

                if (b.Row % 2 == 0)
                {
                    b.Panel.BackColor = Color.Black;

                    if (b.Col % 2 == 0)
                    {
                        b.Panel.BackColor = Color.DarkGray;
                    }
                }
                else
                {
                    b.Panel.BackColor = Color.DarkGray;
                    if (b.Col % 2 == 0)
                    {
                        b.Panel.BackColor = Color.Black;
                    }

                }
            }
        }


        public static Board[,] CreateCopy()
        {
            Board[,] Clone = new Board[8, 8];

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (BoardPanels[row, col] != null)
                    {
                        Clone[row, col] = new Board(BoardPanels[row, col]);
                    }
                }
            }

            return Clone;
        }
        public static void DeleteCopy(Board[,] copy)
        {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (copy[row, col].Piece != null)
                        {
                            copy[row, col].Piece = null;
                        }
                        copy[row, col] = null;
                    }
                }
        }
        private void PawnCanPromote()
        {
           if(Piece.Piecetype == PieceType.Pawn)
            {
                if (Piece.CanPromote()) 
                    Promotion.HandlePromotion(Piece);
            }
        }
        //Creates all starting chess pieces when board is loaded
        public static void GenerateChessPieces()
        {
            //Black Piece Generation
            GeneratePiece(0, 0,PieceType.Rook,PlayerType.Black);
            GeneratePiece(0, 1,PieceType.Knight,PlayerType.Black);
            GeneratePiece(0, 2,PieceType.Bishop,PlayerType.Black);
            GeneratePiece(0, 3,PieceType.Queen,PlayerType.Black);
            GeneratePiece(0, 4,PieceType.King,PlayerType.Black);
            GeneratePiece(0, 5,PieceType.Bishop,PlayerType.Black);
            GeneratePiece(0, 6,PieceType.Knight,PlayerType.Black);
            GeneratePiece(0, 7,PieceType.Rook,PlayerType.Black);
            
            //Black/White pawn generation
            for(int i = 0; i <=7; i++)
            {
                GeneratePiece(1,i,PieceType.Pawn,PlayerType.Black);
                GeneratePiece(6,i, PieceType.Pawn, PlayerType.White);
            }

            //White Piece Generation
            GeneratePiece(7, 0,PieceType.Rook,PlayerType.White);
            GeneratePiece(7, 1,PieceType.Knight,PlayerType.White);
            GeneratePiece(7, 2,PieceType.Bishop,PlayerType.White);
            GeneratePiece(7, 3,PieceType.Queen,PlayerType.White);
            GeneratePiece(7, 4,PieceType.King,PlayerType.White);
            GeneratePiece(7, 5 ,PieceType.Bishop,PlayerType.White);
            GeneratePiece(7, 6,PieceType.Knight,PlayerType.White);
            GeneratePiece(7, 7,PieceType.Rook,PlayerType.White);
        }

        
        //Fetches file name for piece art as string
        public static string GetFileName(PieceType p,PlayerType f)
        {
          string[] ChessArt = Directory.GetFiles(@"C:\Users\alexa\OneDrive\Documents\Visual Studio Projects\Chess\Chess\PieceArt");

            switch (p)
            {
                case PieceType.Bishop:
                    if(f == PlayerType.White)
                    {
                        return ChessArt[1];
                    }
                    else if(f == PlayerType.Black)
                    {
                        return ChessArt[0];
                    }
                    break;
               
                case PieceType.King:
                    if(f == PlayerType.Black)
                    {
                        return ChessArt[5];
                    }
                    else if(f == PlayerType.White)
                    {
                        return ChessArt[6];
                    } 
                    break;
               
                case PieceType.Knight:
                    if(f == PlayerType.Black)
                    {
                        return ChessArt[7];
                    }
                    else if(f == PlayerType.White)
                    {
                        return ChessArt[8];
                    }
                   break;
                case PieceType.Pawn:
                    if(f == PlayerType.Black)
                    {
                        return ChessArt[9];
                    }
                    else if(f == PlayerType.White)
                    {
                        return ChessArt[10];
                    }
                     break;
               
                case PieceType.Queen:
                    if(f == PlayerType.Black)
                    {
                        return ChessArt[11];
                    }
                    else if(f == PlayerType.White)
                    {
                        return ChessArt[12];
                    }
                    break;
                case PieceType.Rook:
                    if(f == PlayerType.Black)
                    {
                        return ChessArt[13];
                    }
                    else if(f == PlayerType.White)
                    {
                        return ChessArt[14];
                    }
                  break;
            
            }
            return null;
        }
       
        //Updates Board tile to have the art for the chess piece
        private static Board GeneratePiece(int y, int x,PieceType e,PlayerType p)
        {
            switch (e) // instantiates a new piece of their respective classes and generates said piece on the board
            {
                case PieceType.Pawn:
                    Pieces pawn = new Pawn(y,x, e, p);
                    GeneratePieceArt(y,x,e,p,pawn);
                    break;
                case PieceType.Bishop:
                    Pieces bishop = new Bishop(y, x, e, p);
                    GeneratePieceArt(y, x, e, p, bishop);
                    break;
                case PieceType.King:
                    Pieces king = new King(y, x, e, p);
                    GeneratePieceArt(y, x, e, p, king);
                    break;
                case PieceType.Knight:
                    Pieces knight = new Knight(y, x, e, p);
                    GeneratePieceArt(y, x, e, p, knight);
                    break;
                case PieceType.Queen:
                    Pieces queen = new Queen(y, x, e, p);
                    GeneratePieceArt(y, x, e, p, queen);
                    break;
                case PieceType.Rook:
                    Pieces rook = new Rook(y, x, e, p);
                    GeneratePieceArt(y, x, e, p, rook);
                    break;
            }
            return BoardPanels[y, x];
        }
        private static void GeneratePieceArt(int y, int x,PieceType e,PlayerType p, Pieces piece) // used in above method
        {
            piece.Image = Image.FromFile(GetFileName(e, p));
            BoardPanels[y, x].Piece = piece;
            BoardPanels[y, x].Panel.BackgroundImage = piece.Image;
            BoardPanels[y, x].Panel.BackgroundImageLayout = ImageLayout.Stretch;
            Pieces.AddPiece(piece);
        }
    }
   
    
}
