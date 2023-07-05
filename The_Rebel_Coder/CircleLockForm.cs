using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    public partial class CircleLockForm : GeForm {
        GraphicsPath Ellipse;//Эта фигура нужна для корректного определения, попадал курсор игрока в область круга или нет.
        private static Random r = Presets.r;//Используем рандом из пресетов
        public CircleLockForm() {
            InitializeComponent();
        }
        private const double piece = Math.PI * 0.25 / 4;//Определяем, сколько площади заполняют оба типа заполнения (внутри круга и вне)
        private const double outpiece = 0.25-piece;
        double must;//Сколько площади должно выйти
        bool[,] selected;//Массив выбранных игроком элементов

        public override void onOpen() {//Всё сбрасывается при открытии формы
            selected = new bool[2, 8];
            must = r.Next(5) * piece + (r.Next(4) + 1) * outpiece;
            CircleLockForm_SizeChanged(null, null);//Нужно для обновления выводящих элементов
        }
        private void panel1_Paint(object sender, PaintEventArgs e) {//Рисуем наш круг в этой панели
            Graphics g = e.Graphics;
            Panel p = (Panel)sender;
            g.DrawRectangle(Presets.blackPen3,0,0,p.Width-1,p.Height-1);
            g.DrawEllipse(Presets.blackPen3, 0, 0, p.Width - 1, p.Height - 1);
            double result = 0;
            for (int j = 0; j < selected.GetLength(0); j++) {
                for (int i = 0; i < selected.GetLength(1); i++) {
                    if (selected[j, i]) {
                        using (GraphicsPath gp = new GraphicsPath()) {
                            int absIndex = j==0?i/2:3-i/2; //Абсолютный номер сектора (1-4) по часовой стрелке.
                            bool inverse = i == 0 || i == 3;
                            gp.AddArc(0, 0, p.Width - 1, p.Height - 1, 180 + absIndex * 90, 90);

                            if (inverse) {//Закрашиваем внешние части (вне круга)
                                switch (absIndex) {//Здесь есть закономерность, но мне лень её вычислять.
                                    case 0: gp.AddLine(p.Width / 2, 0, 0, 0); break;
                                    case 1: gp.AddLine(p.Width, 0, p.Width / 2, 0); break;
                                    case 2: gp.AddLine(p.Width / 2, p.Height, p.Width, p.Height); break;
                                    case 3: gp.AddLine(0, p.Height, p.Width / 2, p.Height); break;
                                }
                                result += outpiece;
                            } else {//Закрашиваем внутренние части
                                switch (absIndex) {
                                    case 0: gp.AddLine(p.Width / 2, p.Height / 2, 0, p.Height / 2); break;
                                    case 1: gp.AddLine(0, p.Height / 2, p.Width / 2, p.Height / 2); break;
                                    case 2: gp.AddLine(p.Width / 2, p.Height / 2, p.Width, p.Height / 2); break;
                                    case 3: gp.AddLine(p.Width, p.Height / 2, p.Width / 2, p.Height / 2); break;
                                }
                                result += piece;
                            }
                            gp.CloseFigure();

                            e.Graphics.FillPath(Presets.blackBrush, gp);
                        }
                    }
                }
            }
            //Обновляем данные внутри всех выводящих элементов, а не только круга.
            label1.Text = "S = "+ (100 * result).ToString("0.0")+"% / "+ (100 * must).ToString("0.0") + "%";
            label1.Location = new Point(panel1.Location.X + panel1.Width / 2 - label1.Width / 2, panel1.Location.Y - label1.Height);
            if (result == must) {//И если вдруг подобрали правильное комбо, побеждаем.
                Program.videoChangeForm(2);
            }
        }

        private void panel1_Click(object sender, EventArgs e) {//Ловим нажатие по круговой панели и перепросчитываем всё.
            Point p = panel1.PointToClient(MousePosition);
            int x = p.X / (panel1.Width/2+1);//Находим абсолютные координаты мыши (4 квадрата)
            int y = p.Y / (panel1.Height / 2+1);
            if (Ellipse.IsVisible(p)){//Определяем, в какой именно части угла был клик
                if (x == 0) x = 1;
                else if (x == 1) x = 2;
            } else {
                if (x == 1) x = 3;
            }
            selected[y, x] = !selected[y,x];
            panel1.Invalidate();
            
        }

        private void CircleLockForm_Load(object sender, EventArgs e) {

        }

        private void CircleLockForm_SizeChanged(object sender, EventArgs e) {//Простой пересчёт расположения элементов.
            int size = (int)(Math.Min(Width, Height) * 0.5);
            panel1.Location=new Point(Width/2-size/2,Height/2-size/2);
            panel1.Width = size;
            panel1.Height = size;
            Ellipse = new GraphicsPath();
            Ellipse.AddEllipse(0, 0, panel1.Width - 1, panel1.Height - 1);
            panel1.Invalidate();//Перерисовывает компонент.
        }
    }
}
