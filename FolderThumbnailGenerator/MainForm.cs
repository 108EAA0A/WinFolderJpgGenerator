using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

#pragma warning disable CS0168

namespace FolderThumbnailGenerator
{
    public partial class MainForm : Form
    {
        #region field
        // 作業の進捗カウンタ
        int completeWorkNum = 0;

        SemaphoreSlim sems;

        bool p_working;

        // 作業開始前後に代入し、フォームのレイアウトを制御
        bool Working
        {
            set
            {
                p_working = value;
                this.button_Execution.Enabled = !value;
                this.label_progress.Visible = value;
            }
        }

        // 圧縮時の圧縮率(default:75)
        const long compressQuality = 75L;

        const int thumbnailNormalSidesSize = 200;

        // folder.jpgの作成元にすることを許可する拡張子一覧
        readonly string[] permissionImageExtList =
        {
            "jpg",
            "jpeg",
            "png",
            "gif",
            "bmp",
            "tiff",
            "cr2",
        };

        // app.configが無かったらこれを書き込む
        readonly string defaultAppConfig =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.7.2"" />
  </startup>
  <appSettings>
    <add key=""isOverwrite"" value=""True"" />
    <add key=""isRecursion"" value=""True"" />
    <add key=""RecursionDepth"" value=""-1"" />
    <add key=""isHiddenFile"" value=""True"" />
    <add key=""isImageCompress"" value=""True"" />
  </appSettings>
</configuration>
";
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        #region MainForm_Event
        void MainForm_Load(object sender, EventArgs e)
        {
            AssemblyName asmName = Assembly.GetExecutingAssembly().GetName();
            this.Text = asmName.Name + "        Version " + asmName.Version.ToString();

            // app.configの読み込み
            LoadSettings();

            // 再帰深度機能の初期化
            const string caption = "-1にすると無制限になります";
            this.toolTip_RecursionDepth.SetToolTip(this.label_RecursionDepth, caption);
            this.toolTip_RecursionDepth.SetToolTip(this.numericUpDown_RecursionDepth, caption);
            this.numericUpDown_RecursionDepth.Maximum = decimal.MaxValue;

            RecursionDepthFormStateUpdate();
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //アプリケーションの設定を保存する
            SaveSettings();
        }

        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            // ドラッグドロップ時にカーソルの形状を変更
            e.Effect = DragDropEffects.All;
        }

