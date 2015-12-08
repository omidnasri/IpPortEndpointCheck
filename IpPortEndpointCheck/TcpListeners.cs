﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class TcpListeners : NetCheckServers
    {

        public bool StartListen()
        {
            GoonListen = true;

            DoStart(TcpListeners.TcpListenerThreadProc);

            return true;
        }

        static private void TcpListenerThreadProc(object data)
        {
            TcpListeners tcpListener = (TcpListeners)data;
            int port = tcpListener.PopPort();

            
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                listener.Start();

                while (tcpListener.GoonListen)
                {
                    TcpClient cli = listener.AcceptTcpClient();
                    cli.Close();
                }

                listener.Stop();
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10048:
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            tcpListener.DoDecrease();
        }
    }
}
