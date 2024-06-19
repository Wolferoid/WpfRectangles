using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Media;
using WpfRectangles.Models;
using WpfRectangles.Logging;

namespace WpfRectangles.Processing
{
    public class DoWork
    {
        // для удобства вместо массива список
        public List<Rectangle> Rectangles { get; set; }
        //public Rectangle[] Rectangles { get; set; }
        public Rectangle MainRectangle { get; set; }
        private Log _log;

        public DoWork(Log log)
        {
            _log = log;
            // второстепенные прямоугольники
            Rectangles = new List<Rectangle>();
            Rectangles.Add(new Rectangle(Colors.Green, new Point(1, 1), new Point(1, 2), new Point(3, 2), new Point(3, 1)));
            Rectangles.Add(new Rectangle(Colors.Green, new Point(12, 3), new Point(12, 6), new Point(23, 6), new Point(23, 3)));
            Rectangles.Add(new Rectangle(Colors.Green, new Point(18, 0), new Point(18, 8), new Point(26, 8), new Point(26, 0)));
            Rectangles.Add(new Rectangle(Colors.Green, new Point(5, 2), new Point(5, 7), new Point(10, 7), new Point(10, 2)));
            Rectangles.Add(new Rectangle(Colors.Green, new Point(30, 6), new Point(30, 11), new Point(40, 11), new Point(40, 6)));
            // главный прямоугольник
            MainRectangle = new Rectangle(Colors.Transparent, new Point(0, 0), new Point(0, 40), new Point(35, 40), new Point(35, 0));

            LogInfoAboutRectangles();
        }

        public DoWork(Log log, List<Rectangle> rectangles, Rectangle mainRectagle)
        {
            _log = log;
            Rectangles = rectangles;
            MainRectangle = mainRectagle;

            LogInfoAboutRectangles();
        }

        // выделить из массива повпавшие полностью в главный прямоугольник
        private List<Rectangle> GetRectanglesByOuter(List<Rectangle> rectangles)
        {
            if (rectangles.Count > 0)
            {
                var resList = new List<Rectangle>();
                foreach (Rectangle rectangle in rectangles)
                {
                    if (MainRectangle.Left < rectangle.Left && MainRectangle.Bottom < rectangle.Bottom
                        && MainRectangle.Top > rectangle.Top && MainRectangle.Right > rectangle.Right)
                    {
                        resList.Add(rectangle);
                    }
                }

                return resList;
            }
            else { return new List<Rectangle>(); }
        }

        // выделить из массива повпавшие внутренними углами в главный прямоугольник
        private List<Rectangle> GetRectanglesByInnerCorner(List<Rectangle> rectangles)
        {
            if (rectangles.Count > 0)
            {
                var resList = new List<Rectangle>();
                foreach (Rectangle rectangle in rectangles)
                {
                    if (MainRectangle.Left < rectangle.Right && MainRectangle.Bottom < rectangle.Top
                        || MainRectangle.Left < rectangle.Right && MainRectangle.Top > rectangle.Bottom
                        || MainRectangle.Right > rectangle.Left && MainRectangle.Top > rectangle.Bottom
                        || MainRectangle.Right > rectangle.Left && MainRectangle.Bottom < rectangle.Top
                        )
                    {
                        resList.Add(rectangle);
                    }
                }

                return resList;
            }
            else { return new List<Rectangle>(); }
        }

        // выделить из массива повпавшие внутренними точками в главный прямоугольник
        private List<Rectangle> GetRectanglesByMain(List<Rectangle> rectangles)
        {
            if (rectangles.Count > 0)
            {
                var resList = new List<Rectangle>();
                foreach (Rectangle rectangle in rectangles)
                {
                    if (MainRectangle.Left < rectangle.Right || MainRectangle.Bottom < rectangle.Top
                        || MainRectangle.Right < rectangle.Left || MainRectangle.Top > rectangle.Bottom)
                    {
                        resList.Add(rectangle);
                    }
                }

                return resList;
            }
            else { return new List<Rectangle>(); }
        }

        // получить прямоугольник очерчения по внешним координатам масива
        private Rectangle GetOutlineRectangle(List<Rectangle> rectangles)
        {
            if (rectangles.Count > 0)
            {
                double maxLeft = rectangles.First().Left;
                double maxTop = rectangles.First().Top;
                double maxRight = rectangles.First().Right;
                double maxBottom = rectangles.First().Bottom;

                foreach (Rectangle rectangle in rectangles)
                {
                    if (maxLeft > rectangle.Left)
                        maxLeft = rectangle.Left;

                    if (maxBottom > rectangle.Bottom)
                        maxBottom = rectangle.Bottom;

                    if (maxRight < rectangle.Right)
                        maxRight = rectangle.Right;

                    if (maxTop < rectangle.Top)
                        maxTop = rectangle.Top;
                }

                return new Rectangle(new Point(maxLeft, maxBottom), new Point(maxLeft, maxTop), new Point(maxRight, maxTop), new Point(maxRight, maxBottom));
            }
            else { return new Rectangle(); }
        }

