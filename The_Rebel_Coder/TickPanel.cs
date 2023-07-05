using System.Windows.Forms;

namespace The_Rebel_Coder {
    class TickPanel : Panel{
        //Обычный оверрайд панели с характеристиками, необходимыми для решения проблем с частообновляемым отображением.
        public TickPanel() {//Мы просто задаём стиль панели, который нельзя сделать через дизайнер
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true
            );
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);//Это нужно для корректного отображения в дизайнере.
        }
    }
}
