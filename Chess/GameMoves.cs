using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct GameMoves //For en passant,fifty move rule and insufficient material
    {
        public Board FromSquare { get; set; }
        public Board ToSquare { get; set; }
        public bool IsEnPassant { get; set; }
        public Pieces MovedPiece { get; set; }
        public PieceType PieceType { get; set; }
        public PlayerType Player { get; set; }

        private bool IsPlayerCaptured { get; set; }
        public GameMoves(Board _from, Board _to)
        {
            FromSquare = _from;
            ToSquare = _to;
            MovedPiece = _from.Piece;
            PieceType = _from.Piece.Piecetype;
            Player = _from.Piece.Player;


            FENstrings fen = new FENstrings(Board.GetBoard());
            Gameflow.AddFEN(fen);

            if (_to.Piece != null) IsPlayerCaptured = true;
            else IsPlayerCaptured = false;

            IsEnPassant = false;

            IsMaterialSufficient();
            FiftyMoveRule();
            ThreeFoldRepetition(fen);
        }

        private void IsMaterialSufficient()
        {
            List<Pieces> AllPieces = Pieces.GetAllPieces();
            List<Pieces> BlackPieces = new List<Pieces>();
            List<Pieces> WhitePieces = new List<Pieces>();

            foreach (Pieces p in AllPieces)
            {
                if (p.Player == PlayerType.White) WhitePieces.Add(p);
                if (p.Player == PlayerType.Black) BlackPieces.Add(p);
            }


            bool isWhiteSufficient = WhitePieces.Any(
                piece => piece.Piecetype == PieceType.Pawn ||
                         piece.Piecetype == PieceType.Queen ||
                         piece.Piecetype == PieceType.Rook
                );

            bool isBlackSufficient = BlackPieces.Any(
                piece => piece.Piecetype == PieceType.Pawn ||
                         piece.Piecetype == PieceType.Queen ||
                         piece.Piecetype == PieceType.Rook
                );

            if (!isWhiteSufficient && !isBlackSufficient)
            {
                Gameflow.GameState = GameState.Draw_InsufficientMaterial;
            }
        }
        private void FiftyMoveRule()
        {
            List<GameMoves> Moves = Gameflow.GetMoves();
            bool isFiftyMoveRule = false;
            if(Moves.Count() > 50)
            {
                List<GameMoves> LastFiftyMoves = Moves.Skip(Moves.Count() - 50).ToList();
                isFiftyMoveRule = Moves.All(
                    move => move.IsPlayerCaptured == false &&
                    move.PieceType != PieceType.Pawn);
            }

            if (isFiftyMoveRule)
            {
                Gameflow.GameState = GameState.Draw_FiftyMoveRule;
            }
        }
        private void ThreeFoldRepetition(FENstrings fen)
        {
            if (Gameflow.ThreeFoldRepetition(fen))
            {
                Gameflow.GameState = GameState.Draw_ThreeFoldRepitition;
            }
        }
    }

}
