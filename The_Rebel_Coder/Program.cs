using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rebel_Coder {
    static class Program {
        public static List<GeForm> forms = new List<GeForm>();//Список форм на "учёте" для системы переключений.
        public static GeForm activeForm = null;//Текущая форма, которую сейчас стоит обновлять центральной системе.
        static Thread thread;//Поток, в котором будет происходить динамическое обновление форм.
        static VideoForm vid;//Отдельная от списка форма для проигрывания видео.
        public static Save save;//Сохранение, в котором мы сейчас находимся (в меню его ещё не выбрали)
        [STAThread]
        static void Main() {//Метод, запускающийся первым при старте программы.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SaveManager.init();//Инициируем статические классы перед загрузкой всего остального
            GlobalStats.init();
            forms.Add(new Form1());//Добавляем формы уровней в список в нужном нам порядке
            forms.Add(new CircleLockForm());
            forms.Add(new ForceItToMake1());
            forms.Add(new NumFight());
            forms.Add(new Graph());
            forms.Add(new Massives());
            vid = new VideoForm();//Создаём видеоформу
            changeForm(0);//Запускаем меню, грамотно подключая его к центральной системе.
            thread = new Thread(ticker);//Создаём поток центральной системы.
            thread.IsBackground = true;//Делаем так, чтобы поток закрывался при закрытии приложения.
            thread.Start();//Запускаем поток.
            Application.Run();//Запускаем приложение.
        }
        static int timer = 0;//Это число необходимо для ритма в центральном потоке. Например, чтобы воспроизводить код не каждый тик, а каждый 4-й тик (timer%4==0)
        static void ticker() {//Метод для потока центральной системы.
            while (true) {//Бесконечный цикл (можно прервать внешними силами)
                if (activeForm != null) activeForm.onTick(timer++);//Обновляем активную форму и таймер
                Thread.Sleep(50);//Частота обновлений примерно 20 раз в секунду
            }
        }

        /// <summary>
        /// Запустить видео-переход на новый уровень.
        /// </summary>
        public static void videoChangeForm(int to) {
            save.lvl = to;//Обновляем данные в сохранении
            if (activeForm != null) {
                save.watch.Stop();//Останавливаем секундомер сохранения, чтобы списать с него асечённое время
                save.time += (int)save.watch.ElapsedMilliseconds;//В инт вместится до 600 часов.
                save.watch.Reset();//Сбрасываем таймер в секундомере.
                if (to==forms.Count) {//Если мы пытаемся перейти с последнего уровня на следующий, нас должны вернуть в меню и обновить рекорды.
                    save.save(SaveManager.conn);//Сохраняем сохранение
                    GlobalStats.tryUpdate(save);//И пытаемся обновить рекорды
                    save = null;//Находясь в меню, у нас нет активного сохранения.
                }
                cloneSettings(activeForm, vid);//Переносим свойства предыдущей формы на новую.
            }
            vid.Show();//Открываем форму с видео
            if (activeForm != null) activeForm.Hide();//Скрываем предыдущую форму
            activeForm = vid;//Устанавливаем видео-форму как текущую
            vid.play(Presets.path+"videos/"+to+".mp4", to);//Запускаем наше видео
        }
        /// <summary>
        /// Прямой запуск формы без проигрывания видео
        /// </summary>
        public static void changeForm(int to) {
            if (activeForm != null) {//Устанавливаем свойства прошлого окна на новое, если прошлое вообще было
                cloneSettings(activeForm, forms[to]);
            }
            forms[to].Show();//Показываем новое окно
            if (activeForm != null) activeForm.Hide();//Скрываем старое
            activeForm = forms[to];//Устанавливаем новую форму как активную для центральной системы
            activeForm.onOpen();//Запускаем личный скрипт запуска уровня
            if(to!=0)save.watch.Start();//Начинаем засекать время, если мы не в меню перешли.
        }
        /// <summary>
        /// Скопировать настройки окна from в окно to.
        /// </summary>
        static void cloneSettings(Form from, Form to) {
            to.Size = from.Size;
            to.Location = from.Location;
            to.StartPosition = FormStartPosition.Manual;
            to.WindowState = from.WindowState;
        }

        /// <summary>
        /// Запустить код (act) в синхронизации с потоком объекта obj.
        /// </summary>
        public static void sync(Control obj, Action act) {
            if (obj.InvokeRequired) {
                obj.Invoke(act);
            } else {
                act();
            }
        }

        /// <summary>
        /// Перевести надпись во float, учитывая пустоту и некорректность ввода.
        /// Возвращает false, если перевод провалился и текст при том был не пустым.
        /// Пустой текст не окажет влияния на число и вернёт true.
        /// </summary>
        public static bool readFloat(string from, ref float to) {//out предполагает создание переменной для мульти-ретурна. ref же предполагает возможную модификацию уже существующей переменной.
            if (from.Length == 0||from.Equals("-")) return true; //пустое поле игнорируется, нечего пользователя обижать, когда он стёр старое число и готовится ввести новое.
            return float.TryParse(from.Replace('.', ','), out to);
        }
        /// <summary>
        /// Сохранить текущее сохранение (если оно есть) в БД и выйти из приложения.
        /// </summary>
        public static void saveAndExit() {
            if (save != null) {
                save.save(SaveManager.conn);
            }
            Application.Exit();
        }
    }
}
