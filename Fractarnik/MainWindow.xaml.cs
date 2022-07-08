using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractarnik
{

    /// <summary>
    /// Основное окно.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Текущий фрактал.
        FractalBase myFractal;
        // Глубина рекурсии.
        int depth = 1;
        // Число сохраненных изображений.
        int imageCount = 0;

        /// <summary>
        /// Обработка события кнопки фотоаппарата.
        /// </summary>
        /// <param name="sender"> Отправитель-кнопка. </param>
        /// <param name="e"> Событие. </param>
        private void ClickToImage(object sender, RoutedEventArgs e)
        {
            imageCount++;
            toImage(myCanvas);
        }

        /// <summary>
        /// Создание изображения из канваса.
        /// </summary>
        /// <param name="canvas"> Канвас. </param>
        private void toImage(Canvas canvas)
        {
            try
            {
                Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
                RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width,
                    (int)size.Height, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual drawingvisual = new DrawingVisual();
                using (DrawingContext context = drawingvisual.RenderOpen())
                {
                    context.DrawRectangle(new VisualBrush(canvas), null, new Rect(new Point(), size));
                    context.Close();
                }

                result.Render(drawingvisual);
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(result));

                using (Stream s = File.OpenWrite(@"..\..\..\images\" + "PeerMoment" + imageCount + ".png"))
                {
                    png.Save(s);
                }
                MessageBox.Show("Фотография сохранена в каталоге images, рядом с bin и obj");
            }
            catch
            {
                MessageBox.Show("При снятии кадра что-то пошло не так :(");
            }

        }


        /// <summary>
        /// Приближение канваса. Обработка события.
        /// </summary>
        /// <param name="sender"> Кпнопка-отправитель события. </param>
        /// <param name="e"> Событие. </param>
        private void ClickToScale(object sender, RoutedEventArgs e)
        {
            var scaling = 1;
            if (((Button)sender).Name == "myButton2")
                scaling = 2;
            else if (((Button)sender).Name == "myButton3")
                scaling = 3;
            else if (((Button)sender).Name == "myButton4")
                scaling = 4;
            Scaling(myCanvas, scaling);
        }


        /// <summary>
        /// Вовзращение из зум-режима. Обработка события.
        /// </summary>
        /// <param name="sender"> Кнопка-отпрвитель события. </param>
        /// <param name="e"> Событие. </param>
        private void ClickBack(object sender, RoutedEventArgs e)
        {
            Button buttonBack = (Button)sender;
            buttonBack.Opacity = 0;
            GoBack(1, myCanvas, buttonBack);
        }

        /// <summary>
        /// Возвращение из зум-режима.
        /// </summary>
        /// <param name="size"> Кратность зума. </param>
        /// <param name="canvas"> Канвас. </param>
        /// <param name="buttonBack"> Кпопка, вызывающее событие возвращения. </param>
        private void GoBack(double size, Canvas canvas, Button buttonBack)
        {
            ScaleTransform scale = new ScaleTransform(size, size);

            if (myFractal is null)
                myFractal = new FractalZero(1, canvas);

            scale.CenterX = myFractal.xCenter;
            scale.CenterY = myFractal.yCenter;

            canvas.RenderTransform = scale;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            canvas.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetColumn(canvas, 1);
            canvas.Children.Remove(buttonBack);

        }

        /// <summary>
        /// Приближение канваса.
        /// </summary>
        /// <param name="canvas"> Канвас. </param>
        /// <param name="zoom"> Кратность зума. </param>
        private void Scaling(Canvas canvas, int zoom)
        {
            try
            {
                ScaleTransform scale = new ScaleTransform(zoom, zoom);
                if (myFractal is null)
                    myFractal = new FractalZero(1, myCanvas);
                scale.CenterX = myFractal.xCenter;
                scale.CenterY = myFractal.yCenter;
                canvas.RenderTransform = scale;
                Button newButton = new Button();
                TextBlock label = new TextBlock();
                label.FontSize = 8;
                label.Text = "back";
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Stretch;
                label.Height = 10;
                label.Width = 20;
                newButton.Name = "back";
                newButton.Width = 30;
                newButton.Height = 20;
                canvas.Children.Add(newButton);
                StackPanel a = new StackPanel();
                newButton.Content = label;
                newButton.Click += ClickBack;
                Canvas.SetLeft(newButton, myFractal.xCenter);
                Canvas.SetTop(newButton, myFractal.yCenter);
            }
            catch (Exception trouble)
            {
                MessageBox.Show(trouble.Message);
            }
        }


        /// <summary>
        /// Возвращает нужный угол в зависимости от выбора.
        /// </summary>
        /// <param name="selected"> Выбор пользователя. </param>
        /// <returns> Угол. </returns>
        private int FirstAngleSetter(string selected)
        {
            int firstAngle = 45;
            if (selected == "Angle1")
                firstAngle = 30;
            else if (selected == "Angle2")
                firstAngle = 45;
            else if (selected == "Angle3")
                firstAngle = 60;
            else if (selected == "Angle4")
                firstAngle = 90;
            return firstAngle;
        }

        /// <summary>
        /// Возвращает нужный угол в зависимости от выбора.
        /// </summary>
        /// <param name="selected"> Выбор пользователя. </param>
        /// <returns> Угол. </returns>
        private int SecondAngleSetter(string selected)
        {
            int firstAngle = 45;
            if (selected == "Angle11")
                firstAngle = 30;
            else if (selected == "Angle22")
                firstAngle = 45;
            else if (selected == "Angle33")
                firstAngle = 60;
            else if (selected == "Angle44")
                firstAngle = 90;
            return firstAngle;
        }

        /// <summary>
        /// Возвращает нужное соотношения для длины в зависимости от выбора.
        /// </summary>
        /// <param name="selected"> Выбор пользователя. </param>
        /// <returns> Соотношение. </returns>
        private double LengthSetter(string selected)
        {
            double length = 0.7;
            if (selected == "First")
                length = 0.5;
            else if (selected == "Second")
                length = 0.7;
            else if (selected == "Third")
                length = 0.9;
            return length;
        }

        /// <summary>
        /// Возвращает нужную длину для длины в зависимости от выбора.
        /// </summary>
        /// <param name="selected"> Выбор пользователя. </param>
        /// <returns> Длина. </returns>
        private int CantorLengthSetter(string selected)
        {
            int length = 20;
            if (selected == "Twenty")
                length = 20;
            else if (selected == "Ten")
                length = 10;
            else if (selected == "Thirty")
                length = 30;
            return length;
        }

        /// <summary>
        /// Создает нужный фрактал в зависимости от выбора.
        /// </summary>
        /// <param name="selection"> Выбор пользователя. </param>
        private void Menu(string selection)
        {
            try
            {
                if (depth <= 0)
                    depth = 1;
                if (selection == "Tree")
                {
                    int firstAngle = 45;
                    int secondAngle = 45;
                    double length = 0.7;
                    ComboBoxItem selectedItem = (ComboBoxItem)myComboBox3.SelectedItem;
                    if (selectedItem is not null)
                        firstAngle = FirstAngleSetter(selectedItem.Name.ToString());
                    selectedItem = (ComboBoxItem)myComboBox4.SelectedItem;
                    if (selectedItem is not null)
                        secondAngle = SecondAngleSetter(selectedItem.Name.ToString());
                    selectedItem = (ComboBoxItem)myComboBox5.SelectedItem;
                    if (selectedItem is not null)
                        length = LengthSetter(selectedItem.Name.ToString());
                    myFractal = new FractalTree(colorPicker, colorPicker2,
                        depth, firstAngle, secondAngle, length, myCanvas);
                }
                else if (selection == "Koch")
                    myFractal = new FractalKoch(colorPicker, colorPicker2, depth, myCanvas);
                else if (selection == "Karpet")
                    myFractal = new FractalKarpet(colorPicker, colorPicker2, depth, myCanvas);
                else if (selection == "Triangle")
                    myFractal = new FractalTriangle(depth, colorPicker, colorPicker2, myCanvas);
                else if (selection == "Cantor")
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)myComboBox2.SelectedItem;
                    int length = 20;
                    if (selectedItem is not null)
                        length = CantorLengthSetter(selectedItem.Name.ToString());
                    myFractal = new FractalCantor(length, colorPicker, colorPicker2, depth, myCanvas);
                }
                else if (selection == "Zero")
                    myFractal = new FractalZero(depth, myCanvas);
            }
            catch (Exception trouble)
            {
                MessageBox.Show(trouble.Message);
            }
        }

        /// <summary>
        /// Изменения в чекбоксах
        /// </summary>
        /// <param name="sender"> Чекбокс отправитель события. </param>
        /// <param name="e"> Событие.</param>
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.Name == "myComboBox")
            {
                if (comboBox.SelectedItem != null)
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                    if (selectedItem.Name != null)
                        Menu(selectedItem.Name.ToString());
                }
            }
            else if (comboBox.Name == "myComboBox2")
            {
                if (comboBox.SelectedItem != null)
                {
                    if (myFractal is null)
                        myFractal = new FractalZero(1, myCanvas);
                    if (myFractal.name == "Cantor")

                        Menu(myFractal.name);
                }
            }
            else if (comboBox.Name == "myComboBox3" || comboBox.Name == "myComboBox4" 
                || comboBox.Name == "myComboBox5")
            {
                if (comboBox.SelectedItem != null)
                {
                    if (myFractal is null)
                        myFractal = new FractalZero(1, myCanvas);
                    if (myFractal.name == "Tree")

                        Menu(myFractal.name);
                }
            }
        }


        /// <summary>
        /// Обработка изменений в текстбоксе.
        /// </summary>
        /// <param name="sender"> Текстбокс отправитель. </param>
        /// <param name="args"> Информация о событие. </param>
        void TextChanged1(object sender, TextChangedEventArgs args)
        {
            TextBox textBox = (TextBox)sender;
            try
            {
                if (textBox.Text != null && textBox.Text != "")
                    if (int.TryParse(textBox.Text, out int digit))
                        if (digit < 1)
                            MessageBox.Show("Слишком маленькое число.");
                        else
                            depth = digit;
                    else
                        MessageBox.Show("Это должно быть натуральное число.");
                else
                    depth = 1;
                if (myFractal is null)
                    myFractal = new FractalZero(1, myCanvas);
                Menu(myFractal.name);
            }
            catch (Exception trouble)
            {
                MessageBox.Show(trouble.Message);
            }
        }

        /// <summary>
        /// Инициализация основного окна.
        /// </summary>
        public MainWindow()
        {

            try
            {
                InitializeComponent();
            }
            catch (Exception trouble)
            {
                MessageBox.Show(trouble.ToString());
            }
        }

    }
}
