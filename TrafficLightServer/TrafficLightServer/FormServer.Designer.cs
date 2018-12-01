namespace TrafficLightServer
{
    partial class FormServer
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
            this.components = new System.ComponentModel.Container();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.startTemplateButton = new System.Windows.Forms.Button();
            this.trafficLightsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.templateComboBox = new System.Windows.Forms.ComboBox();
            this.stopTemplateButton = new System.Windows.Forms.Button();
            this.disconnectAllClientsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(11, 11);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(176, 31);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect to sort of proxy";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.Location = new System.Drawing.Point(11, 207);
            this.listBoxOutput.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(315, 238);
            this.listBoxOutput.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(207, 18);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(50, 18);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // startTemplateButton
            // 
            this.startTemplateButton.Enabled = false;
            this.startTemplateButton.Location = new System.Drawing.Point(11, 145);
            this.startTemplateButton.Name = "startTemplateButton";
            this.startTemplateButton.Size = new System.Drawing.Size(106, 23);
            this.startTemplateButton.TabIndex = 9;
            this.startTemplateButton.Text = "Start";
            this.startTemplateButton.UseVisualStyleBackColor = true;
            this.startTemplateButton.Click += new System.EventHandler(this.startTemplateButton_Click);
            // 
            // trafficLightsPanel
            // 
            this.trafficLightsPanel.BackColor = System.Drawing.Color.White;
            this.trafficLightsPanel.Location = new System.Drawing.Point(331, 75);
            this.trafficLightsPanel.Name = "trafficLightsPanel";
            this.trafficLightsPanel.Size = new System.Drawing.Size(479, 370);
            this.trafficLightsPanel.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Template";
            // 
            // templateComboBox
            // 
            this.templateComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.templateComboBox.Enabled = false;
            this.templateComboBox.FormattingEnabled = true;
            this.templateComboBox.Items.AddRange(new object[] {
            "Crossroad"});
            this.templateComboBox.Location = new System.Drawing.Point(11, 111);
            this.templateComboBox.Name = "templateComboBox";
            this.templateComboBox.Size = new System.Drawing.Size(106, 21);
            this.templateComboBox.TabIndex = 12;
            this.templateComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.templateComboBox_DrawItem);
            this.templateComboBox.SelectedIndexChanged += new System.EventHandler(this.templateComboBox_SelectedIndexChanged);
            // 
            // stopTemplateButton
            // 
            this.stopTemplateButton.Enabled = false;
            this.stopTemplateButton.Location = new System.Drawing.Point(12, 174);
            this.stopTemplateButton.Name = "stopTemplateButton";
            this.stopTemplateButton.Size = new System.Drawing.Size(106, 23);
            this.stopTemplateButton.TabIndex = 13;
            this.stopTemplateButton.Text = "Stop";
            this.stopTemplateButton.UseVisualStyleBackColor = true;
            this.stopTemplateButton.Click += new System.EventHandler(this.stopTemplateButton_Click);
            // 
            // disconnectAllClientsButton
            // 
            this.disconnectAllClientsButton.Enabled = false;
            this.disconnectAllClientsButton.Location = new System.Drawing.Point(193, 109);
            this.disconnectAllClientsButton.Name = "disconnectAllClientsButton";
            this.disconnectAllClientsButton.Size = new System.Drawing.Size(118, 23);
            this.disconnectAllClientsButton.TabIndex = 14;
            this.disconnectAllClientsButton.Text = "Disconnect all Clients";
            this.disconnectAllClientsButton.UseVisualStyleBackColor = true;
            this.disconnectAllClientsButton.Click += new System.EventHandler(this.disconnectAllClientsButton_Click);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 454);
            this.Controls.Add(this.disconnectAllClientsButton);
            this.Controls.Add(this.stopTemplateButton);
            this.Controls.Add(this.templateComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trafficLightsPanel);
            this.Controls.Add(this.startTemplateButton);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.buttonConnect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormServer";
            this.Text = "Server (sort of)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServer_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button startTemplateButton;
        private System.Windows.Forms.Panel trafficLightsPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox templateComboBox;
        private System.Windows.Forms.Button stopTemplateButton;
        private System.Windows.Forms.Button disconnectAllClientsButton;
    }
}

