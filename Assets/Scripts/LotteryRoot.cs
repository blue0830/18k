/// If you're new to Strange, start with MyFirstProject.
/// If you're interested in how Signals work, return here once you understand the
/// rest of Strange. This example shows how Signals differ from the default
/// EventDispatcher.

using System;
using UnityEngine;
using strange.extensions.context.impl;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public class LotteryRoot : ContextView
{
	public readonly object locker = new object();

    void Awake()
    {
		DontDestroyOnLoad (gameObject);
        context = new LotteryContext(this);
        //Application.logMessageReceived+=HandleLog;

        config = ConfigManager.Instance.GetEmailConfigLoader();

        Application.targetFrameRate = 30;
        //设置IOS键盘输入
		TouchScreenKeyboard.hideInput = true;
    }

    string emailString = "";
    EmailConfigLoader config; 
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Error||type == LogType.Exception)
        {
            lock (locker)
                emailString += "Log: \n" + logString + "\n stackTrace " + stackTrace;
        }

        if (emailString.Length > 5000)
        {
            Thread thread = new Thread(SendEmail);
            thread.IsBackground = true;
            thread.Start();
        }
    }

    private void SendEmail()
    {
        try
        {
            string sendContent = emailString;
            lock (locker)
                emailString = "";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(config.from);
            mail.To.Add(config.to);
            mail.Subject = "bug log" + DateTime.Now.ToString();
            mail.Body = emailString;
            SmtpClient smtpServer = new SmtpClient(config.stmp);
            smtpServer.Port = config.port;
            smtpServer.Credentials = new System.Net.NetworkCredential(config.username, config.password) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mail);

            
            Debug.Log("success");
        }
        catch (Exception e)
        {
            Debug.Log("Send Email exceiption "+e);
        }
    }

    protected override void OnDestroy()
    {
        Debug.Log("  runBackThread = false; ");
    }
}