using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


public interface IFileWorker {
    Task<bool> ExistsAsync(string filename); // проверка существования файла
    Task SaveTextAsync(string filename, string text);   // сохранение текста в файл
    Task<string> LoadTextAsync(string filename);  // загрузка текста из файла
    Task<IEnumerable<string>> GetFilesAsync();  // получение файлов из определнного каталога
    Task DeleteAsync(string filename);  // удаление файла
    string GetFilePath(string filename);
}
