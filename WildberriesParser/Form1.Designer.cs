namespace WildberriesParser
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.searchButton = new System.Windows.Forms.Button();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.WBCountTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WBPictureBox = new System.Windows.Forms.PictureBox();
            this.ozonPictureBox = new System.Windows.Forms.PictureBox();
            this.beruPictureBox = new System.Windows.Forms.PictureBox();
            this.WBCheckBox = new System.Windows.Forms.CheckBox();
            this.ozonCheckBox = new System.Windows.Forms.CheckBox();
            this.beruCheckBox = new System.Windows.Forms.CheckBox();
            this.WBComboBox = new System.Windows.Forms.ComboBox();
            this.ozonComboBox = new System.Windows.Forms.ComboBox();
            this.beruComboBox = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.beruCountTextBox = new System.Windows.Forms.TextBox();
            this.ozonCountTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.WBProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.ozonProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.beruProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.commonProgressBar = new System.Windows.Forms.ProgressBar();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.WBPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ozonPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beruPictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.searchButton.ForeColor = System.Drawing.Color.Black;
            this.searchButton.Location = new System.Drawing.Point(775, 5);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(108, 38);
            this.searchButton.TabIndex = 0;
            this.searchButton.Text = "Поиск";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // queryTextBox
            // 
            this.queryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryTextBox.Location = new System.Drawing.Point(5, 5);
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.Size = new System.Drawing.Size(764, 38);
            this.queryTextBox.TabIndex = 1;
            this.queryTextBox.Text = "Введите запрос...";
            this.queryTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.queryTextBox_KeyDown);
            // 
            // WBCountTextBox
            // 
            this.WBCountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBCountTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WBCountTextBox.Location = new System.Drawing.Point(158, 297);
            this.WBCountTextBox.Name = "WBCountTextBox";
            this.WBCountTextBox.Size = new System.Drawing.Size(231, 32);
            this.WBCountTextBox.TabIndex = 5;
            this.WBCountTextBox.Text = "Все";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 297);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "Количество";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WBPictureBox
            // 
            this.WBPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WBPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("WBPictureBox.Image")));
            this.WBPictureBox.Location = new System.Drawing.Point(158, 4);
            this.WBPictureBox.Name = "WBPictureBox";
            this.WBPictureBox.Size = new System.Drawing.Size(231, 169);
            this.WBPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WBPictureBox.TabIndex = 7;
            this.WBPictureBox.TabStop = false;
            this.WBPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WBPictureBox_MouseClick);
            // 
            // ozonPictureBox
            // 
            this.ozonPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ozonPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ozonPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("ozonPictureBox.Image")));
            this.ozonPictureBox.Location = new System.Drawing.Point(396, 4);
            this.ozonPictureBox.Name = "ozonPictureBox";
            this.ozonPictureBox.Size = new System.Drawing.Size(231, 169);
            this.ozonPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ozonPictureBox.TabIndex = 8;
            this.ozonPictureBox.TabStop = false;
            this.ozonPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ozonPictureBox_MouseClick);
            // 
            // beruPictureBox
            // 
            this.beruPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.beruPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beruPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("beruPictureBox.Image")));
            this.beruPictureBox.Location = new System.Drawing.Point(634, 4);
            this.beruPictureBox.Name = "beruPictureBox";
            this.beruPictureBox.Size = new System.Drawing.Size(232, 169);
            this.beruPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.beruPictureBox.TabIndex = 9;
            this.beruPictureBox.TabStop = false;
            this.beruPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.beruPictureBox_MouseClick);
            // 
            // WBCheckBox
            // 
            this.WBCheckBox.AutoSize = true;
            this.WBCheckBox.Location = new System.Drawing.Point(3, 6);
            this.WBCheckBox.Name = "WBCheckBox";
            this.WBCheckBox.Size = new System.Drawing.Size(18, 17);
            this.WBCheckBox.TabIndex = 13;
            this.WBCheckBox.UseVisualStyleBackColor = true;
            // 
            // ozonCheckBox
            // 
            this.ozonCheckBox.AutoSize = true;
            this.ozonCheckBox.Location = new System.Drawing.Point(3, 3);
            this.ozonCheckBox.Name = "ozonCheckBox";
            this.ozonCheckBox.Size = new System.Drawing.Size(18, 17);
            this.ozonCheckBox.TabIndex = 14;
            this.ozonCheckBox.UseVisualStyleBackColor = true;
            // 
            // beruCheckBox
            // 
            this.beruCheckBox.AutoSize = true;
            this.beruCheckBox.Location = new System.Drawing.Point(15, 16);
            this.beruCheckBox.Name = "beruCheckBox";
            this.beruCheckBox.Size = new System.Drawing.Size(18, 17);
            this.beruCheckBox.TabIndex = 15;
            this.beruCheckBox.UseVisualStyleBackColor = true;
            // 
            // WBComboBox
            // 
            this.WBComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBComboBox.DropDownHeight = 126;
            this.WBComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WBComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WBComboBox.IntegralHeight = false;
            this.WBComboBox.ItemHeight = 26;
            this.WBComboBox.Items.AddRange(new object[] {
            "Популярность",
            "Рейтинг",
            "Цена 🠕",
            "Цена 🠗",
            "Скидка",
            "Обновление"});
            this.WBComboBox.Location = new System.Drawing.Point(158, 180);
            this.WBComboBox.Name = "WBComboBox";
            this.WBComboBox.Size = new System.Drawing.Size(231, 34);
            this.WBComboBox.TabIndex = 16;
            // 
            // ozonComboBox
            // 
            this.ozonComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ozonComboBox.DropDownHeight = 216;
            this.ozonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ozonComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ozonComboBox.FormattingEnabled = true;
            this.ozonComboBox.IntegralHeight = false;
            this.ozonComboBox.Items.AddRange(new object[] {
            "Популярность",
            "Новинки",
            "Цена 🠕",
            "Цена 🠗",
            "Скидка",
            "Рейтинг"});
            this.ozonComboBox.Location = new System.Drawing.Point(396, 180);
            this.ozonComboBox.Name = "ozonComboBox";
            this.ozonComboBox.Size = new System.Drawing.Size(231, 34);
            this.ozonComboBox.TabIndex = 17;
            // 
            // beruComboBox
            // 
            this.beruComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beruComboBox.DropDownHeight = 216;
            this.beruComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.beruComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beruComboBox.FormattingEnabled = true;
            this.beruComboBox.IntegralHeight = false;
            this.beruComboBox.Items.AddRange(new object[] {
            "Популярность",
            "Цена 🠕",
            "Цена 🠗",
            "Оценка",
            "Отзывы",
            "Скидка"});
            this.beruComboBox.Location = new System.Drawing.Point(634, 180);
            this.beruComboBox.Name = "beruComboBox";
            this.beruComboBox.Size = new System.Drawing.Size(232, 34);
            this.beruComboBox.TabIndex = 18;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 7000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Перейти по ссылке";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.beruCountTextBox, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.WBComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ozonComboBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ozonCountTextBox, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.ozonPictureBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.WBCountTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.beruComboBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.beruPictureBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.WBPictureBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(870, 425);
            this.tableLayoutPanel1.TabIndex = 19;
            this.tableLayoutPanel1.SizeChanged += new System.EventHandler(this.tableLayoutPanel1_SizeChanged);
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // beruCountTextBox
            // 
            this.beruCountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beruCountTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beruCountTextBox.Location = new System.Drawing.Point(634, 297);
            this.beruCountTextBox.Name = "beruCountTextBox";
            this.beruCountTextBox.Size = new System.Drawing.Size(232, 32);
            this.beruCountTextBox.TabIndex = 25;
            this.beruCountTextBox.Text = "Все";
            // 
            // ozonCountTextBox
            // 
            this.ozonCountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ozonCountTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ozonCountTextBox.Location = new System.Drawing.Point(396, 297);
            this.ozonCountTextBox.Name = "ozonCountTextBox";
            this.ozonCountTextBox.Size = new System.Drawing.Size(231, 32);
            this.ozonCountTextBox.TabIndex = 24;
            this.ozonCountTextBox.Text = "Все";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 380);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 41);
            this.label6.TabIndex = 23;
            this.label6.Text = "Общий";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 221);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 69);
            this.label4.TabIndex = 21;
            this.label4.Text = "Активен";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 336);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 37);
            this.label5.TabIndex = 22;
            this.label5.Text = "Прогресс";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 169);
            this.label2.TabIndex = 19;
            this.label2.Text = "Маркетплейс";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 34);
            this.label3.TabIndex = 20;
            this.label3.Text = "Сортировка";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.WBCheckBox);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(158, 221);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 69);
            this.panel1.TabIndex = 26;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ozonCheckBox);
            this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(396, 221);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(231, 69);
            this.panel2.TabIndex = 27;
            this.panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.beruCheckBox);
            this.panel3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(634, 221);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(232, 69);
            this.panel3.TabIndex = 28;
            this.panel3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.WBProgressBar);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(158, 336);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(231, 37);
            this.panel4.TabIndex = 29;
            this.panel4.SizeChanged += new System.EventHandler(this.panel4_SizeChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(170, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 26);
            this.label7.TabIndex = 0;
            this.label7.Text = "0%";
            this.label7.SizeChanged += new System.EventHandler(this.panel4_SizeChanged);
            // 
            // WBProgressBar
            // 
            this.WBProgressBar.Location = new System.Drawing.Point(37, 5);
            this.WBProgressBar.Name = "WBProgressBar";
            this.WBProgressBar.Size = new System.Drawing.Size(61, 25);
            this.WBProgressBar.Step = 1;
            this.WBProgressBar.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.ozonProgressBar);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(396, 336);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(231, 37);
            this.panel5.TabIndex = 30;
            this.panel5.SizeChanged += new System.EventHandler(this.panel5_SizeChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(168, 5);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 26);
            this.label8.TabIndex = 0;
            this.label8.Text = "0%";
            this.label8.SizeChanged += new System.EventHandler(this.panel5_SizeChanged);
            // 
            // ozonProgressBar
            // 
            this.ozonProgressBar.Location = new System.Drawing.Point(39, 12);
            this.ozonProgressBar.Name = "ozonProgressBar";
            this.ozonProgressBar.Size = new System.Drawing.Size(111, 22);
            this.ozonProgressBar.Step = 1;
            this.ozonProgressBar.TabIndex = 10;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.beruProgressBar);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(634, 336);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(232, 37);
            this.panel6.TabIndex = 31;
            this.panel6.SizeChanged += new System.EventHandler(this.panel6_SizeChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(117, 5);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 26);
            this.label9.TabIndex = 0;
            this.label9.Text = "0%";
            this.label9.SizeChanged += new System.EventHandler(this.panel6_SizeChanged);
            // 
            // beruProgressBar
            // 
            this.beruProgressBar.Location = new System.Drawing.Point(5, 19);
            this.beruProgressBar.Name = "beruProgressBar";
            this.beruProgressBar.Size = new System.Drawing.Size(106, 15);
            this.beruProgressBar.Step = 1;
            this.beruProgressBar.TabIndex = 11;
            // 
            // panel7
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel7, 3);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.commonProgressBar);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(158, 380);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(708, 41);
            this.panel7.TabIndex = 32;
            this.panel7.SizeChanged += new System.EventHandler(this.panel7_SizeChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 26);
            this.label10.TabIndex = 0;
            this.label10.Text = "0%";
            this.label10.SizeChanged += new System.EventHandler(this.panel7_SizeChanged);
            // 
            // commonProgressBar
            // 
            this.commonProgressBar.Location = new System.Drawing.Point(66, 9);
            this.commonProgressBar.Name = "commonProgressBar";
            this.commonProgressBar.Size = new System.Drawing.Size(403, 24);
            this.commonProgressBar.Step = 1;
            this.commonProgressBar.TabIndex = 12;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Книга Excel (*.xlsx)|*.xlsx";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "Сохранить";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(892, 493);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.queryTextBox);
            this.Controls.Add(this.searchButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(910, 540);
            this.Name = "Form1";
            this.Text = "Marketplace Parser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.WBPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ozonPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beruPictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.ProgressBar WBProgressBar;
        private System.Windows.Forms.TextBox WBCountTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox WBPictureBox;
        private System.Windows.Forms.PictureBox ozonPictureBox;
        private System.Windows.Forms.PictureBox beruPictureBox;
        private System.Windows.Forms.ProgressBar ozonProgressBar;
        private System.Windows.Forms.ProgressBar beruProgressBar;
        private System.Windows.Forms.ProgressBar commonProgressBar;
        private System.Windows.Forms.CheckBox WBCheckBox;
        private System.Windows.Forms.CheckBox ozonCheckBox;
        private System.Windows.Forms.CheckBox beruCheckBox;
        private System.Windows.Forms.ComboBox WBComboBox;
        private System.Windows.Forms.ComboBox ozonComboBox;
        private System.Windows.Forms.ComboBox beruComboBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox beruCountTextBox;
        private System.Windows.Forms.TextBox ozonCountTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label10;
    }
}

