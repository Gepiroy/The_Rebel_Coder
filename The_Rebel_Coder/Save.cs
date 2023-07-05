using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace The_Rebel_Coder {
    public class Save {//Класс сохранения.
        public DateTime date;//Дата создания сейва
        public int lvl=1;//Уровень, на коем сейчас игрок
        public int time=0;//Время прохождения. В миллисекундах, может сохранить до 600 часов.

        public Stopwatch watch = new Stopwatch();//Грубо говоря секундомер.

        public Save() {//Можем как создать новое
            date = DateTime.Now;
        }
        public Save(SQLiteDataReader data) {//Так и загрузить
            date = data.GetDateTime(0);
            lvl = data.GetInt32(1);
            time = data.GetInt32(2);
        }

        public void save(SQLiteConnection conn) {//Ну и сохранить в БД конечно
            string datest = "'"+date.ToString("yyyy-MM-dd HH:mm:ss.fff")+"'";
            SaveManager.save("saves", "date", 
                "date",datest,
                "lvl",""+lvl,
                "time",""+time);
        }
    }
}
