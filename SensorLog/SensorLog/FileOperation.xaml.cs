using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SensorLog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileOperation : ContentPage
	{
        private readonly string filename;
        //private readonly string path;


        public FileOperation() {
            InitializeComponent();
        }

        public FileOperation(string filename) {
            InitializeComponent();
            this.filename = filename;
            //this.path = path;
            fileNameEntry.Text = "Имя файла: " + filename;
            filePathL.Text = "Путь к нему: " + DependencyService.Get<IFileWorker>().GetFilePath(filename);

        }

        // async void Gett() {
        //    string text = await DependencyService.Get<IFileWorker>().LoadTextAsync(filename);
        //    BigInteger count = 0;
        //    foreach(Match m in Regex.Matches(text, "\n")) count++;

        //    fileCount.Text = "Кол-во записей - " + (count-1);

        //    textEditor.Text = text; ;
        //}


        //protected override async void OnAppearing() {
        //    base.OnAppearing();
        //    await UpdateFileList();
        //}
        // сохранение текста в файл

        //async void FileSelect(object sender, SelectedItemChangedEventArgs args) {
        //    if (args.SelectedItem == null) return;
        //    // получаем выделенный элемент
        //    //string filename = (string)args.SelectedItem;
        //    // загружем текст в текстовое поле
        //   // textEditor.Text = await DependencyService.Get<IFileWorker>().LoadTextAsync((string)args.SelectedItem);

        //    // устанавливаем название файла
        //    //fileNameEntry.Text = filename;

        //    // снимаем выделение
        //    filesList.SelectedItem = null;

        //}
        async void Delete(object sender, EventArgs args) {
            // получаем имя файла
            //string filename = (string)((MenuItem)sender).BindingContext;
            // удаляем файл из списка
            await DependencyService.Get<IFileWorker>().DeleteAsync(filename);
            // обновляем список файлов
            //await UpdateFileList();
            await Navigation.PopAsync();
        }

        async void OpenFile(object sender, EventArgs args) {
            string text = await DependencyService.Get<IFileWorker>().LoadTextAsync(filename);
            BigInteger count = 0;
            foreach (Match m in Regex.Matches(text, "\n")) count++;

            fileCount.Text = "Кол-во записей - " + (count - 1);

            textEditor.Text = text;
        }
        //обновление списка файлов
        //async Task UpdateFileList() {
        //     // получаем все файлы
        //     filesList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
        //     // снимаем выделение
        //     filesList.SelectedItem = null;
        // }


    }
}