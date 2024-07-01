using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    static class Movement //all logic for translating pieces on board is here
    {
        //Class is broken; rows and columns attributes switch when it enters here;
        //If done, try to fix
        public static Board Up(Board b, int y,Board[,] type)
        {
            if (IsInsideBoard(b.Row -y, b.Col )) //Checks if possible move is in square
            {
                return type[b.Row -y, b.Col]; //return if it is
            }
            return null;                                //null if not; works with possible move method in piece class
        }
        public static Board Down(Board b, int y,Board[,] type)        //same for the rest
        {
            if (IsInsideBoard(b.Row + y, b.Col))
            {
                return type[b.Row + y, b.Col];
            }
            return null;
        }
        public static Board Left(Board b, int x, Board[,] type)
        {
            if(IsInsideBoard(b.Row,b.Col -x))
            {
                return type[b.Row, b.Col -x];
            }
            return null;
        }
        public static Board Right(Board b, int x, Board[,] type)
        {
            if(IsInsideBoard(b.Row, b.Col + x))
            {
                return type[b.Row, b.Col + x];
            }
            return null;
        }
        public static Board UpLeft(Board b, int x, int y, Board[,] type)   
        {     
            b = Up(b, y, type);                                   //checks if first translation is within board
            if(b != null)   
            {
                b = Left(b, x, type);                             //If it it, checks if second translation is within board
                if (b != null) return b;                    //if both translations are, return the square
            }
            return null;                                    //null if either translation is not within board
        }
        public static Board UpRight(Board b, int x, int y, Board[,] type)  //same for the rest
        {
            b = Up(b, y, type);
            if(b != null)
            {
                b = Right(b, x, type);
                if(b != null)return b;
            }
            return null;
        }
        public static Board DownLeft(Board b, int x, int y,Board[,] type)
        {
            b = Down(b, y, type);
            if (b != null)
            {
                b = Left(b, x, type);
                if(b != null) return b;
            }
            return null;
        }
        public static Board DownRight(Board b, int x, int y, Board[,] type)
        {
            b = Down(b, y, type);
            if (b != null)
            {
                b = Right(b, x, type);
                if (b != null) return b;
            }
            return null;
        }
        public static bool IsInsideBoard(int x, int y)      //Checks if x and y is within board bounds
        {
            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
            {
                return true;
            }
            return false;
            
        }
        public static bool IsInsideBoard(Board b)           //checks if square's x and y is within bounds
        {
            if(b != null)
            {
                if (b.Row >= 0 && b.Row <= 7 && b.Col >= 0 && b.Col <= 7)
                {
                    return true;
                }
            } 
                return false; 
        }
    }
}
