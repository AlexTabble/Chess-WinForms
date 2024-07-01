using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public enum PromoType
    {
        Queen,Knight,Bishop,Rook
    }
    public static class Promotion
    {
        private static Pieces PromoPawn { get; set; }
        
        private static void CreatePromotionForm()
        {
            Board square = PromoPawn.CurrentSquare(Board.GetBoard());
            Point FormLocation = square.Panel.Location;

            Form promo = new Form()
            {
                BackColor = Color.Black,
                Text = "Pawn Promotion",
                Width = 300,
                Height = 150,
                Location = new Point(FormLocation.X, FormLocation.Y + 40)
            };

            // Create and configure buttons for each promotion choice
            PieceType[] pieces = { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight };
            for (int i = 0; i < pieces.Length; i++)
            {
                Button button = new Button
                {

                    Location = new Point(10 + (i * 70), 50),
                    Width = 40,
                    Height = 40,
                    BackgroundImage = GetImage(pieces[i], PromoPawn.Player),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Name = pieces[i].ToString()
                };
                button.Click += (sender, e) =>
                {
                    AddPromoPiece(button.Name);

                    promo.Close();
                };
                promo.Controls.Add(button);
            }

            // Show the form
            promo.ShowDialog();
            promo.Dispose();
            promo.Close();

        }
        public static void HandlePromotion(Pieces Piece)
        {
            PromoPawn = Piece;
            CreatePromotionForm();
        }

        private static string GetPieceArt(PieceType piece,PlayerType player)
        {
            return Board.GetFileName(piece, player);
        }
        private static Image GetImage(PieceType e, PlayerType player)
        {
            return Image.FromFile(GetPieceArt(e, player));
        }
        private static void AddPromoPiece(string btnName)
        {
            switch (btnName)
            {
                case "Queen":
                    Pieces PromoQueen = new Queen(PromoPawn.Row, PromoPawn.Col, PieceType.Queen, PromoPawn.Player);
                    GeneratePiece(PromoQueen, PieceType.Queen);
                    break;
                case "Rook":
                    Pieces PromoRook = new Rook(PromoPawn.Row, PromoPawn.Col, PieceType.Rook, PromoPawn.Player);
                    GeneratePiece(PromoRook, PieceType.Rook);

                    break;
                case "Bishop":
                    Pieces PromoBishop = new Bishop(PromoPawn.Row, PromoPawn.Col, PieceType.Bishop, PromoPawn.Player);
                    GeneratePiece(PromoBishop, PieceType.Bishop);

                    break;
                case "Knight":
                    Pieces PromoKnight = new Knight(PromoPawn.Row, PromoPawn.Col, PieceType.Knight, PromoPawn.Player);
                    GeneratePiece(PromoKnight, PieceType.Knight);

                    break;
            }
        }
        private static void GeneratePiece(Pieces PromoPiece,PieceType piecetype)
        {
            //add new piece and remove pawn piece art
            Pieces.AddPiece(PromoPiece);
            Board.GetBoard()[PromoPawn.Row, PromoPawn.Col].Panel.BackgroundImage = null;

            //get promotion image and set it
            PromoPiece.Image = GetImage(piecetype, PromoPawn.Player);
            Board.GetBoard()[PromoPawn.Row, PromoPawn.Col].Panel.BackgroundImage = PromoPiece.Image;

            //set new piece and remove pawn
            Pieces.GetAllPieces().Remove(Board.GetBoard()[PromoPawn.Row,PromoPawn.Col].Piece);
            Board.GetBoard()[PromoPawn.Row, PromoPawn.Col].Piece = PromoPiece;
        }
    }
}
