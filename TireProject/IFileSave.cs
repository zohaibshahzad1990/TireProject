using System;
using System.IO;
using System.Threading.Tasks;

namespace TireProject
{
    public interface IFileSave
    {
        void SavePicture(Stream data);
        string GetPicture();
        string GetPath();
        void OpenExcel(string filepath);
        Task SaveFileTxt(string data);
    }
}
