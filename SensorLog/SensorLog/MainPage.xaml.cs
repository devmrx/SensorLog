using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Numerics;

namespace SensorLog {
    public partial class MainPage : ContentPage {

        private string filename = "";
        private BigInteger count = 0;



        public MainPage() {
            InitializeComponent();

            ToolbarItem tb1 = new ToolbarItem {
                Text = "Настройки",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };

            ToolbarItem tb2 = new ToolbarItem {
                Text = "Об авторе",
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };

            ToolbarItem tb3 = new ToolbarItem {
                Text = "О программе",
                Order = ToolbarItemOrder.Secondary,
                Priority = 3
            };

            tb1.Clicked += (s, e) => {
                Navigation.PushAsync(new Setting());
            };

            ToolbarItems.Add(tb1);
            ToolbarItems.Add(tb2);
            ToolbarItems.Add(tb3);

            if (filename == "") filename = "LogPosition - " + DateTime.Now.TimeOfDay;
        }



        async void Save(string text) {

            if (String.IsNullOrEmpty(filename)) return;
            // если файл существует
            if (!await DependencyService.Get<IFileWorker>().ExistsAsync(filename)) {
                // запрашиваем разрешение на перезапись
                //bool isRewrited = await DisplayAlert("Подверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");
                //if (isRewrited == false) return;
                count = 0;
            }
            // перезаписываем файл
            await DependencyService.Get<IFileWorker>().SaveTextAsync(filename, text);
            // обновляем список файлов
        }

        private void BtnStart_OnClicked(object sender, EventArgs e) {
            string X, Y, Z;

            //IDeviceInfo deviceInfo = DependencyService.Get<IDeviceInfo>();

            //Test1.Text = deviceInfo.GetInfo();

            ///// count add

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);

            //Task.Factory.StartNew(() => MyTask(), TaskCreationOptions.LongRunning);


            Task.Factory.StartNew(() => CrossDeviceMotion.Current.SensorValueChanged += async (s, a) => {
                switch (a.SensorType) {
                    case MotionSensorType.Accelerometer:

                        X = ((MotionVector)a.Value).X.ToString("F");
                        Y = ((MotionVector)a.Value).Y.ToString("F");
                        Z = ((MotionVector)a.Value).Z.ToString("F");
                        count++;
                        Posit.Text = $"x = {X}, y = {Y}, z = {Z}";
                        //Lbl1.Text = $"x = {X}, y = {Y}, z = {Z}" + Environment.NewLine + Lbl1.Text;
                        Lbl_Count.Text = "Кол-во записей - " + count.ToString();
                        //Debug.WriteLine($"{X}|{Y}|{Z}");

                        Save($"{X}|{Y}|{Z}" + Environment.NewLine);
                        //textEditor.Text = await DependencyService.Get<IFileWorker>().LoadTextAsync(filename);
                        Lbl_Sp.Text = "Список файлов";
                        await UpdateFileList();
                        break;
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void BtnStop_OnClicked(object sender, EventArgs e) {
            CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);
        }

        async void FileSelect(object sender, SelectedItemChangedEventArgs args) {
            if (args.SelectedItem == null) return;
            // получаем выделенный элемент
            string filename = (string)args.SelectedItem;
            // загружем текст в текстовое поле    
            //textEditor.Text = await DependencyService.Get<IFileWorker>().LoadTextAsync((string)args.SelectedItem);
            // устанавливаем название файла
            //fileNameEntry.Text = filename;
            // снимаем выделение
            filesList.SelectedItem = null;


            await Navigation.PushAsync(new FileOperation(filename));
        }

        // обновление списка файлов
        async Task UpdateFileList() {
            // получаем все файлы
            filesList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            // снимаем выделение
            filesList.SelectedItem = null;
        }


    }
}
