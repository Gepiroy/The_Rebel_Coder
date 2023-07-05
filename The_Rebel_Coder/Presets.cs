using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Rebel_Coder {
    public static class Presets {//Класс с пресетами частоиспользуемых вещей. Экономит ресурсы. И время.
        public static string path = Directory.GetCurrentDirectory() + @"\src\";

        public static Random r = new Random(); //Центральный генератор псевдослучайности.

        public static Pen blackPen = new Pen(Color.Black);
        public static Pen blackPen3 = new Pen(Color.Black, 3);
        public static Pen greenPen = new Pen(Color.Green);

        public static SolidBrush blackBrush = new SolidBrush(Color.Black);
        public static SolidBrush greenBrush = new SolidBrush(Color.Green);
        public static SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        public static SolidBrush halfYellowBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 0));

        public static Font font = new Font("Times New Roman", 14);
        public static Font font2 = new Font("Times New Roman", 12);
        public static Font font3 = new Font("Times New Roman", 10);
    }
}
