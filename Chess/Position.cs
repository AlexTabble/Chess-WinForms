using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    static class Position
    {
        private static Board _From;     //sets first clicked square; used in board and piece class extensively
        private static Board _To;       //sets second clicked square; used to relocate piece attributes

        public static Board FromPos
        {
            get { return _From; }
            set
            {
                _From = value;
            }
        }
        public static Board FromCopy { get; private set; }
        public static Board ToCopy { get; set; }


        public static Board ToPos       //ensures only legal moves can be relocated towards
        {
            get { return _To; }
            set { if (Move.GetLegalMoves().Contains(_To)) _To = value; }
        }

        public static void DeletePosition() //after translation, delete piece for next move
        {
            FromPos = null;
            ToPos = null;
        }

        public static void CopyFromPosition(Board[,] copy) //sets from position for copy translation
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (Board.GetBoard()[row, col] == FromPos)
                        FromCopy = copy[row, col];
                }
            }
        }

    }


}
