using System.Drawing;
using System.Windows.Forms;
namespace Chess
{
    public partial class ChessFrm : Form
    {
        public ChessFrm()
        {
            InitializeComponent();
           
            var black = Color.Black;
            var White = Color.DarkGray;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var Panel = new Panel() //instantiate a panel and declare its properties
                    {
                        Size = new Size(40, 40),
                        Location = new Point(i * 40 + 60, j * 40 + 60)


                    };
                    //Assign panel to form and to panel array

                    Controls.Add(Panel); //add panel to formm

                    Board board = new Board(j, i, Panel); //instantiate a board square
                    Board.AddBoardPanel(j,i, board);      //add board square to grid

                    //Panel colors
                    if (i % 2 == 0)
                    {
                        Panel.BackColor = black;

                        if (j % 2 == 0)
                        {
                            Panel.BackColor = White;
                        }
                    }
                    else
                    {
                        Panel.BackColor = White;
                        if (j % 2 == 0)
                        {
                            Panel.BackColor = black;
                        }


                    }
                    //Clicking EventHandler
                    Panel.Click += board.ClickedPanel;


                }//y-axis loop
            }//x-axis loop

            Board.GenerateChessPieces();        //Generates initial starting pieces

            Gameflow.Turn = PlayerType.White;
            Gameflow.GameState = GameState.Normal;
        }
         
    }
}


