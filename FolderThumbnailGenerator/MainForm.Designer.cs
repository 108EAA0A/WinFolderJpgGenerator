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
            this.button_DirectorySelect = new System.Windows.Forms.Button();
            this.checkBox_IsRecursion = new System.Windows.Forms.CheckBox();
            this.textBox_DirectoryName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_IsOverwrite = new System.Windows.Forms.CheckBox();
            this.checkBox_IsHiddenFile = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.button_Execution = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // button_DirectorySelect
            // 
            this.button_DirectorySelect.Location = new System.Drawing.Point(312, 34);
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
            this.checkBox_IsRecursion.Location = new System.Drawing.Point(64, 106);
            this.checkBox_IsRecursion.Name = "checkBox_IsRecursion";
            this.checkBox_IsRecursion.Size = new System.Drawing.Size(199, 16);
            this.checkBox_IsRecursion.TabIndex = 1;
            this.checkBox_IsRecursion.Text = "サブディレクトリまで再帰的に処理する";
            this.checkBox_IsRecursion.UseVisualStyleBackColor = true;
            // 
            // textBox_DirectoryName
            // 
            this.textBox_DirectoryName.Location = new System.Drawing.Point(95, 36);
            this.textBox_DirectoryName.Name = "textBox_DirectoryName";
            this.textBox_DirectoryName.Size = new System.Drawing.Size(211, 19);
            this.textBox_DirectoryName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "フォルダ名";
            // 
            // checkBox_IsOverwrite
            // 
            this.checkBox_IsOverwrite.AutoSize = true;
            this.checkBox_IsOverwrite.Checked = true;
            this.checkBox_IsOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOverwrite.Location = new System.Drawing.Point(64, 84);
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
            this.checkBox_IsHiddenFile.Location = new System.Drawing.Point(64, 129);
            this.checkBox_IsHiddenFile.Name = "checkBox_IsHiddenFile";
            this.checkBox_IsHiddenFile.Size = new System.Drawing.Size(107, 16);
            this.checkBox_IsHiddenFile.TabIndex = 5;
            this.checkBox_IsHiddenFile.Text = "隠しファイルにする";
            this.checkBox_IsHiddenFile.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(39, 210);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(348, 23);
            this.progressBar.TabIndex = 6;
            // 
            // button_Execution
            // 
            this.button_Execution.Location = new System.Drawing.Point(167, 169);
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
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.button_Execution);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.checkBox_IsHiddenFile);
            this.Controls.Add(this.checkBox_IsOverwrite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_DirectoryName);
            this.Controls.Add(this.checkBox_IsRecursion);
            this.Controls.Add(this.button_DirectorySelect);
            this.Name = "MainForm";
            this.Text = "FolderThumbnailGenerator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_DirectorySelect;
        private System.Windows.Forms.CheckBox checkBox_IsRecursion;
        private System.Windows.Forms.TextBox textBox_DirectoryName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_IsOverwrite;
        private System.Windows.Forms.CheckBox checkBox_IsHiddenFile;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button button_Execution;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

