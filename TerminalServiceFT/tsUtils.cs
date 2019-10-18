using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TerminalServiceFT
{
    class tsUtils
    {

        usuarios users = new  usuarios();


        ArrayList arraylist = new ArrayList();


        IntPtr pServer = IntPtr.Zero;
        string sUserName = string.Empty;
        string sDomain = string.Empty;
        string sClientApplicationDirectory = string.Empty;
        string sIPAddress = string.Empty;


        public void retornausuarios()
        {

            RDPDLL.WTS_CLIENT_ADDRESS oClientAddres = new RDPDLL.WTS_CLIENT_ADDRESS();
            RDPDLL.WTS_CLIENT_DISPLAY oClientDisplay = new RDPDLL.WTS_CLIENT_DISPLAY();

            IntPtr pSessionInfo = IntPtr.Zero;

            int iCount = 0;
            int iReturnValue = RDPDLL.WTSEnumerateSessions(pServer, 0, 1, ref pSessionInfo, ref iCount);
            int iDataSize = Marshal.SizeOf(typeof(RDPDLL.WTS_SESSION_INFO));

            int iCurrent = (int)pSessionInfo;

            if (iReturnValue != 0)
            {
                //Go to all sessions
                for (int i = 0; i < iCount; i++)
                {
                    RDPDLL.WTS_SESSION_INFO oSessionInfo = (RDPDLL.WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)iCurrent,
                typeof(RDPDLL.WTS_SESSION_INFO));
                    iCurrent += iDataSize;
                    uint iReturned = 0;

                    //Get the IP address of the Terminal Services User
                    IntPtr pAddress = IntPtr.Zero;
                    if (RDPDLL.WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, RDPDLL.WTS_INFO_CLASS.WTSClientAddress,
            out pAddress, out iReturned) == true)
                    {
                        oClientAddres = (RDPDLL.WTS_CLIENT_ADDRESS)Marshal.PtrToStructure
                    (pAddress, oClientAddres.GetType());
                        sIPAddress = oClientAddres.bAddress[2] + "." +
                                oClientAddres.bAddress[3] + "." + oClientAddres.bAddress[4]
                                + "." + oClientAddres.bAddress[5];
                    }
                    //Get the User Name of the Terminal Services User
                    if (RDPDLL.WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, RDPDLL.WTS_INFO_CLASS.WTSUserName,
                out pAddress, out iReturned) == true)
                    {
                        sUserName = Marshal.PtrToStringAnsi(pAddress);
                    }
                    //Get the Domain Name of the Terminal Services User
                    if (RDPDLL.WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, RDPDLL.WTS_INFO_CLASS.WTSDomainName,
                out pAddress, out iReturned) == true)
                    {
                        sDomain = Marshal.PtrToStringAnsi(pAddress);
                    }
                    //Get the Display Information  of the Terminal Services User
                    if (RDPDLL.WTSQuerySessionInformation(pServer,
                oSessionInfo.iSessionID, RDPDLL.WTS_INFO_CLASS.WTSClientDisplay,
                out pAddress, out iReturned) == true)
                    {
                        oClientDisplay = (RDPDLL.WTS_CLIENT_DISPLAY)Marshal.PtrToStructure
                (pAddress, oClientDisplay.GetType());
                    }
                    //Get the Application Directory of the Terminal Services User
                    if (RDPDLL.WTSQuerySessionInformation(pServer, oSessionInfo.iSessionID,
            RDPDLL.WTS_INFO_CLASS.WTSClientDirectory, out pAddress, out iReturned) == true)
                    {
                        sClientApplicationDirectory = Marshal.PtrToStringAnsi(pAddress);
                    }

                    Console.WriteLine("Session ID : " + oSessionInfo.iSessionID);
                    Console.WriteLine("Session State : " + oSessionInfo.oState);
                    Console.WriteLine("Workstation Name : " +
                oSessionInfo.sWinsWorkstationName);
                    Console.WriteLine("IP Address : " + sIPAddress);
                    Console.WriteLine("User Name : " + sDomain + @"\" + sUserName);
                    Console.WriteLine("Client Display Resolution: " +
            oClientDisplay.iHorizontalResolution + " x " +
                oClientDisplay.iVerticalResolution);
                    Console.WriteLine("Client Display Colour Depth: " +
                oClientDisplay.iColorDepth);
                    Console.WriteLine("Client Application Directory: " +
                sClientApplicationDirectory);

                    Console.WriteLine("-----------------------");
                }

                RDPDLL.WTSFreeMemory(pSessionInfo);
                Console.ReadKey();
            }

        }

    }
}
