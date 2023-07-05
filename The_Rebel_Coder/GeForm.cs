using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    public partial class GeForm : Form {//Родительский класс всех форм.
        public GeForm() {//Автоматически все формы будут вызывать скрипт закрытия при закрытии.
            FormClosed += new FormClosedEventHandler(onClose);
        }
        public virtual void onTick(int timer) {//virtual позволяет делать некую основу для будущих необязательных override методов.
            //Это - подключение к системе обновления в реальном времени.
        }
        public virtual void onOpen() {
            //Это - метод, вызывающийся при переходе к окну из центральной системы.
        }
        public virtual void onClose(object sender, EventArgs e) {
            Program.saveAndExit();//Базовый скрипт на случаи закрытия окна
        }
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);//Нужно чтобы дизайнер visual studio не тупил
        }
    }
}
