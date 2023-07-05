using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace The_Rebel_Coder {
    public partial class VideoForm : GeForm {
        //Универсальное окно-проигрыватель видео.
        public VideoForm() {
            InitializeComponent();
        }
        private int setAfterEnd;//На какое окно переключить, когда видео будет окончено
        public void play(string path, int set) {//Метод запуска видео
            axWindowsMediaPlayer1.URL=path;
            axWindowsMediaPlayer1.uiMode = "none";
            setAfterEnd = set;
            if (setAfterEnd >= Program.forms.Count()) setAfterEnd = 0;
        }
        //Отлов ивента окончания видео (просто включаем форму, что была указана при запуске)
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e) {
            if(e.newState==8) Program.changeForm(setAfterEnd);
        }
    }
}
