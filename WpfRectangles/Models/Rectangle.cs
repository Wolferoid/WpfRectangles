using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfRectangles.Models
{
    public class Rectangle
    {  
        public Color Color { get; set; }
        public Point BotLeft { get; set; }
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }
        public Point BotRight { get; set; }

        public double Left { get { return BotLeft.X; } }
        public double Right { get { return BotRight.X; } }
        public double Top { get { return TopLeft.Y; } }
        public double Bottom { get { return BotLeft.Y; } }

        public Rectangle()
        {
            Color = Colors.Transparent;
            TopLeft = new Point(0,0);
            TopRight = new Point(0, 0);
            BotLeft = new Point(0, 0);
            BotRight = new Point(0, 0);
        }
        public Rectangle(Point botLeft, Point topLeft, Point topRight, Point botRight)
        {
            Color = Colors.Transparent;
            TopLeft = topLeft;
            TopRight = topRight;
            BotLeft = botLeft;
            BotRight = botRight;
        }
        public Rectangle(Color color, Point botLeft, Point topLeft, Point topRight, Point botRight)
        {
            Color = color;
            TopLeft = topLeft;
            TopRight = topRight;
            BotLeft = botLeft;
            BotRight = botRight;
        }

        public override string ToString()
        {
            return string.Format("Rectangle: BotLeft{0}, TopLeft{1}, TopRight{2}, BotRight{3}; Color: {4}",
                BotLeft.ToString(), TopLeft.ToString(), TopRight.ToString(), BotRight.ToString(), Color.ToString());
        }
    }
}
