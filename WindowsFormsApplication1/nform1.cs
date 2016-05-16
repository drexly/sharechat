using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using svchoset;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static FileStream fs;
        static StreamWriter sw;
        //static string path = @"2artbde.army.mil/upload/board/3993/said.log";// + "said.log";
        //static string realkey = @"2artbde.army.mil/upload/board/3993/said.log";// + "rkey.cer";
        static string path = @Directory.GetCurrentDirectory() + "\\said.log";
        //static string errpath = @Directory.GetCurrentDirectory() + "\\bugreport.log";
        static string realkey = @Directory.GetCurrentDirectory() + "\\rkey.cer";
        static string ip = null;
        static long filesize;
        static string secret = "password";
        static int active = 0;
        static string current = null;
        static bool onmain = true;
        static bool security = false;
        static string[] rkeyarr;
        static string decrikey;
        static bool realexit = false;
        static string realname=null;
        static bool hasbeen=false;
        static bool pop = false;
        static bool poponce = true;
        static bool javis = false;
        static string javistemp = null;
        static RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static string info = "\n\n\n\t\t   SECURE MODE LOCK ACTIVATED: ENTER CORRECT PASSWORD OR LEAVE\n\n\n\n\t\tF1: Opacity+\t F2: Opacity-\t F3: BLACK/WHITE\t F11: BALLOON/POP\n\n\n\t\tF4: TYPING CLEAR\t F5: REFRESH DIALOGUE MANUALLY\n\n\n\t\tF6: SECURE MODE LOCK ON/OFF(like PC KaTalk)\t F7: RESET PW\n\n\n\t\tF8: DIALOGUE CLEAR\tF10: START PROGRAM ON/OFF\tF12: EXIT TO TRAY";
        [DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);
        [StructLayout(LayoutKind.Sequential)]

        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        public const UInt32 FLASHW_STOP = 0;//stop flashin system restores the window to its original state
        public const UInt32 FLASHW_CAPTION = 1;//flashin the window caption
        public const UInt32 FLASHW_TRAY = 2;//flashin the taskbar button
        public const UInt32 FLASHW_ALL = 3;//flashin the taskbar button&caption
        public const UInt32 FLASHW_TIMER = 4;//flashin continuously till flashw_stop flag is set
        public const UInt32 FLASHW_TIMERNOFG = 12;//flashin continuously till window comes to the foreground

      
        public Form1()
        {
            InitializeComponent();
            textBox1.Focus();
            richTextBox1.Text = info;
            richTextBox1.Enabled = false;
            textBox1.UseSystemPasswordChar = true;
            getname();
            getip();
            if (System.IO.File.Exists(path))
            {
                connecting();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                if (security)
                {
                    Text = string.Format("RAWS *SECURED* {0}::{1}", active, current);
                }
                else
                {
                    Text = string.Format("RAWS {0}::{1}", active, current);
                }
            }
            Text = "����ݸ�� ON(Off/On F6), �������α׷� ��� OFF(����Ϸ��� F10), ���� ����Է� �� ENTER";
        }

        static void connecting()
        {
            refreshkey();
            string[] inandout = System.IO.File.ReadAllLines(path, Encoding.Default);
            int s = 0;
            foreach (string item in inandout)
            {
                if (item.Contains("���"))
                {
                    s += 1;
                    continue;
                }
                inandout[s++] = AESDecrypt256(item, decrikey);
            }
            current = null;
            active = 0;
            for (int j = 0; j < inandout.Length; j++)
            {
                if (inandout[j][0] == '��')
                {
                    string inip = null;
                    for (int i = 1; !char.IsLetter(inandout[j][i]); i++)
                    {
                        inip += inandout[j][i];
                    }
                    bool isin = true;
                    for (int k = j; k < inandout.Length; k++)
                    {
                        if (inandout[k][0] == '��' && inandout[k].Contains(inip))
                        {
                            isin = false;
                        }
                    }
                    if (isin)
                    {
                        active++;
                        current += (inip + " ");
                    }
                }
            }
            
        }
      
        static void getname()
        {
            string[] nameo = Application.StartupPath.ToString().Split('\\');
            realname=nameo[nameo.Length - 1].ToUpper();
            /*string engname=nameo[nameo.Length - 1].ToUpper();
            foreach (char item2 in engname)
            {
                switch (item2)
                {
                    default:
                        realname += item2; break;
                    case 'A':
                        realname += '��'; break;
                    case 'B':
                        realname += '��'; break;
                    case 'C': realname += '��'; break;
                    case 'D': realname += '��'; break;
                    case 'E': realname += '��'; break;
                    case 'F': realname += '��'; break;
                    case 'G': realname += '��'; break;
                    case 'H': realname += '��'; break;
                    case 'I': realname += '��'; break;
                    case 'J': realname += '��'; break;
                    case 'K': realname += '��'; break;
                    case 'L': realname += '��'; break;
                    case 'M': realname += '��'; break;
                    case 'N': realname += '��'; break;
                    case 'O': realname += '��'; break;
                    case 'P': realname += '��'; break;
                    case 'Q': realname += '��'; break;
                    case 'R': realname += '��'; break;
                    case 'S': realname += '��'; break;
                    case 'T': realname += '��'; break;
                    case 'U': realname += '��'; break;
                    case 'V': realname += '��'; break;
                    case 'W': realname += '��'; break;
                    case 'X': realname += '��'; break;
                    case 'Y': realname += '��'; break;
                    case 'Z': realname += '��'; break;
                    case ' ': realname += ' '; break;
                    case '��': realname += 'A'; break;
                    case '��': realname += 'B'; break;
                    case '��': realname += 'C'; break;
                    case '��': realname += 'D'; break;
                    case '��': realname += 'E'; break;
                    case '��': realname += 'F'; break;
                    case '��': realname += 'G'; break;
                    case '��': realname += 'H'; break;
                    case '��': realname += 'I'; break;
                    case '��': realname += 'J'; break;
                    case '��': realname += 'K'; break;
                    case '��': realname += 'L'; break;
                    case '��': realname += 'M'; break;
                    case '��': realname += 'N'; break;
                    case '��': realname += 'O'; break;
                    case '��': realname += 'P'; break;
                    case '��': realname += 'Q'; break;
                    case '��': realname += 'R'; break;
                    case '��': realname += 'S'; break;
                    case '��': realname += 'T'; break;
                    case '��': realname += 'U'; break;
                    case '��': realname += 'V'; break;
                    case '��': realname += 'W'; break;
                    case '��': realname += 'X'; break;
                    case '��': realname += 'Y'; break;
                    case '��': realname += 'Z'; break;
                }
            }*/
        }
        static void getip()
        {
            System.Diagnostics.ProcessStartInfo proInfo = new System.Diagnostics.ProcessStartInfo();//Process ���� ������ ������ ProcessStartInfo����
            System.Diagnostics.Process pro = new System.Diagnostics.Process();//Process����
            proInfo.FileName = "CMD.exe";//������ ���ϸ� �Է�--cmd
            proInfo.WorkingDirectory = @"C:\windows\system32";
            proInfo.CreateNoWindow = true;//cmdâ ����
            proInfo.UseShellExecute = false;//
            proInfo.RedirectStandardOutput = true;//cmd ���� ���� ������ �ޱ�
            proInfo.RedirectStandardInput = true;//cmd �� ������ ������
            proInfo.RedirectStandardError = true;//cmd �������� �ޱ�

            pro.StartInfo = proInfo;//Process �������� �߰�
            pro.Start();//Process ����
            pro.StandardInput.Write("ipconfig" + Environment.NewLine);
            pro.StandardInput.Close();
            String resultValue = pro.StandardOutput.ReadToEnd();
            pro.Close();
            int IPindex = resultValue.IndexOf("IPv4");
            for (int i = 28; i < 40; i++)
            {
                ip += resultValue[IPindex + i];
            }
            ip = ip.Trim();
            return;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.Enabled = true;
            textBox1.Focus();
            string Tex = e.KeyCode.ToString();
            if (Tex.Equals("Return"))
            {
                refreshkey();
                if (javis)
                {
                    string did = null;
                    if (textBox1.Text == "rlawngusslacksdidgkqslek")
                    {
                        did = (ip + ">>\t" + javistemp + "\t" + "@" + DateTime.Now.ToString());
                    }
                    else
                    {
                        did = (ip + ">>\t�ΰ����� �ں񽺴� ���δ� ���� ��´�ϴ�\t" + "@" + DateTime.Now.ToString());
                    }
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    FileInfo said = new FileInfo(path);
                    filesize = said.Length;
                    textBox1.Clear();                    
                    textBox1.UseSystemPasswordChar = false;
                    javis = false;
                    richTextBox1_DoubleClick(sender, e);
                }
                else if (textBox1.Text.Contains("�ں�,"))
                {
                    javistemp = textBox1.Text;
                    textBox1.Clear();
                    textBox1.UseSystemPasswordChar = true;
                    javis = true;
                }
                else if (textBox1.UseSystemPasswordChar)
                {
                    string ekey = AESEncrypt256(textBox1.Text,secret);
                    if (System.IO.File.Exists(realkey))
                    {
                        if (rkeyarr[0] == ekey)
                        {
                            textBox1.Clear();
                            if (!security)
                            {
                                string did = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� �����ϼ̽��ϴ�");
                                hasbeen = true;
                                richTextBox1.AppendText(did + "\n");
                                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                                sw.WriteLine(AESEncrypt256(did, decrikey));
                                textBox1.Clear();
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                            }
                          
                            FileInfo said = new FileInfo(path);
                            filesize = said.Length;
                            textBox1.UseSystemPasswordChar = false;
                            security = true;
                        }
                        else
                        {
                            refreshkey();
                            string did = ("[X]" + ip + " ���� " + DateTime.Now.ToString() + "�� �߸��� ��й�ȣ" + textBox1.Text + "�� �Է��Ͽ����ϴ�.");
                            string did1 = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� ������ϼ̽��ϴ�");
                            richTextBox1.AppendText(did + "\n" + did1 + "\n");
                            fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                            sw = new StreamWriter(fs, System.Text.Encoding.Default);
                            sw.WriteLine(AESEncrypt256(did, decrikey));
                            sw.WriteLine(AESEncrypt256(did1, decrikey));
                            textBox1.Clear();
                            sw.Flush();
                            sw.Close();
                            fs.Close();
                            FileInfo said = new FileInfo(path);
                            filesize = said.Length;
                            realexit = true;
                            this.Close();
                            Application.Exit();
                        }
                        richTextBox1_DoubleClick(sender, e);                        
                    }
                    else
                    {
                        Form2 form2 = new Form2();
                        form2.ShowDialog();
                    }
                }
                else
                {
                    refreshkey();
                    string did = (ip + ">>\t" + textBox1.Text + "\t" + "@" + DateTime.Now.ToString());
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
                    textBox1.Clear();
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    FileInfo said = new FileInfo(path);
                    filesize = said.Length;
                    richTextBox1_DoubleClick(sender, e);
                    
                }

            }
            else if (Tex.Equals("F1"))
            {
                Opacity += 0.1;
            }
            else if (Tex.Equals("F2"))
            {
                Opacity -= 0.1;
            }
            else if (Tex.Equals("F3"))
            {
                Color temp = richTextBox1.BackColor;
                richTextBox1.BackColor = richTextBox1.ForeColor;
                richTextBox1.ForeColor = temp;
                temp = textBox1.BackColor;
                textBox1.BackColor = textBox1.ForeColor;
                textBox1.ForeColor = temp;
                temp = this.BackColor;
                this.BackColor = this.ForeColor;
                this.ForeColor = temp;
            }
            else if (Tex.Equals("F4"))
            {
                textBox1.Clear();
            }
            else if (Tex.Equals("F5"))
            {
                richTextBox1_DoubleClick(sender, e);
            }
            else if (Tex.Equals("F6"))
            {
                if (!textBox1.UseSystemPasswordChar)
                {
                    if (security)
                    {
                        security = false;
                        notifyIcon1.BalloonTipTitle = realname;
                        notifyIcon1.BalloonTipText = "����ݸ�尡 ��Ȱ��ȭ �Ǿ����ϴ�(��ݸ�� Ȱ��ȭ F6)";
                        notifyIcon1.ShowBalloonTip(50);
                    }
                    else
                    {
                        security = true;
                        notifyIcon1.BalloonTipTitle = realname;
                        notifyIcon1.BalloonTipText = "����ݸ�尡 Ȱ��ȭ �Ǿ����ϴ�(��ݸ�� ���� F6)";
                        notifyIcon1.ShowBalloonTip(50);
                    }
                   
                }
               
            }
            else if (Tex.Equals("F7"))
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
                //  SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                // this.BackColor = Color.Transparent;
                //  richTextBox1.BackColor= Color.Transparent;
                //  textBox1.BackColor= Color.Transparent;

                //Color temp = TransparencyKey;
                //TransparencyKey = this.BackColor;
            }
            else if (Tex.Equals("F8"))
            {
                richTextBox1.Clear();
            }
            else if (Tex.Equals("F10"))
            {
                if (rk.GetValue(Application.ExecutablePath.ToString()) == null)
                {
                    rk.SetValue(realname, Application.StartupPath.ToString() + "\\baro.lnk");
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "�ڽ������α׷��� ��� �Ǿ����ϴ� (��� ���� F10)";
                    notifyIcon1.ShowBalloonTip(50);
                }
                else 
                {
                    rk.DeleteValue(realname, false);
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "�ڽ������α׷����κ��� ���� �Ǿ����ϴ� (���� F10)";
                    notifyIcon1.ShowBalloonTip(50);
                }
            }
            else if (Tex.Equals("F11"))
            {
                if (pop)
                {
                    pop = false;
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "������ Ʈ���� ǳ�����θ� �˸��̿ɴϴ�.\n(�˾����� �ٲٷ��� F11)";
                    notifyIcon1.ShowBalloonTip(50);
                }
                else
                {
                    pop = true;
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "������ �˾����θ� �˸��̿ɴϴ�.\n(Ʈ���� ǳ������ �ٲٷ��� F11)";
                    notifyIcon1.ShowBalloonTip(50);
                }
            }
            else if (Tex.Equals("F12"))
            {
                this.Close();
            }
            else if (Tex.Equals("F9"))
            {
                int actived = active;
                string []currented = current.Split(' ');
                richTextBox1.Clear();
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.Close();
                fs.Close();    
                foreach (string item in currented)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    string did = ("��" + item + " ���� " + DateTime.Now.ToString() + " ���� �������Դϴ�");
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
                    sw.Close();
                    fs.Close();    
                }                
            }
        }
        public static bool FlashWindowEx(IntPtr hWnd)
        {
            FLASHWINFO newmsg = new FLASHWINFO();
            newmsg.cbSize = Convert.ToUInt32(Marshal.SizeOf(newmsg));
            newmsg.hwnd = hWnd;
            newmsg.dwFlags =onmain ? (FLASHW_CAPTION | FLASHW_TIMER):(FLASHW_ALL | FLASHW_TIMERNOFG);
            newmsg.uCount = 3;
            newmsg.dwTimeout = 500;
            return (FlashWindowEx(ref newmsg) == 0);
        }
     
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(path))
            {
                FileInfo said = new FileInfo(path);
                if (said.Length != filesize)
                {    
                    filesize = said.Length;
                    richTextBox1_DoubleClick(sender, e);
                        if (this.Visible==false)
                        {
                            if (pop)
                            {
                                this.Visible = true;
                                if (this.WindowState == FormWindowState.Minimized)
                                {
                                    this.WindowState = FormWindowState.Normal;
                                }
                                this.Activate();
                            }
                            else
                            {
                                if (!hasbeen)
                                {
                                    notifyIcon1.BalloonTipTitle = realname+" LOCKED";
                                    notifyIcon1.BalloonTipText = "Unlock to Start";
                                }
                                else if (security)
                                {
                                    notifyIcon1.BalloonTipTitle = realname+" SECURE MODE";
                                    notifyIcon1.BalloonTipText = Text;
                                }
                                else
                                {
                                    notifyIcon1.BalloonTipTitle = realname + " New MSG";
                                    string[] tv = richTextBox1.Text.Split('\n');
                                   foreach (string item in tv)
                                    {
                                        if (!string.IsNullOrEmpty(item))
                                        {
                                            notifyIcon1.BalloonTipText = item;
                                        }
                                    }
                                }
                                notifyIcon1.ShowBalloonTip(50);
                            }
                        }
                        FlashWindowEx(this.Handle);
                }
            }
            /* if (!richTextBox1.Enabled&&!textBox1.UseSystemPasswordChar)
            {
                 textBox1.Enabled = false;
                         notifyIcon1.BalloonTipTitle = "��332HQ A.C.E 2015";
                         notifyIcon1.BalloonTipText = "�ý��� ���������� �����Ǿ� �����մϴ�\n�ٽ� �������ּ���";
                         notifyIcon1.ShowBalloonTip(3000);
                         fs = new FileStream(errpath, FileMode.Append, FileAccess.Write);
                         sw = new StreamWriter(fs, System.Text.Encoding.Default);
                         sw.WriteLine(DateTime.Now.ToString()+" "+ip);
                         sw.Flush();
                         sw.Close();
                         fs.Close();
                         Thread.Sleep(3000);
                         realexit = true;
                         this.Close();
                         Application.Exit();
            }*/
        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar)
            {
                refreshkey();
                richTextBox1.Clear();
                string[] textValue = System.IO.File.ReadAllLines(path, Encoding.Default);
                int s = 0;
                foreach (string item in textValue)
	            {
                    if (item.Contains("���"))
                    {
                        s += 1;
                        continue;
                    }
                     textValue[s++] = AESDecrypt256(item, decrikey);
	            }
                foreach (string item in textValue)
                {
                    richTextBox1.AppendText(item + "\n");
                    if (item[0] == '��' || item[0] == '��')
                    {
                        richTextBox1.Select(richTextBox1.TextLength - item.Length, item.Length);
                        richTextBox1.SelectionColor = Color.Blue;
                    }
                    else if (item.Contains(ip+">>"))
                    {
                        richTextBox1.Select(richTextBox1.TextLength - item.Length-1, item.Length);
                        richTextBox1.SelectionColor = Color.Green;
                    }
               
                }
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                //richTextBox1.SelectionStart=richTextBox1.Text.Length;
                connecting();
                if (security)
                {
                    Text = string.Format("RAWS *SECURED* {0}::{1}", active, current);
                }
                else
                {
                    Text = string.Format("RAWS {0}::{1}", active, current);
                }
                
                richTextBox1.ScrollToCaret();
                
            }
	else
	{
	string[] textValue = System.IO.File.ReadAllLines(path, Encoding.Default);
                int s = 0;
                foreach (string item in textValue)
	            {
                    if (item.Contains("���"))
                    {
                        s += 1;
                        continue;
                    }
                     textValue[s++] = AESDecrypt256(item, decrikey);
	            }
	foreach(string item in textValue)
	{
		  s--;
                 if (s<1&&item.Contains("�ں�,")&&item.Contains("����"))
                 {
                     if (item.Substring(15).Contains(ip))
                     {
                         string did1 = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� ������Ͽ����ϴ�");
                         fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                         sw = new StreamWriter(fs, System.Text.Encoding.Default);
                         sw.WriteLine(AESEncrypt256(did1, decrikey));
                         textBox1.Clear();
                         sw.Flush();
                         sw.Close();
                         fs.Close();
                         textBox1.Enabled = false;
                         notifyIcon1.BalloonTipTitle = "��332HQ A.C.E 2015";
                         notifyIcon1.BalloonTipText = "����� �ΰ������� ����������׽��ϴ�";
                         notifyIcon1.ShowBalloonTip(3000);
                         Thread.Sleep(3000);
                         realexit = true;
                         this.Close();
                         Application.Exit();
                     }
                 }
                 else if (item.Contains("�ں�,")&&item.Contains("�޸�ũ��������"))
                 {
                     string did1 = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� ������Ͽ����ϴ�");
                     fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                     sw = new StreamWriter(fs, System.Text.Encoding.Default);
                     sw.WriteLine(AESEncrypt256(did1, decrikey));
                     textBox1.Clear();
                     sw.Flush();
                     sw.Close();
                     fs.Close();
                     textBox1.Enabled = false;
                     notifyIcon1.BalloonTipTitle = "��332HQ A.C.E 2015";
                     notifyIcon1.BalloonTipText = "����� �ΰ������� ����������׽��ϴ�";
                     notifyIcon1.ShowBalloonTip(3000);
                     Thread.Sleep(3000);
                     realexit = true;
                     this.Close();
                     Application.Exit();
                 }
	}
	}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            if (realexit)
            {
                e.Cancel = false;
            }
            else
            {
                if (poponce)
                {
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "����� �ּ�ȭ �Ǿ����ϴ�.\n���� Ʈ���� ������ ���� Ȯ���ϼ���";
                    notifyIcon1.ShowBalloonTip(70);
                    poponce = false;
                }
                e.Cancel = true;
                this.Visible = false;
            }
        }
            /*if (!textBox1.UseSystemPasswordChar)
            {
                refreshkey();
                string did = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� �����ϼ̽��ϴ�");
                richTextBox1.AppendText(did + "\n");

                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(AESEncrypt256(did, decrikey));
                textBox1.Clear();
                sw.Flush();
                sw.Close();
                fs.Close();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                richTextBox1_DoubleClick(sender, e);
                */
            /*
                if (MessageBox.Show("���ڽ����(�۾�ǥ���� �� ȭ�鿡�� ������ ����� �޽��� �� ���� ��Ÿ��. �޽��� Ȯ�� �� �ּ�ȭ ������. ���ڽ���� Ȱ��/��Ȱ��ȭ �Ϸ��� Alt+Tab���� ã�Ƽ� F11)�� ������ '�ƴϿ�', �׳� �����Ϸ��� '��'�� ��������.", "Stealth Mode Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //if (!textBox1.UseSystemPasswordChar)
                    {
                        refreshkey();
                        string did = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� �����ϼ̽��ϴ�");
                        richTextBox1.AppendText(did + "\n");

                        fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(AESEncrypt256(did, decrikey));
                        textBox1.Clear();
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                        FileInfo said = new FileInfo(path);
                        filesize = said.Length;
                        richTextBox1_DoubleClick(sender, e);
                    }
                }
                else
                {
                    e.Cancel = true;
                    stealth = true;
                    this.Opacity = 0;
                    this.ShowInTaskbar = false;
                    Form1_Deactivate(sender, e);
                }
                
            }
            */
            /*
            if (MessageBox.Show("�������� �������α׷� ����� �����Ͻðڽ��ϱ�?\n(�̹� �������α׷��� ��ϵǾ��ֽ��ϴ�. ����ý� ��ݸ�� ���·� �����ϴ�)", "�������α׷� ���� ����", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

            }
            else
            {
                 
            
            }


        }
                if (e.CloseReason != CloseReason.UserClosing)
                    e.Cancel = false;
                else
                    e.Cancel = true;*/
               
       
       /* private void timer2_Tick(object sender, EventArgs e)
        {
            tflash++;
            if (tflash==5)
            {
                timer2.Enabled = false;
                FlashWindowEx(this.Handle);
            }
        }*/
        

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            onmain = false;
            if (!textBox1.UseSystemPasswordChar)
            {
                if (security)
                {
                    textBox1.Clear();
                    textBox1.Focus();
                    richTextBox1.Text = info;
                    richTextBox1.Enabled = false;
                    textBox1.UseSystemPasswordChar = true;
                }
                else
                {
                    if (textBox1.UseSystemPasswordChar)
                    {
                        textBox1.Clear();
                    }
                    textBox1.Focus();
                    richTextBox1.Enabled = true;
                    textBox1.UseSystemPasswordChar = false;
                }
            }
            else if (javis)
            {
                javis = false;
                if (security)
                {
                    textBox1.Clear();
                    textBox1.Focus();
                    richTextBox1.Text = info;
                    richTextBox1.Enabled = false;
                    textBox1.UseSystemPasswordChar = true;
                }
                else
                {
                    if (textBox1.UseSystemPasswordChar)
                    {
                        textBox1.Clear();
                    }
                    textBox1.Focus();
                    richTextBox1.Enabled = true;
                    textBox1.UseSystemPasswordChar = false;
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            onmain = true;

        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Focus();
        }
        static string AESEncrypt256(string InputText, string Password)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);//�Է¹������ڿ�����Ʈ�迭�κ�ȯ
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);//dictionary ���
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));//IV 16����Ʈ secretkey 32����Ʈ ���� encryptor ��ü ����
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptostream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);//cryptostream ��ü�� ��ȣȭ�� �����͸� �������� �뵵�� ����
            cryptostream.Write(PlainText, 0, PlainText.Length);
            cryptostream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptostream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            return EncryptedData;
        }
        static string AESDecrypt256(string InputText, string Password)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] EncryptedData = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);//dictionary ���
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));//IV 16����Ʈ secretkey 32����Ʈ ���� encryptor ��ü ����

            MemoryStream memoryStream = new MemoryStream(EncryptedData);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);//cryptostream ��ü�� ��ȣȭ�� �����͸� �б����� �뵵�� ����

            byte[] PlainText = new byte[EncryptedData.Length];//�Է¹������ڿ�����Ʈ�迭�κ�ȯ

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();
            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            return DecryptedData;
        }
        static void refreshkey()
        {
            if (System.IO.File.Exists(realkey))
            {

                rkeyarr = null;
                decrikey = null;
                rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                decrikey = AESDecrypt256(rkeyarr[0], secret);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar||hasbeen)
            {
                string did = ("��" + ip + " ���� " + DateTime.Now.ToString() + " �� �����ϼ̽��ϴ�");
                //active--;
                //current.Replace(ip, "");
                //Text = string.Format("RAWS {0}::{1}", active, current);
                richTextBox1.AppendText(did + "\n");
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(AESEncrypt256(did, decrikey));
                textBox1.Clear();
                sw.Flush();
                sw.Close();
                fs.Close();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                richTextBox1_DoubleClick(sender, e);
            }
            realexit = true;
            notifyIcon1.Visible = false;
            this.Close();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = realname;
	javis=false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "��332HQ A.C.E 2015";
            notifyIcon1.BalloonTipText = "F1:����+\nF2:����-\nF3:�����ȯ\nF4:�Է��ϴ��� �����\nF5:��ȭ�����������\nF6:�Ǿ�ī��ó�� ��ݸ��(On/Off)\nF7:��� �缳��(������Ű �ʿ�, ���� �ʱ�ȭ)\nF8:��ȭ���� ��� �����\nF9:��ȭ���� ���� �����\nF10:�������α׷��� ���(ON/OFF)\nF11:Ʈ���������� �˸���� (ǳ��/�˾�)\nF12:Ʈ���̷�";
            notifyIcon1.ShowBalloonTip(2000);
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {

        }
        /*static void enc()
        {
            if (System.IO.File.Exists(realkey) && System.IO.File.Exists(path))
            {
                string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                string[] contents = System.IO.File.ReadAllLines(path, Encoding.Default);
                string decrikey = AESDecrypt256(rkeyarr[0], secret);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < contents.Length; i++)
                {
                    if (string.IsNullOrEmpty(contents[i]))
                    {
                        break;
                    }
                    contents[i] = AESEncrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
                encrypted = true;
            }
        }
        static void dec()
        {
            if (System.IO.File.Exists(realkey) && System.IO.File.Exists(path))
            {
                string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                string[] contents = System.IO.File.ReadAllLines(path, Encoding.Default);
                string decrikey = AESDecrypt256(rkeyarr[0], secret);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < contents.Length; i++)
                {
                    if (string.IsNullOrEmpty(contents[i]))
                    {
                        break;
                    }
                    contents[i] = AESDecrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
                encrypted = false;
            }
        }*/
    }
}
