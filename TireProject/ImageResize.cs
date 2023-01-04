using System;
using System.Threading.Tasks;

namespace TireProject
{
    public interface IImageResize
    {
        Task<string> ResizeImage(byte[] imageData, float width, float height);
        Task<string> ResizeImage(string imagePath, string output, float width, float height);
    }
}
