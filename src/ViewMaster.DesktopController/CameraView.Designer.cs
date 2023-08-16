namespace ViewMaster.DesktopController
{
    partial class CameraView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraView));
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)this.webView).BeginInit();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = Color.White;
            this.webView.Dock = DockStyle.Fill;
            this.webView.Location = new Point(0, 0);
            this.webView.Margin = new Padding(5, 5, 5, 5);
            this.webView.Name = "webView";
            this.webView.Size = new Size(1360, 738);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            // 
            // CameraView
            // 
            this.AutoScaleDimensions = new SizeF(17F, 41F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1360, 738);
            this.Controls.Add(this.webView);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(5, 5, 5, 5);
            this.Name = "CameraView";
            this.Text = "Camera View";
            this.WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)this.webView).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    }
}