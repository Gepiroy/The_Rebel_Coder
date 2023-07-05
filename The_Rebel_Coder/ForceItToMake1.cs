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
    public partial class ForceItToMake1 : GeForm {
        private static Random r = Presets.r;//С пресетов добавим немного рандома
        float x=1, y= 1;//И выставим числа для формул дефолтно
        public ForceItToMake1() {
            InitializeComponent();
            label1.Text = "3^x - 4x + (y - √|x|).";//Напишем весь текст, коль создали форму
        }
        public override void onOpen() {//И числа срандомим, открыв эту форму.
            x = (float)(r.Next(51) / 10.0+0.1); y = (float)(r.Next(51) / 10.0+0.1);
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
        }
        private void textBox1_TextChanged(object sender, EventArgs e) {//Мы слышим, как юзер меняет наш текст
            Program.readFloat(textBox1.Text, ref x);
            tryCount();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            Program.readFloat(textBox2.Text, ref y);
            tryCount();
        }

        private void ForceItToMake1_Load(object sender, EventArgs e) {

        }

        private void tryCount() {//И числа сменить сразу рвёмся в ответ
            double result = x * x * x - 4 * x + (y - Math.Sqrt(Math.Abs(x)));
            label2.Text = $"3^{x} - 4*{x} + ({y} - √|{x}|) = {result.ToString("0.00")} / 0.00";
            if (result == 0) Program.videoChangeForm(3);
        }
    }
}
