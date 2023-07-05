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
    public partial class Form1 : GeForm {
        Image[] imgs;//Список картинок всех тех, что в меню
        Image chimg;//А также отдельно, на смену одну
        public Form1() {
            InitializeComponent();
            imgs = new Image[5];//Их наполняем мы новым контентом
            for (int i=0; i<imgs.Length;i++) {
                imgs[i] = Image.FromFile(Presets.path+@"images\RebelCoder_menu"+i+".png");
            }//Что заготовлен был мною тем летом
            chimg = Image.FromFile(Presets.path + @"images\RebelCoder_menu_changing.png");
        }
        private int panwidth;//Размер панелей в списке меняется, когда элементы оказываются вне него.
        public override void onOpen() {
            if (GlobalStats.timeRecord != int.MaxValue) label1.Text = "Рекорд: "+(GlobalStats.timeRecord/1000.0).ToString("0.000")+" сек.";
            flowLayoutPanel1.Controls.Clear();
            panwidth = flowLayoutPanel1.ClientSize.Width - 24;//Поэтому обновляем ширину только один раз.
            int height = Math.Min(flowLayoutPanel1.Height, (SaveManager.saves.Count+1)*57);
            flowLayoutPanel1.Height = height;
            foreach (Save save in SaveManager.saves) {
                addSPan(new SavePan(save));
            }
            addSPan(new SavePan());//New game. Они отображаются снизу вверх (чтобы новейшие сохранения выше были)
        }
        private static int rate = 60;//Смене картин зададим сейчас ритму
        public override void onTick(int timer) {//И будем их каждый мы тик проверять.
            if (timer % rate == 0) {//Коль время настанет - заменят картину.
                pictureBox1.Image = imgs[timer / rate % imgs.Length];
            }else if (timer%rate==rate*0.9) {//Но перед этим на "пшик" поменять.
                pictureBox1.Image = chimg;
            }
        }

        void addSPan(SavePan pan) {//Добавление панели в список с нужными размерами
            pan.Height = 50;
            pan.Width = panwidth;
            flowLayoutPanel1.Controls.Add(pan);
        }

        private void Form1_Load(object sender, EventArgs e) {
            label1.Parent = pictureBox1; //Winform's drawing system не способна рисовать отдельные прозрачные объекты, поэтому
            //приходится совать один объект в другой и рисовать друг на друге. Иначе всегда будет непрозрачный фон.
            
        }
    }
}
