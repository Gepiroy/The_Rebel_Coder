using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    public partial class Graph : GeForm {
        public Graph() {
            InitializeComponent();
        }

        public override void onOpen() {//Ресетаем всё, когда форма активируется.
            reset();
            recountGraph();
            label1.Select();
        }

        private void reset() {//Сбросить всё туда, где всё было изначально.
            guy.x = 10;
            guy.y = panel1.Height;
            keys.Clear();
            a = 1;
            b = 1;
            Program.sync(textBox1, () => textBox1_TextChanged(null, null));
            Program.sync(textBox2, () => textBox2_TextChanged(null, null));
        }

        private static float scale = 10f;//1 x или y = 10 пикселей.
        float a = 1;//Числа для подставки в формулу.
        float b = 1;

        Guy guy = new Guy();//Игрок.
        float[] graph=null;//Точки графика (пол под игроком)
        Bitmap graphImg;//"Заготовка" графика для рисования
        private void recountGraph() {//Обновляем карту пола под игроком и картинку графика.
            graph = new float[panel1.Width];//Пересоздаём массив, так как размер экрана мог измениться
            graphImg = new Bitmap(panel1.Width,panel1.Height);
            using(Graphics g = Graphics.FromImage(graphImg)){
                graph[0] = pos(0);
                for (int x = 1; x < panel1.Width; x++) {
                    graph[x] = pos(x);
                    g.DrawLine(Presets.blackPen, x - 1, graph[x - 1], x, graph[x]);
                }
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e) {//Рисуем график, игрока и стены.
            Graphics g = e.Graphics;
            g.DrawImage(graphImg,0,0);
            g.DrawEllipse(Presets.blackPen, guy.x-2, guy.y, 5, -5);
            g.FillRectangle(Presets.blackBrush, panel1.Width-2, 0, 2, panel1.Height);
            g.FillRectangle(Presets.yellowBrush, panel1.Width - 2, panel1.Height / 2 - 30, 2, 30);
        }

        public override void onTick(int rate) {//Обновляем состояние игрока каждый тик.
            if (keys.Contains(Keys.D)) guy.x+=guy.speed;
            if (keys.Contains(Keys.A)) guy.x-=guy.speed;
            if (guy.y > panel1.Height) reset();
            guy.update(panel1);
            if (guy.x >= panel1.Width-1) {
                if (guy.y < panel1.Height / 2 && guy.y > panel1.Height / 2 - 30) {
                    Program.sync(this, ()=> { Program.videoChangeForm(5); });//Ура, победа!!!
                }
                guy.x = panel1.Width - 1;
            }
            if (guy.y > graph[(int)guy.x]) guy.y = graph[(int)guy.x];
            panel1.Invalidate();
        }

        private float pos(float x) {//Краткий метод для расчёта Y на позиции X, выровненного по пространству формы.
            float ret = (float)(Math.Log(a - (x - panel1.Width / 2) / scale) + b) * scale;

            if (float.IsInfinity(ret)|| float.IsNaN(ret)) ret = -panel1.Height;
            return panel1.Height - (ret + panel1.Height / 2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {//Ловим ивенты изменения текста и обновляем график по ним
            cht(ref a, textBox1, e==null);
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            cht(ref b, textBox2, e==null);
        }

        private void cht(ref float f, TextBox tb, bool set) {//Обобщённый ивент изменения текста в текстбоксе. 
            float saved = f;
            if (!set && Program.readFloat(tb.Text, ref f)) recountGraph();
            else {
                Program.sync(label1, () => label1.Select());
                Program.sync(tb, () => tb.Text = saved.ToString("0.0"));
            }
        }
        HashSet<Keys> keys = new HashSet<Keys>();//Сохранённые кнопки, что были нажаты (но ещё не отпущены) игроком.
        //hashset защищает от дубликатов, в отличии от List.
        private void Graph_KeyDown(object sender, KeyEventArgs e) {//Фиксируем нажатие кнопки
            keys.Add(e.KeyCode);
            if ((e.KeyCode == Keys.Space || e.KeyCode==Keys.W) && graph[(int)guy.x]-guy.y<=10&&guy.vecY>-5) guy.vecY -= 10;
        }

        private void Graph_KeyUp(object sender, KeyEventArgs e) {//Фиксируем отпускание кнопки
            keys.Remove(e.KeyCode);
        }

        private void Graph_SizeChanged(object sender, EventArgs e) {
            recountGraph();
        }

        private void Graph_Load(object sender, EventArgs e) {
            label4.Parent = panel1;//Ставим панель родителем лэйбла, чтобы они не рисовали фон лишний раз
            label5.Parent = panel1;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e) {
            label1.Select();//Нам нужно убирать фокус пользователя с текстовых полей, но панель невозможно "выбрать".
        }
    }
}
