using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chess
{

    public static class Gameflow
    {
        private static Board _King;
        private static List<GameMoves> _GameMoves = new List<GameMoves>();

        private static Dictionary<Pieces, List<Board>> CurrentEnemyMoves = new Dictionary<Pieces, List<Board>>();
        private static Dictionary<Pieces, List<Board>> CurrentPlayerMoves = new Dictionary<Pieces, List<Board>>();

        public static Dictionary<string, int> History = new Dictionary<string,int>();
        
        private static Board King
        {
            get { return _King; }
            set { _King = value; }
        }
        
        public static GameState GameState { get; set; }
        public static PlayerType Turn { get; set; }
        public static bool IsTestRun { get; private set; }
        private static Board[,] Copy = new Board[8, 8];

        private static PlayerType Opponent { get; set; }
        public static List<GameMoves> GetMoves()
        {
            return _GameMoves;
        }
        public static void RecordMove(Board From, Board To)
        {
            GameMoves move = new GameMoves(From, To);
            if(From != null && To != null)
            _GameMoves.Add(move);   
        }

        public static void ChangeTurn()
        {
            Turn = (Turn == PlayerType.Black) ? PlayerType.White : PlayerType.Black;

            Opponent = (Turn == PlayerType.Black) ? PlayerType.White : PlayerType.Black;
        }

        public static bool DetermineGameState() //if any of the enemy moves contain king, check for checkmate and stalemate
        {
            Copy = Board.CreateCopy();
            GetCurrentEnemyMoves(Opponent);
            King = FindKing(Turn, Copy);
            Board.DeleteCopy(Copy);

            CheckForDraws();

            if (CurrentEnemyMoves.Any(p => p.Value.Contains(King)))
            {
                GameState = GameState.Check;
                CheckForMates();
                CheckForDraws();
                return true;
            }
            else
            {
                GameState = GameState.Normal;
                return false;
            }
        }
       
        private static void GetCurrentEnemyMoves(PlayerType opponent) // all possible moves for current enemy
        {

            IsTestRun = true;
            CurrentEnemyMoves.Clear();

            foreach (Board b in Copy)
            {
                Pieces enemy = b.Piece;
                if (enemy != null && enemy.Player == opponent)
                {
                    IsTestRun = true;
                    enemy.CalculatePossibleMoves(b, Copy);
                    Move.AddDicMoves(CurrentEnemyMoves, enemy);
                }
            }

            IsTestRun = false;
        }
        private static void GetCurrentPlayerMoves(PlayerType opponent) //All possible moves for current player
        {
            IsTestRun = true;

            CurrentPlayerMoves.Clear();
            
            foreach (Board b in Copy)
            {
                Pieces current = b.Piece;

                if (current != null && current.Player != opponent)
                {
                    
                    current.CalculatePossibleMoves(b, Copy);
                    Move.AddDicMoves(CurrentPlayerMoves, current);
                    Move.ClearLegalMoves();
                }
            }
            IsTestRun = false;
        }
        public static Board FindKing(PlayerType player, Board[,] board)
        {
            foreach (Board b in board)
            {
                if (b != null && b.Piece != null && b.Piece.Piecetype == PieceType.King && b.Piece.Player == player)
                {

                    return b;
                }
            }
            MessageBox.Show("something is broken");
            
            return null;
        }
        public static bool IsKingInChecked(Board move) // makes simulated move and gets all possible enemy moves
        {
            GetCurrentEnemyMoves(Opponent);

            foreach (KeyValuePair<Pieces, List<Board>> key in CurrentEnemyMoves) // if enemy moves contains king, player in check
            {
                if (key.Value.Contains(King))
                {
                    return true;
                }
            }
            return false;
        }


        public static void FilterLegalMoves(Pieces CurrentPiece) // for each possible move of selected piece, determine if its legal
        {
            IsTestRun = false;
            CurrentPiece.CalculatePossibleMoves(Position.FromPos, Board.GetBoard());
            List<Board> UnfilteredLegalMoves = new List<Board>(Move.GetLegalMoves());  // all possible moves on ACTUAL board
            Move.ClearLegalMoves();

            IsTestRun = true;
            List<Board> FilteredLegalMoves = new List<Board>();

            for (int i = 0; i < UnfilteredLegalMoves.Count(); i++)
            {
                Copy = Board.CreateCopy();                              //for each actual possible move simulate it on a copied board
                Position.CopyFromPosition(Copy);                        //board copy is always of actual board before a move

                Board move = UnfilteredLegalMoves.ElementAt(i);
                Board move_test = UnfilteredLegalMoves.ElementAt(i);
                Move.MovePiece(Position.FromCopy, move_test, Copy);

                foreach(Board b in Copy)
                {
                    if (move_test.Row == b.Row && move_test.Col == b.Col) move_test = b; // ensures correct referencing of actualboard
                }

                King = FindKing(CurrentPiece.Player, Copy);

                if (!IsKingInChecked(move_test))        //make a possible move; if king is not in checked after simulated move, its legal
                {
                    FilteredLegalMoves.Add(move);
                }

                Board.DeleteCopy(Copy);

            }
            
            Move.NewLegalMoves(FilteredLegalMoves);    //Overwrite legal moves with the actual legal moves
            IsTestRun = false;
        }
        private static bool HasLegalMoves()
        {
            Copy = Board.CreateCopy();

            GetCurrentPlayerMoves(Opponent);

            foreach(KeyValuePair<Pieces,List<Board>> playermoves in CurrentPlayerMoves)
            {
                foreach(Board move in playermoves.Value)
                {
                    King = null;
                    Board.DeleteCopy(Copy);
                    Copy = Board.CreateCopy();
                    Board From = playermoves.Key.CurrentSquare(Copy);
                    Board _move = Move.MovePiece(From, move, Copy);
                    King = FindKing(Turn, Copy);
                    if (!IsKingInChecked(_move))
                    {
                        Board.DeleteCopy(Copy);
                        return true;
                    };
                }
            }
            Board.DeleteCopy(Copy);
            return false;
        }
        public static void CheckForMates()
        {
            Copy = null;
            IsTestRun = true;


            // If no legal moves are possible, determine whether checkmate or stalemate
            if (!HasLegalMoves())
            {
                
                Copy = Board.CreateCopy();
                King = FindKing(Turn, Copy);

                GetCurrentEnemyMoves(Opponent);
                if (CurrentEnemyMoves.Values.Any(moves => moves.Contains(King)))
                {
                    Gameflow.GameState = GameState.CheckMate;
                }
                else
                {
                    Gameflow.GameState = GameState.Stalemate;
                }
            }
            else
            {
                //If legal moves are possible, determine if king is still in check 
                Copy = Board.CreateCopy();
                King = FindKing(Turn, Copy);
                GetCurrentEnemyMoves(Opponent);
                if (!CurrentEnemyMoves.Values.Any(moves => moves.Contains(King))) GameState = GameState.Normal;
            }
                
            if(GameState == GameState.CheckMate || GameState == GameState.Stalemate)
            {
                ChangeTurn();
                string sMsg = string.Format("{0} wins by {1}\n\nPLAY AGAIN?", Turn.ToString(), GameState.ToString());

                DialogResult dlg = new DialogResult();

                dlg = MessageBox.Show(sMsg, "GAME OVER", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(dlg == DialogResult.Yes)
                {
                    Application.Restart();
                }
                if(dlg == DialogResult.No)
                {
                    Application.Exit();
                }
            }
        }
        private static void CheckForDraws()
        {
            if (GameState == GameState.Draw_InsufficientMaterial)
            {
                GenerateResult(GameState);
            }
            else if(GameState == GameState.Draw_ThreeFoldRepitition)
            {
                GenerateResult(GameState);
            }
            else if(GameState == GameState.Draw_FiftyMoveRule)
            {
                GenerateResult(GameState);
            }
        }
        private static void GenerateResult(GameState result)
        {
            DialogResult dlg = new DialogResult();

            dlg = MessageBox.Show("PLAY AGAIN?", result.ToString(), MessageBoxButtons.YesNo,MessageBoxIcon.Information);

            if(dlg == DialogResult.Yes)
            {
                Application.Restart();
            }
            
            if(dlg == DialogResult.No)
            {
                Application.Exit();
            }
        }

        
        public static bool ThreeFoldRepetition(FENstrings fen)
        {
            if (History.Values.Contains(3)) return true;

            return false;
        }
        public static void AddFEN(FENstrings fen)
        {
            bool FirstRepetition = History.ContainsKey(fen.Fen);
            bool SecondRepetition = FirstRepetition && History[fen.Fen] == 2;

            if (FirstRepetition) History[fen.Fen] = 2;

            if (SecondRepetition) History[fen.Fen] = 3;

            if (!FirstRepetition) History.Add(fen.Fen, 1);
        }
    }
}
