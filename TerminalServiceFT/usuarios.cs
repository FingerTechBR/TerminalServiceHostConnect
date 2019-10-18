using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalServiceFT
{
    class usuarios
    {
       private string sUserName;
       private string sDomain;
       private string sClientApplicationDirectory;
       private string sIPAddress;

        public string SUserName { get => sUserName; set => sUserName = value; }
        public string SDomain { get => sDomain; set => sDomain = value; }
        public string SClientApplicationDirectory { get => sClientApplicationDirectory; set => sClientApplicationDirectory = value; }
        public string SIPAddress { get => sIPAddress; set => sIPAddress = value; }
    }
}
