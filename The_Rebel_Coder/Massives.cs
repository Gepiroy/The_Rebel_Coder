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
    public partial class Massives : GeForm {
        private static Random r = Presets.r;//Используем рандом из пресетов.
        float[] arr = new float[7];//Для избежания ошибок массив должен быть не пустым до запуска уровня.
        public Massives() : base(){
            InitializeComponent();
        }

        public override void onOpen() {//Обновляем форму при открытии центральной системой...
            arr = new float[7];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = (arr.Length - i) * 1f;
            }
            resize();
            updateLabels();
        }
        float part=10;//Размер одного элемента массива, выводящегося на экран.
        void resize() {
            part = panel1.Width / arr.Length;
        }

        private void Massives_Load(object sender, EventArgs e) {

        }
        private static int step = 2;//Это для удобства экспериментов
        int selected = -1;//Элемент, выбранный мышкой сейчас
        private void panel1_Paint(object sender, PaintEventArgs e) {//Рисуем панель массива
            float part = panel1.Width / arr.Length;
            Graphics g = e.Graphics;
            g.DrawRectangle(Presets.blackPen, 0, 0, panel1.Width-1, panel1.Height-1);
            Point m = panel1.PointToClient(Cursor.Position);
            g.FillRectangle(Presets.halfYellowBrush, arr.Length/2 * part + step, step, part - 2 * step, panel1.Height - 1 - 2 * step);
            for (int i=0;i<arr.Length;i++) {
                if (selected == i) {
                    g.DrawRectangle(Presets.greenPen, m.X-part/2, step, part - 2 * step, panel1.Height - 1 - 2 * step);
                    g.DrawString(arr[i].ToString("0.0"), Presets.font, Presets.greenBrush, m.X - part/2 + step + 2, step + 2);
                } else {
                    g.DrawRectangle(Presets.blackPen, i*part+step,step,part-2*step, panel1.Height-1-2*step);
                    g.DrawString(arr[i].ToString("0.0"), Presets.font, Presets.blackBrush, i * part + step + 2, step + 2);
                }
            }
        }

        public override void onTick(int rate) {
            panel1.Invalidate();//Перерисовываем панель каждый тик, чтобы элемент массива красиво тянулся.
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e) {//Когда мышью нажали - элемент взяли.
            selected = Math.Min((int)(e.X / part),arr.Length-1);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e) {//А коль отпустили - элемент просадили.
            int newPos = Math.Max(0,Math.Min((int)(e.X / part), arr.Length - 1));
            move(selected, newPos);
            selected = -1;
            updateLabels();
        }

        void updateLabels() {//Числа для лейблов дружно считая, их обновляем мы, тексты вставляя.
            float left = 0, right=1;
            int maxi = 0;
            for (int i=0;i<arr.Length;i++) {
                if (arr[i] > arr[arr.Length / 2]) left++;
                if (Math.Abs(arr[maxi]) < Math.Abs(arr[i])) {
                    maxi = i;
                }
            }
            if (maxi == arr.Length - 1) right = 0;
            for (int i = maxi+1; i < arr.Length; i++) {
                right *= arr[i];
            }
            right = Math.Abs(right);
            labelLeft.Text = left.ToString("0.0");
            labelRight.Text = right.ToString("0.0");
            if (left > right) Program.videoChangeForm(6);//А коли получится верно сложить - прямо к победе вперёд вас пустить!
        }

        void move(int from, int to) {//Простой метод для перемещения элемента массива.
            float saved = arr[from];
            //Нужно сдвинуть массив влево, чтобы заполнить пустоту от взятого элемента
            for (int i = from; i < arr.Length - 1; i++) {
                arr[i] = arr[i + 1];
            }
            //Затем вправо от места, куда перенесли взятый элемент.
            for (int i = arr.Length - 1; i > to; i--) {
                arr[i] = arr[i - 1];
            }
            arr[to] = saved;
        }
    }
}
