using System.IO;
using System.Threading.Tasks;

namespace TireProject
{
    public interface IConvertToPng
    {
        Task<string> Convert(string PdfPath);
    }
}