        // получить прямоугольник очерчения с ограничениме главного
        private Rectangle GetOutlineRectangleByMain(List<Rectangle> rectangles)
        {
            if (rectangles.Count > 0)
            {
                double maxLeft = MainRectangle.Left;
                double maxTop = MainRectangle.Top;
                double maxRight = MainRectangle.Right;
                double maxBottom = MainRectangle.Bottom;

                foreach (Rectangle rectangle in rectangles)
                {
                    if (rectangle.Left > MainRectangle.Left && maxLeft > rectangle.Left)
                        maxLeft = rectangle.Left;

                    if (rectangle.Bottom > MainRectangle.Bottom && maxBottom > rectangle.Bottom)
                        maxBottom = rectangle.Bottom;

                    if (rectangle.Right < MainRectangle.Right && maxRight < rectangle.Right)
                        maxRight = rectangle.Right;

                    if (rectangle.Top < MainRectangle.Top && maxTop < rectangle.Top)
                        maxTop = rectangle.Top;
                }

                return new Rectangle(new Point(maxLeft, maxBottom), new Point(maxLeft, maxTop), new Point(maxRight, maxTop), new Point(maxRight, maxBottom));
            }
            else { return new Rectangle(); }
        }

        // выделить из массива по цвету
        private List<Rectangle> GetRectanglesByColor(List<Rectangle> rectangles, Color color)
        {
            if (rectangles.Count > 0)
            {
                var resList = new List<Rectangle>();
                foreach (Rectangle rectangle in rectangles)
                {
                    if (rectangle.Color == color)
                    { resList.Add(rectangle); }
                }
                return resList;
            }
            else { return new List<Rectangle>(); }
        }

        // выделить из массива исключив цвет
        private List<Rectangle> GetRectanglesByNotColor(List<Rectangle> rectangles, Color color)
        {
            if (rectangles.Count > 0)
            {
                var resList = new List<Rectangle>();
                foreach (Rectangle rectangle in rectangles)
                {
                    if (rectangle.Color != color)
                    { resList.Add(rectangle); }
                }
                return resList;
            }
            else { return new List<Rectangle>(); }
        }

        // прямоуголник очерчения попавших внутрь главного целиком
        public Rectangle OutlineInsideRectangles()
        {
            var resList = GetRectanglesByOuter(Rectangles);
            return GetOutlineRectangle(resList);
        }

        // прямоуголник очерчения попавших внутрь главного целиком c цветом
        public Rectangle OutlineInsideRectangles(Color color, bool ignore = false)
        {
            var resList = GetRectanglesByOuter(ignore ? GetRectanglesByNotColor(Rectangles, color) : GetRectanglesByColor(Rectangles, color));
            return GetOutlineRectangle(resList);
        }

        // прямоуголник очерчения попавших внутрь главного внутренними углами (по отношению к главному)
        public Rectangle OutlineStretchInsideRectangles()
        {
            var resList = GetRectanglesByInnerCorner(Rectangles);
            return GetOutlineRectangle(resList);
        }

        // прямоуголник очерчения попавших внутрь главного внутренними углами (по отношению к главному) с цветом
        public Rectangle OutlineStretchInsideRectangles(Color color, bool ignore = false)
        {
            var resList = GetRectanglesByInnerCorner(ignore ? GetRectanglesByNotColor(Rectangles, color) : GetRectanglesByColor(Rectangles, color));
            return GetOutlineRectangle(resList);
        }

        // прямоуголник очерчения попавших внутрь главного без учета внешних координат второстепенных
        public Rectangle OutlineByMain()
        {
            var resList = GetRectanglesByMain(Rectangles);
            return GetOutlineRectangleByMain(resList);
        }

        // прямоуголник очерчения попавших внутрь главного без учета внешних координат второстепенных с цветом
        public Rectangle OutlineByMain(Color color, bool ignore = false)
        {
            var resList = GetRectanglesByMain(ignore ? GetRectanglesByNotColor(Rectangles, color) : GetRectanglesByColor(Rectangles, color));
            return GetOutlineRectangleByMain(resList);
        }

        private void LogInfoAboutRectangles()
        {
            _log.Info("Sub rectangles");

            foreach (var rectangle in Rectangles)
            {
                _log.Info(rectangle.ToString());
            }

            _log.Info("Main rectangle");
            _log.Info(MainRectangle.ToString());
        }
    }
}
