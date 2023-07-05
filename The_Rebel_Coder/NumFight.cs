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
    public partial class NumFight : GeForm {
        private Image defimg, losimg;//Картинки на фоне. Их лучше загрузить заранее.
        public NumFight() {
            InitializeComponent();
            defimg = Image.FromFile(Presets.path + @"\images\RebelCoder_numfight.png");
            losimg = Image.FromFile(Presets.path + @"\images\RebelCoder_numfight_losing.png");
        }
        double[] nums;//Числа, участвующие в формуле.
        public override void onOpen() {//Ресет всего при активации формы
            nums = new double[]{ 2,3,1,3,5,4,3,2};
            textBox1.ResetText();
            textBox2.ResetText();
            display();
        }
        private void button1_Click(object sender, EventArgs e) {//Заменяем все числа при нажатии на кнопку
            double from = double.Parse(textBox1.Text.Replace('.',','));
            double to = double.Parse(textBox2.Text.Replace('.', ','));
            for (int i=0;i<nums.Length;i++) {
                if (nums[i] == from) nums[i] = to;
            }
            display();
        }
        void display() {//Обновляем обе формулы
            double res1 = Math.Sin(Math.PI / nums[0] + nums[1]) / (nums[2] - Math.Sin(nums[3] - Math.PI));
            if (double.IsInfinity(res1) || double.IsNaN(res1)) res1 = 0;
            double res2 = 1.0 / Math.Tan(nums[4] / nums[5] * Math.PI + nums[6] / nums[7]);
            if (double.IsInfinity(res2) || double.IsNaN(res2)) res2 = 0;
            if (res1 >= res2-0.001) {//Небольшая погрешность в double. А так, тут мы подменяем изображения.
                pictureBox1.Image = defimg;
            } else {
                pictureBox1.Image = losimg;
            }
            label1.Text = $"Sin(PI/{nums[0]}+{nums[1]}) / {nums[2]}-Sin({nums[3]}-PI) = " + res1.ToString("0.00");
            label2.Text = $"Ctg({nums[4]}/{nums[5]}*PI+{nums[6]}/{nums[7]}) = " + res2.ToString("0.00");
            if (res1 > res2) Program.videoChangeForm(4);//Если победили, то идём дальше.
        }

        private void NumFight_Load(object sender, EventArgs e) {

        }
    }
}
