using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    class Guy {//Этот парень был из тех, что просто любит жить...
        public float x=10, y=10, speed=5;//Координаты игрока и его скорость
        public float vecY=0;//Сила импульса (от прыжка)

        public void update(Panel panel) {//Обновляем базовую физику игрока.
            y += vecY;
            vecY *= 0.95f;
            if (x < 0) x = 0;//И не даём ему выйти за пределы экрана.
            y += 5;
        }
    }
}
