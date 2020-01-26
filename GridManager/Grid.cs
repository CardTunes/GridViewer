using System;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace GridManager
{

    public struct Vertex
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public struct Triangle
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int X3 { get; set; }
        public int Y3 { get; set; }
    }

    public struct Position
    {
        public char Row { get; set; }
        public int Column { get; set; }
    }

    public class Grid
    {
        private const int _rows = 12;
        private const int _columns = 12;
        private const int _adjustment = 65; // Adjustment for ascii code of character starting from 'A'
        private const int _side = 10;

        public Grid()
        {
        }

        private int GetRowNum(char c)
        {
            var asc = Convert.ToInt32(c);
            // only allow valid range of characters. Could also allow for lower case but not specified so far.
            if (asc < _adjustment || asc > _adjustment + _rows - 1)
            {
                throw new ArgumentException("Invalid row value.");
            }
            return asc - _adjustment;
        }

        public Triangle GetTriangle(char row, int column)
        {
            // TODO check bounds and valid char
            var rowNum = GetRowNum(row);
            if (column < 1 || column > _columns)
            {
                throw new ArgumentException("Invalid column value.");
            }


            // Return upper or lower triangle depending on odd or even column
            if (column % 2 == 0)
            {
               return GetUpper(rowNum, column);
            }
            else
            {
               return GetLower(rowNum, column);
            }
        }

        private Triangle GetUpper(int row, int column)
        {
            // Column passed needs index adjusted to 0 base.
            int col = column - 1;
            int x1 = _side * ((col / 2) + 1);
            int y1 = _side * row;

            int x2 = _side * ((col / 2));
            int y2 = _side * row;

            int x3 = _side * ((col / 2) + 1);
            int y3 = _side * (row + 1);

            return new Triangle
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                X3 = x3,
                Y3 = y3
            };
        }

        private Triangle GetLower(int row, int column)
        {
            // Column passed needs index adjusted to 0 base.
            int col = column - 1;

            int x1 = _side * ((col / 2));
            int y1 = _side * (row + 1);

            int x2 = _side * ((col / 2));
            int y2 = _side * row;

            int x3 = _side * ((col / 2) + 1);
            int y3 = _side * (row + 1);

            return new Triangle
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                X3 = x3,
                Y3 = y3
            };


        }
        private bool IsValidColumn(int value)
        {
            return value >= 0 && value <= _columns * _side;
        }

        public Position GetPosition(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (!IsValidColumn(x1) || !IsValidColumn(y1) || !IsValidColumn(x2) || !IsValidColumn(y2)
                || !IsValidColumn(x3) || !IsValidColumn(y3))
            {
                throw new ArgumentException(string.Format("Columns must be between 1 and {0}.", _columns));
            }

            var triangle = new Vertex[]
            {
                new Vertex { X = x1, Y = y1 },
                new Vertex { X = x2, Y = y2 },
                new Vertex { X = x3, Y = y3 }
            };
         
            if (triangle.Length != 3)
            {
                throw new ArgumentException("Must have 3 vertices suppled.");
            }

            // Not assuming any vertex order here. If it was known could be vastly simplified.
            OrderVertexes(triangle, out Vertex v1, out Vertex v2, out Vertex v3);
            return GetPosition(v1, v2, v3);
        }

        private void OrderVertexes(Vertex[] triangle, out Vertex v1, out Vertex v2, out Vertex v3)
        {
            if (triangle[0].X == triangle[1].X)
            {
                // 1st 2 are a side ege.
                v3 = triangle[2];
                if (triangle[2].Y == triangle[0].Y)
                {
                    v1 = triangle[0];
                    v2 = triangle[1];
                }
                else
                {
                    v1 = triangle[1];
                    v2 = triangle[0];
                }
            }
            else if (triangle[0].Y == triangle[1].Y)
            {
                // 1st 2 are top or bottom edge.
                v2 = triangle[2];
                if (triangle[2].X == triangle[0].X)
                {
                    v1 = triangle[0];
                    v3 = triangle[1];
                }
                else
                {
                    v1 = triangle[1];
                    v3 = triangle[0];
                }
            }
            else
            {
                v1 = triangle[2];
                if (triangle[0].X == triangle[2].X)
                {
                    v2 = triangle[0];
                    v3 = triangle[1];
                }
                else
                {
                    v2 = triangle[1];
                    v3 = triangle[0];
                }
            }

        }
        private Position GetPosition(Vertex v1, Vertex v2, Vertex v3)
        {

            // Vertexes must already be in correct order for this to work.
            int row = 0;
            int col = 0;
            if (v3.X > v1.X)
            {
                // r2 is top left corner
                row = v2.Y / _side;
                col = 2 * (v1.X / _side);
            }
            else
            {
                // r2 is bottom right corner
                row = v3.Y / _side;
                // adjust col by 1 for top triangle
                col = (2 * (v1.X / _side)) - 1;
            }
            // We can get row letter here based on row and ascii code.
            var letter = (char)(row + _adjustment);

            // Adjust column for 1 based index.           
            return new Position { Row = letter, Column = col + 1 };
        }

    }
}
