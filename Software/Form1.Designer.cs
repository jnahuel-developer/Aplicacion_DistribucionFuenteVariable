namespace DistribucionFuenteVariable
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEstadoDelPuerto = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCerrarPuerto = new System.Windows.Forms.Button();
            this.btnAbrirPuerto = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDetenerDatos = new System.Windows.Forms.Button();
            this.cbxV_Control = new System.Windows.Forms.CheckBox();
            this.cbxEnableFuenteVariable = new System.Windows.Forms.CheckBox();
            this.cbxEnablePeltierVacio = new System.Windows.Forms.CheckBox();
            this.cbxEnablePeltierRFFacial = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblVout_Var = new System.Windows.Forms.Label();
            this.lblI_V_RF = new System.Windows.Forms.Label();
            this.lblI_Peltier_Vacio = new System.Windows.Forms.Label();
            this.lblI_Peltier_Crio = new System.Windows.Forms.Label();
            this.lblI_HImFU = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRecibirDatos = new System.Windows.Forms.Button();
            this.cbxLed = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblEstadoDelPuerto);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCerrarPuerto);
            this.groupBox1.Controls.Add(this.btnAbrirPuerto);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controlar puerto";
            // 
            // lblEstadoDelPuerto
            // 
            this.lblEstadoDelPuerto.AutoSize = true;
            this.lblEstadoDelPuerto.Location = new System.Drawing.Point(60, 55);
            this.lblEstadoDelPuerto.Name = "lblEstadoDelPuerto";
            this.lblEstadoDelPuerto.Size = new System.Drawing.Size(45, 13);
            this.lblEstadoDelPuerto.TabIndex = 2;
            this.lblEstadoDelPuerto.Text = "Sin abrir";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Estado";
            // 
            // btnCerrarPuerto
            // 
            this.btnCerrarPuerto.Location = new System.Drawing.Point(100, 20);
            this.btnCerrarPuerto.Name = "btnCerrarPuerto";
            this.btnCerrarPuerto.Size = new System.Drawing.Size(90, 25);
            this.btnCerrarPuerto.TabIndex = 1;
            this.btnCerrarPuerto.Text = "Cerrar puerto";
            this.btnCerrarPuerto.UseVisualStyleBackColor = true;
            this.btnCerrarPuerto.Click += new System.EventHandler(this.btnCerrarPuerto_Click);
            // 
            // btnAbrirPuerto
            // 
            this.btnAbrirPuerto.Location = new System.Drawing.Point(5, 20);
            this.btnAbrirPuerto.Name = "btnAbrirPuerto";
            this.btnAbrirPuerto.Size = new System.Drawing.Size(90, 25);
            this.btnAbrirPuerto.TabIndex = 1;
            this.btnAbrirPuerto.Text = "Abrir puerto";
            this.btnAbrirPuerto.UseVisualStyleBackColor = true;
            this.btnAbrirPuerto.Click += new System.EventHandler(this.btnAbrirPuerto_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxLed);
            this.groupBox2.Controls.Add(this.btnDetenerDatos);
            this.groupBox2.Controls.Add(this.cbxV_Control);
            this.groupBox2.Controls.Add(this.cbxEnableFuenteVariable);
            this.groupBox2.Controls.Add(this.cbxEnablePeltierVacio);
            this.groupBox2.Controls.Add(this.cbxEnablePeltierRFFacial);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblVout_Var);
            this.groupBox2.Controls.Add(this.lblI_V_RF);
            this.groupBox2.Controls.Add(this.lblI_Peltier_Vacio);
            this.groupBox2.Controls.Add(this.lblI_Peltier_Crio);
            this.groupBox2.Controls.Add(this.lblI_HImFU);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnRecibirDatos);
            this.groupBox2.Location = new System.Drawing.Point(12, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 224);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos del micro";
            // 
            // btnDetenerDatos
            // 
            this.btnDetenerDatos.Enabled = false;
            this.btnDetenerDatos.Location = new System.Drawing.Point(100, 30);
            this.btnDetenerDatos.Name = "btnDetenerDatos";
            this.btnDetenerDatos.Size = new System.Drawing.Size(90, 25);
            this.btnDetenerDatos.TabIndex = 20;
            this.btnDetenerDatos.Text = "Detener datos";
            this.btnDetenerDatos.UseVisualStyleBackColor = true;
            this.btnDetenerDatos.Click += new System.EventHandler(this.btnDetenerDatos_Click);
            // 
            // cbxV_Control
            // 
            this.cbxV_Control.AutoSize = true;
            this.cbxV_Control.Location = new System.Drawing.Point(200, 159);
            this.cbxV_Control.Name = "cbxV_Control";
            this.cbxV_Control.Size = new System.Drawing.Size(69, 17);
            this.cbxV_Control.TabIndex = 19;
            this.cbxV_Control.Text = "V Control";
            this.cbxV_Control.UseVisualStyleBackColor = true;
            this.cbxV_Control.CheckedChanged += new System.EventHandler(this.cbxV_Control_CheckedChanged);
            // 
            // cbxEnableFuenteVariable
            // 
            this.cbxEnableFuenteVariable.AutoSize = true;
            this.cbxEnableFuenteVariable.Location = new System.Drawing.Point(200, 129);
            this.cbxEnableFuenteVariable.Name = "cbxEnableFuenteVariable";
            this.cbxEnableFuenteVariable.Size = new System.Drawing.Size(136, 17);
            this.cbxEnableFuenteVariable.TabIndex = 18;
            this.cbxEnableFuenteVariable.Text = "Enable Fuente Variable";
            this.cbxEnableFuenteVariable.UseVisualStyleBackColor = true;
            this.cbxEnableFuenteVariable.CheckedChanged += new System.EventHandler(this.cbxEnableFuenteVariable_CheckedChanged);
            // 
            // cbxEnablePeltierVacio
            // 
            this.cbxEnablePeltierVacio.AutoSize = true;
            this.cbxEnablePeltierVacio.Location = new System.Drawing.Point(200, 99);
            this.cbxEnablePeltierVacio.Name = "cbxEnablePeltierVacio";
            this.cbxEnablePeltierVacio.Size = new System.Drawing.Size(121, 17);
            this.cbxEnablePeltierVacio.TabIndex = 17;
            this.cbxEnablePeltierVacio.Text = "Enable Peltier Vacio";
            this.cbxEnablePeltierVacio.UseVisualStyleBackColor = true;
            this.cbxEnablePeltierVacio.CheckedChanged += new System.EventHandler(this.cbxEnablePeltierVacio_CheckedChanged);
            // 
            // cbxEnablePeltierRFFacial
            // 
            this.cbxEnablePeltierRFFacial.AutoSize = true;
            this.cbxEnablePeltierRFFacial.Location = new System.Drawing.Point(200, 69);
            this.cbxEnablePeltierRFFacial.Name = "cbxEnablePeltierRFFacial";
            this.cbxEnablePeltierRFFacial.Size = new System.Drawing.Size(139, 17);
            this.cbxEnablePeltierRFFacial.TabIndex = 16;
            this.cbxEnablePeltierRFFacial.Text = "Enable Peltier RF Facial";
            this.cbxEnablePeltierRFFacial.UseVisualStyleBackColor = true;
            this.cbxEnablePeltierRFFacial.CheckedChanged += new System.EventHandler(this.cbxEnablePeltierRFFacial_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(135, 190);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "V";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(135, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(135, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "A";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(135, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "A";
            // 
            // lblVout_Var
            // 
            this.lblVout_Var.AutoSize = true;
            this.lblVout_Var.Location = new System.Drawing.Point(100, 190);
            this.lblVout_Var.Name = "lblVout_Var";
            this.lblVout_Var.Size = new System.Drawing.Size(34, 13);
            this.lblVout_Var.TabIndex = 10;
            this.lblVout_Var.Text = "0.000";
            // 
            // lblI_V_RF
            // 
            this.lblI_V_RF.AutoSize = true;
            this.lblI_V_RF.Location = new System.Drawing.Point(100, 160);
            this.lblI_V_RF.Name = "lblI_V_RF";
            this.lblI_V_RF.Size = new System.Drawing.Size(34, 13);
            this.lblI_V_RF.TabIndex = 9;
            this.lblI_V_RF.Text = "0.000";
            // 
            // lblI_Peltier_Vacio
            // 
            this.lblI_Peltier_Vacio.AutoSize = true;
            this.lblI_Peltier_Vacio.Location = new System.Drawing.Point(100, 130);
            this.lblI_Peltier_Vacio.Name = "lblI_Peltier_Vacio";
            this.lblI_Peltier_Vacio.Size = new System.Drawing.Size(34, 13);
            this.lblI_Peltier_Vacio.TabIndex = 8;
            this.lblI_Peltier_Vacio.Text = "0.000";
            // 
            // lblI_Peltier_Crio
            // 
            this.lblI_Peltier_Crio.AutoSize = true;
            this.lblI_Peltier_Crio.Location = new System.Drawing.Point(100, 100);
            this.lblI_Peltier_Crio.Name = "lblI_Peltier_Crio";
            this.lblI_Peltier_Crio.Size = new System.Drawing.Size(34, 13);
            this.lblI_Peltier_Crio.TabIndex = 7;
            this.lblI_Peltier_Crio.Text = "0.000";
            // 
            // lblI_HImFU
            // 
            this.lblI_HImFU.AutoSize = true;
            this.lblI_HImFU.Location = new System.Drawing.Point(100, 70);
            this.lblI_HImFU.Name = "lblI_HImFU";
            this.lblI_HImFU.Size = new System.Drawing.Size(34, 13);
            this.lblI_HImFU.TabIndex = 6;
            this.lblI_HImFU.Text = "0.000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Vout_Var";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "I_V_RF";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "I_Peltier_Vacio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "I_Peltier_Crio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "I_HImFU";
            // 
            // btnRecibirDatos
            // 
            this.btnRecibirDatos.Enabled = false;
            this.btnRecibirDatos.Location = new System.Drawing.Point(5, 30);
            this.btnRecibirDatos.Name = "btnRecibirDatos";
            this.btnRecibirDatos.Size = new System.Drawing.Size(90, 25);
            this.btnRecibirDatos.TabIndex = 0;
            this.btnRecibirDatos.Text = "Recibir datos";
            this.btnRecibirDatos.UseVisualStyleBackColor = true;
            this.btnRecibirDatos.Click += new System.EventHandler(this.btnRecibirDatos_Click);
            // 
            // cbxLed
            // 
            this.cbxLed.AutoSize = true;
            this.cbxLed.Location = new System.Drawing.Point(200, 189);
            this.cbxLed.Name = "cbxLed";
            this.cbxLed.Size = new System.Drawing.Size(70, 17);
            this.cbxLed.TabIndex = 21;
            this.cbxLed.Text = "LED D21";
            this.cbxLed.UseVisualStyleBackColor = true;
            this.cbxLed.CheckedChanged += new System.EventHandler(this.cbxLed_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 334);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblEstadoDelPuerto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCerrarPuerto;
        private System.Windows.Forms.Button btnAbrirPuerto;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVout_Var;
        private System.Windows.Forms.Label lblI_V_RF;
        private System.Windows.Forms.Label lblI_Peltier_Vacio;
        private System.Windows.Forms.Label lblI_Peltier_Crio;
        private System.Windows.Forms.Label lblI_HImFU;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRecibirDatos;
        private System.Windows.Forms.CheckBox cbxV_Control;
        private System.Windows.Forms.CheckBox cbxEnableFuenteVariable;
        private System.Windows.Forms.CheckBox cbxEnablePeltierVacio;
        private System.Windows.Forms.CheckBox cbxEnablePeltierRFFacial;
        private System.Windows.Forms.Button btnDetenerDatos;
        private System.Windows.Forms.CheckBox cbxLed;
    }
}

