using System;
using System.Threading.Tasks;

namespace TireProject
{
    public interface IShare
    {
        Task Show(string title, string message, string email, string number, string filePath);
    }
}
