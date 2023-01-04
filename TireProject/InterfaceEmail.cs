using System.Threading.Tasks;

namespace TireProject
{
    public interface InterfaceEmail
    {
        Task sendEmailFunc(string subject, string message, string pathName);
        //void SendSMS(string mesage);
    }
}
