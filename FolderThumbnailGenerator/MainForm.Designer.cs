namespace FolderThumbnailGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_DirectorySelect = new System.Windows.Forms.Button();
            this.checkBox_IsRecursion = new System.Windows.Forms.CheckBox();
            this.textBox_DirectoryName = new System.Windows.Forms.TextBox();
            this.label_DirectoryName = new System.Windows.Forms.Label();
            this.checkBox_IsOverwrite = new System.Windows.Forms.CheckBox();
            this.checkBox_IsHiddenFile = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.button_Execution = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.checkBox_IsImageCompress = new System.Windows.Forms.CheckBox();
            this.label_progress = new System.Windows.Forms.Label();
            this.label_RecursionDepth = new System.Windows.Forms.Label();
            this.numericUpDown_RecursionDepth = new System.Windows.Forms.NumericUpDown();
            this.toolTip_RecursionDepth = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RecursionDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // button_DirectorySelect
            // 
            this.button_DirectorySelect.Location = new System.Drawing.Point(316, 25);
            this.button_DirectorySelect.Name = "button_DirectorySelect";
            this.button_DirectorySelect.Size = new System.Drawing.Size(75, 23);
            this.button_DirectorySelect.TabIndex = 0;
            this.button_DirectorySelect.Text = "選択";
            this.button_DirectorySelect.UseVisualStyleBackColor = true;
            this.button_DirectorySelect.Click += new System.EventHandler(this.button_DirectorySelect_Click);
            // 
            // checkBox_IsRecursion
            // 
            this.checkBox_IsRecursion.AutoSize = true;
            this.checkBox_IsRecursion.Checked = true;
            this.checkBox_IsRecursion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsRecursion.Location = new System.Drawing.Point(68, 97);
            this.checkBox_IsRecursion.Name = "checkBox_IsRecursion";
            this.checkBox_IsRecursion.Size = new System.Drawing.Size(168, 16);
            this.checkBox_IsRecursion.TabIndex = 1;
            this.checkBox_IsRecursion.Text = "サブディレクトリ以下も処理する";
            this.checkBox_IsRecursion.UseVisualStyleBackColor = true;
            this.checkBox_IsRecursion.CheckedChanged += new System.EventHandler(this.checkBox_IsRecursion_CheckedChanged);
            // 
            // textBox_DirectoryName
            // 
            this.textBox_DirectoryName.Location = new System.Drawing.Point(99, 27);
            this.textBox_DirectoryName.Name = "textBox_DirectoryName";
            this.textBox_DirectoryName.Size = new System.Drawing.Size(211, 19);
            this.textBox_DirectoryName.TabIndex = 2;
            // 
            // label_DirectoryName
            // 
            this.label_DirectoryName.AutoSize = true;
            this.label_DirectoryName.Location = new System.Drawing.Point(41, 30);
            this.label_DirectoryName.Name = "label_DirectoryName";
            this.label_DirectoryName.Size = new System.Drawing.Size(52, 12);
            this.label_DirectoryName.TabIndex = 3;
            this.label_DirectoryName.Text = "フォルダ名";
            // 
            // checkBox_IsOverwrite
            // 
            this.checkBox_IsOverwrite.AutoSize = true;
            this.checkBox_IsOverwrite.Checked = true;
            this.checkBox_IsOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOverwrite.Location = new System.Drawing.Point(68, 75);
            this.checkBox_IsOverwrite.Name = "checkBox_IsOverwrite";
            this.checkBox_IsOverwrite.Size = new System.Drawing.Size(199, 16);
            this.checkBox_IsOverwrite.TabIndex = 4;
            this.checkBox_IsOverwrite.Text = "folder.jpgが存在する場合上書きする";
            this.checkBox_IsOverwrite.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsHiddenFile
            // 
            this.checkBox_IsHiddenFile.AutoSize = true;
            this.checkBox_IsHiddenFile.Checked = true;
            this.checkBox_IsHiddenFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsHiddenFile.Location = new System.Drawing.Point(68, 120);
            this.checkBox_IsHiddenFile.Name = "checkBox_IsHiddenFile";
            this.checkBox_IsHiddenFile.Size = new System.Drawing.Size(107, 16);
            this.checkBox_IsHiddenFile.TabIndex = 5;
            this.checkBox_IsHiddenFile.Text = "隠しファイルにする";
            this.checkBox_IsHiddenFile.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(43, 213);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(348, 23);
            this.progressBar.TabIndex = 6;
            // 
            // button_Execution
            // 
            this.button_Execution.Location = new System.Drawing.Point(180, 163);
            this.button_Execution.Name = "button_Execution";
            this.button_Execution.Size = new System.Drawing.Size(75, 23);
            this.button_Execution.TabIndex = 7;
            this.button_Execution.Text = "実行";
            this.button_Execution.UseVisualStyleBackColor = true;
            this.button_Execution.Click += new System.EventHandler(this.button_Execution_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // checkBox_IsImageCompress
            // 
            this.checkBox_IsImageCompress.AutoSize = true;
            this.checkBox_IsImageCompress.Checked = true;
            this.checkBox_IsImageCompress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsImageCompress.Location = new System.Drawing.Point(68, 143);
            this.checkBox_IsImageCompress.Name = "checkBox_IsImageCompress";
            this.checkBox_IsImageCompress.Size = new System.Drawing.Size(100, 16);
            this.checkBox_IsImageCompress.TabIndex = 8;
            this.checkBox_IsImageCompress.Text = "画像を圧縮する";
            this.checkBox_IsImageCompress.UseVisualStyleBackColor = true;
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_progress.Location = new System.Drawing.Point(197, 192);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(43, 12);
            this.label_progress.TabIndex = 9;
            this.label_progress.Text = "0 / 100";
            this.label_progress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_progress.UseMnemonic = false;
            this.label_progress.Visible = false;
            // 
            // label_RecursionDepth
            // 
            this.label_RecursionDepth.AutoSize = true;
            this.label_RecursionDepth.Location = new System.Drawing.Point(314, 98);
            this.label_RecursionDepth.Name = "label_RecursionDepth";
            this.label_RecursionDepth.Size = new System.Drawing.Size(25, 12);
            this.label_RecursionDepth.TabIndex = 10;
            this.label_RecursionDepth.Text = "深さ";
            // 
            // numericUpDown_RecursionDepth
            // 
            this.numericUpDown_RecursionDepth.Location = new System.Drawing.Point(345, 96);
            this.numericUpDown_RecursionDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown_RecursionDepth.Name = "numericUpDown_RecursionDepth";
            this.numericUpDown_RecursionDepth.Size = new System.Drawing.Size(46, 19);
            this.numericUpDown_RecursionDepth.TabIndex = 11;
            this.numericUpDown_RecursionDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_RecursionDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.numericUpDown_RecursionDepth);
            this.Controls.Add(this.label_RecursionDepth);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.checkBox_IsImageCompress);
            this.Controls.Add(this.button_Execution);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.checkBox_IsHiddenFile);
            this.Controls.Add(this.checkBox_IsOverwrite);
            this.Controls.Add(this.label_DirectoryName);
            this.Controls.Add(this.textBox_DirectoryName);
            this.Controls.Add(this.checkBox_IsRecursion);
            this.Controls.Add(this.button_DirectorySelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "FolderThumbnailGenerator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RecursionDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_DirectorySelect;
        private System.Windows.Forms.CheckBox checkBox_IsRecursion;
        private System.Windows.Forms.TextBox textBox_DirectoryName;
        private System.Windows.Forms.Label label_DirectoryName;
        private System.Windows.Forms.CheckBox checkBox_IsOverwrite;
        private System.Windows.Forms.CheckBox checkBox_IsHiddenFile;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button button_Execution;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.CheckBox checkBox_IsImageCompress;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.Label label_RecursionDepth;
        private System.Windows.Forms.NumericUpDown numericUpDown_RecursionDepth;
        private System.Windows.Forms.ToolTip toolTip_RecursionDepth;
    }
}

