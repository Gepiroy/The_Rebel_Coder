using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    public static class SaveManager {//Общий класс для работы со SQLite и сохранениями.
        public static SQLiteConnection conn;//Подключение к БД
        public static List<Save> saves = new List<Save>();//Список загруженных из БД в объекты сохранений
        public static void init() {//Инициатор класса, запускаемый при старте программы
            conn = new SQLiteConnection("Data Source=database.sqlite3");//Создаём подключение
            if (!File.Exists("./database.sqlite3")) {//Создаём файл, если нужно
                SQLiteConnection.CreateFile("database.sqlite3");
            }
            conn.Open();//Открываем подключение.
            using (var cmd = conn.CreateCommand()) {//Создаём 2 основные таблицы
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS saves(date date not null primary key, lvl int not null, time int not null)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS globalstats(id int not null primary key, value int not null)";//Здесь хранится рекорд и максимальный пройденный уровень.
                cmd.ExecuteNonQuery();
            }
            using (var cmd = conn.CreateCommand()) {//Загружаем все сохранения, что там были
                cmd.CommandText = "SELECT * from saves";
                var res = cmd.ExecuteReader();
                while (res.Read()) {
                    saves.Add(new Save(res));
                }
            }
        }

        public static Save createSave() {//Грамотно создаём новый сейв, чтобы тот сразу встал на учёт в список
            Save save = new Save();
            saves.Add(save);
            return save;
        }

        /// <summary>
        /// Метод для быстрого сохранения любой инфы в любую таблицу с любыми параметрами методом "добавь запись или замени".
        /// </summary>
        public static void save(string table, string key, params string[] keyvals) {//params означает, что можно прямо перечислять неограниченное число аргументов типа string в этот метод, и они будут помещены в массив.
            //Здесь массив - это чередующиеся пары "ключ-значение".
            using (var cmd = conn.CreateCommand()) {
                string st = "insert into "+table+"(";
                for (int i=0;i<keyvals.Length;i+=2) {//Перебираем все первые элементы массива
                    st += keyvals[i];
                    if (i != keyvals.Length - 2) st += ",";
                }
                st += ") values(";
                for (int i = 1; i < keyvals.Length; i += 2) {//Переираем все вторые элементы массива
                    st += keyvals[i];
                    if (i != keyvals.Length - 1) st += ",";
                }
                st += ") on conflict("+key+") do update set (";
                int keyid = 0;
                for (int i = 0; i < keyvals.Length; i += 2) {//Перебираем все первые элементы массива
                    if (keyvals[i].Equals(key)) {
                        keyid = i;
                        continue;
                    }
                    st += keyvals[i];
                    if (i != keyvals.Length - 2) st += ",";
                }
                st += ") = (";
                for (int i = 1; i < keyvals.Length; i += 2) {//Переираем все вторые элементы массива
                    if (i == keyid + 1) continue;
                    st += keyvals[i];
                    if (i != keyvals.Length - 1) st += ",";
                }
                st += ") where " + key + "=" + keyvals[keyid+1];
                cmd.CommandText = st;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
