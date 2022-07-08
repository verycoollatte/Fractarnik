using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace Fractarnik
{
    /// <summary>
    /// Базовый класс фрактала.
    /// </summary>
    public abstract class FractalBase
    {
        // Эпицентр действия по иксу.
        public double xCenter;
        // Эпицентр действия по игреку.
        public double yCenter;
        /// Имя фрактала.
        public string name;
        // Максимальная рекурсия доступна.
        public int maxRecursion;
        // Градиент для покраса.
        public List<Color> colors;
        // Канвас для рисования.
        public Canvas canvas;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalBase(int depth, Canvas canvas)
        {
            try
            {
                this.canvas = canvas;
                canvas.Children.Clear();
            }
            catch
            {
                MessageBox.Show("ебоже это фича а не баг");
            }
        }

        /// <summary>
        /// Проверка подходит ли глубина рекурсии.
        /// </summary>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <returns> Доступная рекурсия. Данная или данная уменьшенная до доступной. </returns>
        public int iSDepthOk(int depth)
        {

            if (depth > maxRecursion)
            {
                MessageBox.Show("Максимальная глубина рекурсии для этого фрактала " + maxRecursion);
                depth = maxRecursion;
            }
            return depth;
        }

        /// <summary>
        /// Создание градиента для раскраски фрактала.
        /// </summary>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="colorPicker"> Инструмент ввода первого цвета. </param>
        /// <param name="colorPicker2"> Инструмент ввода второго цвета. </param>
        /// <returns> Набор цветов. </returns>
        public List<Color> CreatePalette(int depth, ColorPicker colorPicker, ColorPicker colorPicker2)
        {
            string selection1 = colorPicker.SelectedColor.ToString();
            string selection2 = colorPicker2.SelectedColor.ToString();
            Color colorStart = (Color)ColorConverter.ConvertFromString(selection1);
            Color colorFinish = (Color)ColorConverter.ConvertFromString(selection2);
            List<Color> palette = new List<Color>();
            palette.Add(colorStart);
            int r = colorFinish.R - colorStart.R;
            int g = colorFinish.G - colorStart.G;
            int b = colorFinish.B - colorStart.B;
            int divider = depth - 1;

            for (var i = 1; i < depth - 1; i++)
            {

                byte m = Convert.ToByte(i);
                int newR = colorStart.R + r / divider * m;
                int newG = colorStart.G + g / divider * m;
                int newB = colorStart.B + b / divider * m;

                Color newColor = new Color();
                newColor.R = Convert.ToByte(newR);
                newColor.G = Convert.ToByte(newG); 
                newColor.B = Convert.ToByte(newB);
                newColor.A = Convert.ToByte(255);
                palette.Add(newColor);

            }
            palette.Add(colorFinish);
            return palette;
        }
        public abstract void Draw1(double x, double y, double length, int depth, int angle);
    }

    /// <summary>
    /// Невидимый фрактал.
    /// </summary>
    public class FractalZero : FractalBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalZero(int depth, Canvas canvas) : base(depth, canvas)
        {
            try
            {
                name = "Zero";
                maxRecursion = 10;

                xCenter = canvas.ActualWidth / 2;
                yCenter = canvas.ActualHeight / 2;
                depth = iSDepthOk(depth);

                Draw1(0, 0, 0, 0, 0);
            }
            catch
            {
                MessageBox.Show("черт побери этого не может быть!");
            }
        }

        /// <summary>
        /// Отсривока пустого фрактала. 
        /// </summary>
        /// <param name="x"> Координата Х. </param>
        /// <param name="y"> Координата У. </param>
        /// <param name="length"> Длина. </param>
        /// <param name="depth">Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle)
        {
            canvas.Children.Clear();
        }
    }

    /// <summary>
    /// Фрактал дерево.
    /// </summary>
    public class FractalTree : FractalBase
    {

        // Первый угол.
        int angleBasic1;
        // Второй угол.
        int angleBasic2;
        // Соотношение длин отрезков. 
        double lengthMultiplication;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="colorpicker1"> Ввод цвета. </param>
        /// <param name="colorpicker2"> Ввод цвета2. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="angleFirst"> Первый угол. </param>
        /// <param name="angleSecond"> Второй угол. </param>
        /// <param name="length"> Соотношение длин отрезков. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalTree(ColorPicker colorpicker1, ColorPicker colorpicker2, int depth, 
            int angleFirst, int angleSecond, double length, Canvas canvas) : base(depth, canvas)
        {
            name = "Tree";
            maxRecursion = 12;
            depth = iSDepthOk(depth);

            angleBasic1 = angleFirst;
            angleBasic2 = angleSecond;
            lengthMultiplication = length;
            canvas.Children.Clear();
            xCenter = canvas.ActualWidth / 2;
            xCenter -= 20;
            yCenter = canvas.ActualHeight / 2 - 30;
            try
            {
                colors = CreatePalette(depth, colorpicker1, colorpicker2);
                Draw1(canvas.ActualWidth / 2, canvas.ActualHeight / 2 + 130, 130, depth, 0);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString());
            }
        }

        /// <summary>
        /// Отрисовка фрактала.
        /// </summary>
        /// <param name="x"> Координата Х. </param>
        /// <param name="y"> Координата У. </param>
        /// <param name="length"> Соотношение длин отрезков. </param>
        /// <param name="depth"> Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle)
        {
            if (depth > 0)
            {

                if (angle > 360) angle -= 360;
                Line line = new Line();
                SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);
                line.Stroke = myBrush;
                line.StrokeThickness = 1;
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x;
                line.Y2 = y - length;

                RotateTransform rotation = new RotateTransform(angle);
                rotation.CenterX = line.X1;
                rotation.CenterY = line.Y1;
                Point p1 = rotation.Transform(new Point(line.X1, line.Y1));
                Point p2 = rotation.Transform(new Point(line.X2, line.Y2));
                line.X1 = p1.X;
                line.Y1 = p1.Y;
                line.X2 = p2.X;
                line.Y2 = p2.Y;
                canvas.Children.Add(line);
                Draw1(line.X2, line.Y2, length * lengthMultiplication, depth - 1, angle + angleBasic1);
                Draw1(line.X2, line.Y2, length * lengthMultiplication, depth - 1, angle - angleBasic2);

            }
        }
    }

    /// <summary>
    /// Кривая Коха.
    /// </summary>
    public class FractalKoch : FractalBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="colorpicker1"> Ввод цвета. </param>
        /// <param name="colorpicker2"> Ввод цвета2. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalKoch(ColorPicker colorpicker1, ColorPicker colorpicker2, 
            int depth, Canvas canvas) : base(depth, canvas)
        {
            name = "Koch";
            maxRecursion = 5;
            Line line = new Line();
            colors = CreatePalette(depth, colorpicker1, colorpicker2);
            SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);
            line.Stroke = myBrush;
            line.StrokeThickness = 1;
            double start = canvas.ActualWidth;
            double start2 = canvas.ActualHeight;
            xCenter = start / 2;
            yCenter = start2 / 2 - 20;
            depth = iSDepthOk(depth);
            try
            {
                line.X1 = start / 2 - 280;
                line.Y1 = start2 / 2 + 50;
                line.X2 = start / 2 + 280;
                line.Y2 = start2 / 2 + 50;

                canvas.Children.Add(line);

            }
            catch (Exception trouble)
            {
                MessageBox.Show(trouble.Message);
            }
            Draw1(0, 0, 0, depth, 0);
        }

        /// <summary>
        /// Создание линий первого типа (вторая треть).
        /// </summary>
        /// <param name="line"> Создаваемая. </param>
        /// <param name="mainLine"> Линия, с которой мы перенимаем координаьы. </param>
        /// <param name="myBrush"> Покрас. </param>
        private void LineCreate(Line line, Line mainLine, SolidColorBrush myBrush)
        {
            line.X1 = mainLine.X1 + (mainLine.X2 - mainLine.X1) / 3;
            line.X2 = mainLine.X1 + (mainLine.X2 - mainLine.X1) * 2 / 3;
            line.Y1 = mainLine.Y1 + (mainLine.Y2 - mainLine.Y1) / 3;
            line.Y2 = mainLine.Y1 + (mainLine.Y2 - mainLine.Y1) * 2 / 3;
            line.Stroke = myBrush;
        }

        /// <summary>
        /// Создание линий второго типа (первая треть).
        /// </summary>
        /// <param name="line3"> Создаваемая лниия. </param>
        /// <param name="mainLine"> Линия-источник координат. </param>
        /// <param name="myBrush"> Покрас. </param>
        private void LineCreate2(Line line3, Line mainLine, SolidColorBrush myBrush)
        {
            line3.X1 = mainLine.X1;
            line3.X2 = mainLine.X1 + (mainLine.X2 - mainLine.X1) / 3;
            line3.Y1 = mainLine.Y1;
            line3.Y2 = mainLine.Y1 + (mainLine.Y2 - mainLine.Y1) / 3;
            line3.Stroke = mainLine.Stroke;
        }

        /// <summary>
        /// Создание линий третьего типа (третья треть).
        /// </summary>
        /// <param name="line4"> Создаваемая линия. </param>
        /// <param name="mainLine"> Линия-источник координат. </param>
        /// <param name="myBrush"> Покрас. </param>
        private void LineCreate3(Line line4, Line mainLine, SolidColorBrush myBrush)
        {
            line4.X1 = mainLine.X1 + (mainLine.X2 - mainLine.X1) * 2 / 3; ;
            line4.X2 = mainLine.X2;
            line4.Y1 = mainLine.Y1 + (mainLine.Y2 - mainLine.Y1) * 2 / 3; ;
            line4.Y2 = mainLine.Y2;
            line4.Stroke = mainLine.Stroke;
        }
  
        /// <summary>
        /// Отрисовка фрактала.
        /// </summary>
        /// <param name="x"> Координаты Х. </param>
        /// <param name="y"> Координаты У. </param>
        /// <param name="length"> Длина. </param>
        /// <param name="depth"> Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle)
        {
            if (depth > 0)
            {
                try
                {
                    List<Line> lines = new List<Line>();
                    List<Line> linesToRemove = new List<Line>();
                    foreach (UIElement shape in canvas.Children)
                    {
                        if (shape is Line)
                        {
                            Line mainLine = (Line)shape;
                            Line line = new Line();
                            SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);
                            LineCreate(line, mainLine, myBrush);
                            
                            RotateTransform rotation = new RotateTransform(-60);
                            rotation.CenterX = line.X1;
                            rotation.CenterY = line.Y1;
                            Point p1 = rotation.Transform(new Point(line.X1, line.Y1));
                            Point p2 = rotation.Transform(new Point(line.X2, line.Y2));
                            line.X1 = p1.X;
                            line.Y1 = p1.Y;
                            line.X2 = p2.X;
                            line.Y2 = p2.Y;
                            lines.Add(line);

                            Line line2 = new Line();
                            LineCreate(line2, mainLine, myBrush);
                            RotateTransform rotation2 = new RotateTransform(60);
                            rotation2.CenterX = line2.X2;
                            rotation2.CenterY = line2.Y2;
                            Point p3 = rotation2.Transform(new Point(line2.X1, line2.Y1));
                            Point p4 = rotation2.Transform(new Point(line2.X2, line2.Y2));
                            line2.X1 = p3.X;
                            line2.Y1 = p3.Y;
                            line2.X2 = p4.X;
                            line2.Y2 = p4.Y;
                            lines.Add(line2);
                            Line line3 = new Line();
                            LineCreate2(line3, mainLine, myBrush);
                            Line line4 = new Line();
                            LineCreate3(line4, mainLine, myBrush);
                            lines.Add(line3);
                            lines.Add(line4);
                            linesToRemove.Add(mainLine);
                        }
                    }
                    foreach (Line element in lines)
                        canvas.Children.Add(element);
                    foreach (Line element in linesToRemove)
                        canvas.Children.Remove(element);
                    Draw1(0, 0, 0, depth - 1, 0);
                }
                catch (Exception trouble)
                {
                    MessageBox.Show(trouble.Message);
                }
            }
        }
    }

    /// <summary>
    /// Ковер Серпинскиого.
    /// </summary>
    public class FractalKarpet : FractalBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="colorpicker1"> Ввод цвета. </param>
        /// <param name="colorpicker2"> Ввод цвета2. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalKarpet(ColorPicker colorpicker1, ColorPicker colorpicker2, 
            int depth, Canvas canvas) : base(depth, canvas)
        {
            name = "Karpet";
            maxRecursion = 4;
            depth = iSDepthOk(depth);

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 240;
            rectangle.Height = 240;

            colors = CreatePalette(depth, colorpicker1, colorpicker2);
            rectangle.Fill = Brushes.White;
            rectangle.Stroke = Brushes.White;

            xCenter = canvas.ActualWidth / 2;
            yCenter = canvas.ActualHeight / 2;

            Canvas.SetLeft(rectangle, canvas.ActualWidth / 2 - rectangle.Width / 2);
            Canvas.SetTop(rectangle, canvas.ActualHeight / 2 - rectangle.Height / 2);

            canvas.Children.Add(rectangle);
            Draw1(0, 0, 0, depth, 0);
        }

        /// <summary>
        /// Отрисовка фрактала.
        /// </summary>
        /// <param name="x"> Координаты Х. </param>
        /// <param name="y"> Координаты У. </param>
        /// <param name="length"> Длина. </param>
        /// <param name="depth"> Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle)
        {
            if (depth > 0)
            {
                try
                {
                    List<Rectangle> rectangles = new List<Rectangle>();
                    List<Rectangle> rectanglesToRemove = new List<Rectangle>();
                    foreach (UIElement shape in canvas.Children)
                    {
                        if (shape is Rectangle)
                        {
                            Rectangle rectangle = (Rectangle)shape;
                            if (rectangle.Fill == Brushes.White)
                            {
                                for (var i = 0; i < 3; i++)
                                {
                                    for (var j = 0; j < 3; j++)
                                    {
                                        Rectangle newRectangle = new Rectangle();
                                        if (i == 1 && j == 1)
                                        {
                                            SolidColorBrush myBrush = 
                                                new SolidColorBrush(colors[colors.Count - depth]);
                                            newRectangle.Fill = myBrush;
                                            newRectangle.Stroke = myBrush;
                                        }
                                        else
                                            newRectangle.Fill = Brushes.White;
                                        newRectangle.Width = rectangle.Height / 3;
                                        newRectangle.Height = rectangle.Width / 3;
                                        Canvas.SetLeft(newRectangle, Canvas.GetLeft(rectangle) +
                                            i * rectangle.Width / 3);
                                        Canvas.SetTop(newRectangle, Canvas.GetTop(rectangle) +
                                            j * rectangle.Height / 3);

                                        rectangles.Add(newRectangle);
                                    }
                                }
                                rectanglesToRemove.Add(rectangle);
                            }
                        }
                    }
                    foreach (var element in rectangles)
                        canvas.Children.Add(element);
                    foreach (var element in rectanglesToRemove)
                        canvas.Children.Remove(element);
                    Draw1(0, 0, 0, depth - 1, 0);
                }
                catch (Exception trouble)
                {
                    MessageBox.Show(trouble.Message);
                }
            }
        }

    }

    /// <summary>
    /// Треугольник Серпинского. 
    /// </summary>
    public class FractalTriangle : FractalBase
    {
        // База.
        Polygon myPolygon;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="colorpicker1"> Ввод цвета. </param>
        /// <param name="colorpicker2"> Ввод цвета2. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalTriangle(int depth, ColorPicker colorpicker1, 
            ColorPicker colorpicker2, Canvas canvas) : base(depth, canvas)
        {
            name = "Triangle";
            maxRecursion = 6;
            depth = iSDepthOk(depth);

            xCenter = 200;
            yCenter = 250;

            myPolygon = new Polygon();
            Point Point1 = new Point(100, 100);
            Point Point2 = new Point(500, 100);
            Point Point3 = new Point(300, 450);

            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPolygon.Points = myPointCollection;

            colors = CreatePalette(depth, colorpicker1, colorpicker2);
            SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);
            myPolygon.Stroke = myBrush;
            myPolygon.Fill = myBrush;
            myPolygon.StrokeThickness = 1;
            canvas.Children.Add(myPolygon);
            Draw1(0, 0, 0, depth, 0);
          //  Draw1(myPolygon, 0, depth, 0);
        }

        /// <summary>
        /// Соблюдаем ООП.
        /// </summary>
        /// <param name="x"> Координаты Х. </param>
        /// <param name="y"> Координаты У. </param>
        /// <param name="length"> Длина. </param>
        /// <param name="depth"> Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle) {
            Draw1(myPolygon, depth);
        }

        /// <summary>
        /// Работа с треугольниками. 
        /// </summary>
        /// <param name="polygon1"> Треугольник первый. </param>
        /// <param name="polygon2"> Треугольник второй. </param>
        /// <param name="polygon3"> Другой треугольник. </param>
        /// <param name="myBrush"> Покрас. </param>
        /// <param name="canvas"> Канвас. </param>
        private void WorkPolygon(Polygon polygon1, Polygon polygon2, Polygon polygon3, 
            SolidColorBrush myBrush, Canvas canvas)
        {
            canvas.Children.Add(polygon1);
            polygon1.Fill = myBrush;
            canvas.Children.Add(polygon2);
            polygon2.Fill = myBrush;
            canvas.Children.Add(polygon3);
            polygon3.Fill = myBrush;
        }

        /// <summary>
        /// Отрисовка фрактала. 
        /// </summary>
        /// <param name="mainPolygon"> Прошлый результат рекурсии/базовый элемент. </param>
        /// <param name="depth"></param>
        public void Draw1(Polygon mainPolygon, int depth)
        {
            if (depth > 0)
            {
                List<Polygon> polygons = new List<Polygon>();
                Polygon myPolygon = new Polygon();
                PointCollection myPointCollection = new PointCollection();
                Point Point1 = new Point((mainPolygon.Points[0].X + mainPolygon.Points[1].X) / 2,
                    (mainPolygon.Points[0].Y + mainPolygon.Points[1].Y) / 2);
                Point Point2 = new Point((mainPolygon.Points[1].X + mainPolygon.Points[2].X) / 2, 
                    (mainPolygon.Points[1].Y + mainPolygon.Points[2].Y) / 2);
                Point Point3 = new Point(mainPolygon.Points[1].X, mainPolygon.Points[1].Y);
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;
                Polygon myPolygon1 = new Polygon();
                myPointCollection = new PointCollection();
                Point Point4 = new Point((mainPolygon.Points[2].X + mainPolygon.Points[1].X) / 2,
                    (mainPolygon.Points[2].Y + mainPolygon.Points[1].Y) / 2);
                Point Point5 = new Point((mainPolygon.Points[0].X + mainPolygon.Points[2].X) / 2,
                    (mainPolygon.Points[0].Y + mainPolygon.Points[2].Y) / 2);
                Point Point6 = new Point(mainPolygon.Points[2].X, mainPolygon.Points[2].Y);
                myPointCollection.Add(Point4);
                myPointCollection.Add(Point5);
                myPointCollection.Add(Point6);
                myPolygon1.Points = myPointCollection;
                Polygon myPolygon2 = new Polygon();
                myPointCollection = new PointCollection();
                Point Point7 = new Point((mainPolygon.Points[0].X + mainPolygon.Points[1].X) / 2,
                    (mainPolygon.Points[0].Y + mainPolygon.Points[1].Y) / 2);
                Point Point8 = new Point((mainPolygon.Points[0].X + mainPolygon.Points[2].X) / 2,
                    (mainPolygon.Points[0].Y + mainPolygon.Points[2].Y) / 2);
                Point Point9 = new Point(mainPolygon.Points[0].X, mainPolygon.Points[0].Y);
                myPointCollection.Add(Point7);
                myPointCollection.Add(Point8);
                myPointCollection.Add(Point9);
                myPolygon2.Points = myPointCollection;
                SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);

                WorkPolygon(myPolygon1, myPolygon2, myPolygon, myBrush, canvas);

                Draw1(myPolygon, depth - 1);
                Draw1(myPolygon1, depth - 1);
                Draw1(myPolygon2, depth - 1);
            }
        }

    }

    /// <summary>
    /// Фрактал Кантора.
    /// </summary>
    public class FractalCantor : FractalBase
    {
        Rectangle rectangle;
        /// <summary>
        /// Конструктор.
        /// </summary>
        ///  /// <param name="length"> Длина расстояния. </param>
        /// <param name="colorpicker1"> Ввод цвета. </param>
        /// <param name="colorpicker2"> Ввод цвета2. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        /// <param name="canvas"> Канвас. </param>
        public FractalCantor(int length, ColorPicker colorpicker1, ColorPicker colorpicker2,
            int depth, Canvas canvas) : base(depth, canvas)
        {
            name = "Cantor";
            maxRecursion = 5;

            depth = iSDepthOk(depth);

            xCenter = canvas.ActualWidth / 2;
            yCenter = canvas.ActualHeight / 2;

            rectangle = new Rectangle();
            rectangle.Width = 400;
            rectangle.Height = 50;
            Canvas.SetLeft(rectangle, canvas.ActualWidth / 2 - 230);
            Canvas.SetTop(rectangle, canvas.ActualHeight / 2 - 200);
            canvas.Children.Add(rectangle);

            colors = CreatePalette(depth + 1, colorpicker1, colorpicker2);
            SolidColorBrush myBrush = new SolidColorBrush(colors[0]);
            rectangle.Fill = myBrush;
            Draw1(0, 0, length, depth, 0);

        }

        /// <summary>
        /// Соблюдаем ООП.
        /// </summary>
        /// <param name="x"> Координаты Х. </param>
        /// <param name="y"> Координаты У. </param>
        /// <param name="length"> Длина. </param>
        /// <param name="depth"> Глубина. </param>
        /// <param name="angle"> Угол. </param>
        public override void Draw1(double x, double y, double length, int depth, int angle)
        {
            Draw1(rectangle, length, depth);
        }

        /// <summary>
        /// Отрисовка фрактала. 
        /// </summary>
        /// <param name="mainRectangle"> Результат прошлой рекурсии. </param>
        /// <param name="length"> Длина между блоками. </param>
        /// <param name="depth"> Глубина рекурсии. </param>
        public void Draw1(Rectangle mainRectangle, double length, int depth)
        {
            if (depth > 0)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Height = mainRectangle.Height;
                rectangle.Width = mainRectangle.Width / 3;
                SolidColorBrush myBrush = new SolidColorBrush(colors[colors.Count - depth]);
                rectangle.Fill = myBrush;

                Canvas.SetLeft(rectangle, Canvas.GetLeft(mainRectangle));
                Canvas.SetTop(rectangle, Canvas.GetTop(mainRectangle) + mainRectangle.Height + length);

                Rectangle anotherRectangle = new Rectangle();
                anotherRectangle.Height = mainRectangle.Height;
                anotherRectangle.Width = mainRectangle.Width / 3;
                anotherRectangle.Fill = myBrush;

                Canvas.SetLeft(anotherRectangle, Canvas.GetLeft(mainRectangle) + mainRectangle.Width / 3 * 2);
                Canvas.SetTop(anotherRectangle, Canvas.GetTop(mainRectangle) + length + mainRectangle.Height);

                canvas.Children.Add(rectangle);
                canvas.Children.Add(anotherRectangle);
                Draw1(rectangle, length, depth - 1);
                Draw1(anotherRectangle, length, depth - 1);
            }

        }
    }
}

