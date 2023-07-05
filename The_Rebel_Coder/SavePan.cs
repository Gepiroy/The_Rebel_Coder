using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    class SavePan : Panel{//Элемент отображения сохранения в списке в меню
        Save save;

        public SavePan() {//По умолчанию всегда должна быть панель "новая игра"
            init();
        }

        public SavePan(Save save) {//А так, подключить сохранение - и готово.
            this.save = save;
            init();
        }

        private void init() {//Базовые характеристики и ивенты элемента (мог и в дизайнере сделать)
            Cursor = Cursors.Hand;
            MouseUp += new MouseEventHandler(mup);
        }

        void mup(Object o, MouseEventArgs e) {//Когда пользователь отпускает мышь, мы загружаемся или создаём новый сейв (смотря какой это элемент)
            if (save == null) {
                Program.save = SaveManager.createSave();
            } else{
                Program.save = save;
            }
            Program.videoChangeForm(Program.save.lvl);
        }

        protected override void OnPaint(PaintEventArgs e) {//Рисуем наш элемент.
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Presets.greenPen, 0, 0, Width-1, Height-1);
            if (save == null) {
                e.Graphics.DrawString("Начать новую игру", Presets.font, Presets.blackBrush, 1, 12);
            } else {
                e.Graphics.DrawString(save.date.ToString(), Presets.font3, Presets.blackBrush, Width - e.Graphics.MeasureString(save.date.ToString(), Presets.font3).Width, -1);
                e.Graphics.DrawString("Уровень: "+save.lvl.ToString(), Presets.font2, Presets.blackBrush, 1, 12);
                e.Graphics.DrawString("Время: " + (save.time/1000.0).ToString("0.000")+" сек.", Presets.font2, Presets.blackBrush, 1, 30);
            }
        }
    }
}