        void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルが渡されていなければ何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            // 渡されたファイルに対して処理を行う
            string dropPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            // ディレクトリならテキストボックスに代入
            if (File.GetAttributes(dropPath).HasFlag(FileAttributes.Directory))
            {
                this.textBox_DirectoryName.Text = dropPath;
            }
        }
        #endregion

        void LoadSettings()
        {
            try
            {
                this.checkBox_IsOverwrite.Checked = AppSettings.Get<bool>("isOverwrite");
                this.checkBox_IsRecursion.Checked = AppSettings.Get<bool>("isRecursion");
                this.numericUpDown_RecursionDepth.Value = AppSettings.Get<decimal>("RecursionDepth");
                this.checkBox_IsHiddenFile.Checked = AppSettings.Get<bool>("isHiddenFile");
                this.checkBox_IsImageCompress.Checked = AppSettings.Get<bool>("isImageCompress");
            }
            catch (AppSettingNotFoundException e)
            {
                // app.config無かったら生成
                File.WriteAllText($@"{Application.ExecutablePath}.Config", defaultAppConfig);
            }
        }

        void SaveSettings()
        {
            //Configurationの作成
            //ConfigurationUserLevel.Noneでアプリケーション構成ファイルを開く
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;
            settings["isOverwrite"].Value = this.checkBox_IsOverwrite.Checked.ToString();
            settings["isRecursion"].Value = this.checkBox_IsRecursion.Checked.ToString();
            settings["RecursionDepth"].Value = this.numericUpDown_RecursionDepth.Value.ToString();
            settings["isHiddenFile"].Value = this.checkBox_IsHiddenFile.Checked.ToString();
            settings["isImageCompress"].Value = this.checkBox_IsImageCompress.Checked.ToString();

            //保存する
            config.Save(ConfigurationSaveMode.Full);
        }

        bool IsImageFile(string path)
        {
            string fileExt = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(fileExt)) return false; // 拡張子のないファイルは除外
            fileExt = fileExt.Remove(0, 1); // 先頭のピリオドを削除

            // permissionImageExtListに存在するかを大文字小文字を区別しないで比較する
            return permissionImageExtList.Any(testExt => string.Compare(testExt, fileExt, true) == 0);
        }

        void AddAttributes(string path, FileAttributes attr)
        {
            FileAttributes fa = File.GetAttributes(path);
            if (fa.HasFlag(attr)) return;
            File.SetAttributes(path, fa | attr);
        }

        void RemoveAttributes(string path, FileAttributes attr)
        {
            var fa = File.GetAttributes(path);
            if (!fa.HasFlag(attr)) return;
            File.SetAttributes(path, fa & ~attr);
        }

        public static Size GetResizeSize(Size size) => GetResizeSize(size.Width, size.Height);
        public static Size GetResizeSize(int width, int height)
        {
            //if (src.Width <= thumbnailSize && src.Height <= thumbnailSize) ;

            // とりあえず縮小のみ実装

            var size = new Size(width, height);

            if (width > thumbnailNormalSidesSize || height > thumbnailNormalSidesSize)
            {
                // 小さい方がthumbnailNormalSidesSizeになるように合わせる
                double ratio = (thumbnailNormalSidesSize / (double)(width < height ? width : height));
                size.Width = (int)(width * ratio);
                size.Height = (int)(height * ratio);
            }

            return size;
        }

        int GetTotalAmountOfWork(string dirPath)
        {
            int sumWorkNum = 0;

            var thumbnailPath = $@"{dirPath}\folder.jpg";
            var sourcePath = Directory.GetFiles(dirPath)
                ?.FirstOrDefault(file => file != thumbnailPath && IsImageFile(file));

            if (sourcePath != null && !(File.Exists(thumbnailPath) && !this.checkBox_IsOverwrite.Checked))
            {
                ++sumWorkNum;
            }

            if (!this.checkBox_IsRecursion.Checked) return sumWorkNum;

            foreach (string subDirPath in Directory.GetDirectories(dirPath))
            {
                sumWorkNum += GetTotalAmountOfWork(subDirPath);
            }

            return sumWorkNum;
        }

        void CreateThumbnailRecursion(string dirPath)
        {
            if (sems == null)
            {
                CreateThumbnail(dirPath);
            }
            else
            {
                sems.Wait();
                Task.Run(() => {
                    CreateThumbnail(dirPath);
                    sems.Release();
                });
            }

            // 再帰処理許可判定
            if (this.checkBox_IsRecursion.Checked)
            {
                foreach (string subDirPath in Directory.GetDirectories(dirPath))
                {
                    CreateThumbnailRecursion(subDirPath);
                }
            }
        }

        void CreateThumbnail(string dirPath)
        {
            var thumbnailPath = $@"{dirPath}\folder.jpg";
            var isThumbnailExist = File.Exists(thumbnailPath);
            var sourcePath = Directory.GetFiles(dirPath)
                ?.FirstOrDefault(file => file != thumbnailPath && IsImageFile(file));


            // 拡張子一覧に合うファイルが存在しない
            if (sourcePath == null) return;
            // ファイルが既に存在し、上書きが許可されていない
            if (isThumbnailExist && !this.checkBox_IsOverwrite.Checked) return;


            // ディレクトリの読み取り専用解除
            RemoveAttributes(dirPath, FileAttributes.ReadOnly);

            // ファイルが既に存在し、上書きが許可されている場合
            if (isThumbnailExist && this.checkBox_IsOverwrite.Checked)
            {
                // 既存ファイルを消去
                File.Delete(thumbnailPath);
            }

            // 圧縮判定
            if (this.checkBox_IsImageCompress.Checked)
            {
                if (string.Compare(Path.GetExtension(sourcePath), ".CR2", true) == 0)
                {
                    CR2ToJPG.CR2Converter.ConvertImage(sourcePath, thumbnailPath, compressQuality, true);
                }
                else
                {
                    ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageEncoders()
                        .First(ici => ici.FormatID == ImageFormat.Jpeg.Guid);
                    EncoderParameters encParams = new EncoderParameters(1);
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, compressQuality);

                    using (var src = new Bitmap(sourcePath))
                    {
                        using (var dest = new Bitmap(src, GetResizeSize(src.Size)))
                        {
                            dest.Save(thumbnailPath, jpegEncoder, encParams);
                        }
                    }
                }
            }
            else
            {
                File.Copy(sourcePath, thumbnailPath, this.checkBox_IsOverwrite.Checked);
            }

            // 隠しファイル化判定
            // NormalはCopyでUnauthorizedAccessExceptionが出る対策になるらしい
            // ファイルの属性がないのはよくないため？
            AddAttributes(thumbnailPath,
                this.checkBox_IsHiddenFile.Checked
                    ? FileAttributes.Hidden | FileAttributes.System
                    : FileAttributes.Normal);

            // 作業進捗率を更新
            this.backgroundWorker.ReportProgress(Interlocked.Increment(ref completeWorkNum));
        }

        void button_DirectorySelect_Click(object sender, EventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog())
            {
                cofd.IsFolderPicker = true;
                if (this.textBox_DirectoryName.Text != null) cofd.DefaultFileName = this.textBox_DirectoryName.Text;
                if (cofd.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(cofd.FileName))
                {
                    this.textBox_DirectoryName.Text = cofd.FileName;
                }
            }
        }

        void button_Execution_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBox_DirectoryName.Text))
            {
                MessageBox.Show(@"作業フォルダを指定してください");
                return;
            }

            // BackgroundWorkerが処理中でなければ
            if (!this.backgroundWorker.IsBusy)
            {
                Working = true;
                this.progressBar.Maximum = GetTotalAmountOfWork(this.textBox_DirectoryName.Text);
                this.backgroundWorker.RunWorkerAsync();
            }
            else
            {
                // ボタンを押せなくしてるので来ないはずだが一応
                MessageBox.Show(@"作業中です");
            }
        }

        #region backgroundWorker
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs ev)
        {
            this.backgroundWorker.ReportProgress(0);

            using (sems = new SemaphoreSlim(Environment.ProcessorCount))
            {
                completeWorkNum = 0;

                if (this.progressBar.Maximum > 0)
                {
                    try
                    {
                        CreateThumbnailRecursion(this.textBox_DirectoryName.Text);
                    }
                    catch (Exception ex)
                    {
                        //ex.Message;
                        ev.Cancel = true;
                        this.backgroundWorker.ReportProgress(this.progressBar.Maximum);
                        return;
                    }
                }

                while (completeWorkNum < this.progressBar.Maximum) Task.Delay(500).Wait();
            }

            this.backgroundWorker.ReportProgress(this.progressBar.Maximum);
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            this.label_progress.Text = $@"{this.progressBar.Value} / {this.progressBar.Maximum}";
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            if (e.Cancelled)
            {
                MessageBox.Show(@"エラーが発生したため、処理を中断しました");
            }
            else
            {
                MessageBox.Show(@"終了しました");
            }
        }
        #endregion

        void checkBox_IsRecursion_CheckedChanged(object sender, EventArgs e) => RecursionDepthFormStateUpdate();

        void RecursionDepthFormStateUpdate()
        {
            bool state = this.checkBox_IsRecursion.Checked;
            this.label_RecursionDepth.Enabled = state;
            this.numericUpDown_RecursionDepth.Enabled = state;
        }
    }
}
