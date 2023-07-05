using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Rebel_Coder {
    public static class GlobalStats {//Класс с рекордами игрока.
        public static int maxLvl=0;//Хотел сделать выбор уровня из открытых, но уже не успеваю
        public static int timeRecord=int.MaxValue;//Рекорд по времени

        public static void init() {//Подгружаем текуще рекорды при запуске приложения
            using (var cmd = SaveManager.conn.CreateCommand()) {
                cmd.CommandText = "SELECT * from globalstats";
                var res = cmd.ExecuteReader();
                while (res.Read()) {
                    switch (res.GetInt32(0)) {//Некоторых характеристик может не быть, поэтому перебираем.
                        case 0:
                            maxLvl = res.GetInt32(1);
                            break;
                        case 1:
                            timeRecord = res.GetInt32(1);
                            break;
                    }
                }
            }
        }

        public static void tryUpdate(Save save) {//По прохождении игры вызывается этот метод, который пытается поставить новые рекорды.
            if (save.time < timeRecord) {
                SaveManager.save("globalstats", "id", "id", "1", "value", save.time + "");
                timeRecord = save.time;
            }
            if (save.lvl > maxLvl) {
                SaveManager.save("globalstats", "id", "id", "0", "value", save.lvl + "");
                maxLvl = save.lvl;
            }
        }
    }
}
