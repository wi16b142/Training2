using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training2.Models
{
    public class Entry
    {

        string username = "";
        string message = "";
        string timestamp;

        public Entry(string username, string message)
        {
            this.Username = username;
            this.Message = message;
            this.Timestamp = DateTime.Now.ToString("HH:mm");
        }

        public string Username { get => username; set => username = value; }
        public string Message { get => message; set => message = value; }
        public string Timestamp { get => timestamp; set => timestamp = value; }
    }
}
