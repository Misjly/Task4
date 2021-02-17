namespace Task4.ServiceClient
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.ServiceProcess.ServiceInstaller _serviceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller _processInstaller;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _processInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            _serviceInstaller = new System.ServiceProcess.ServiceInstaller();


            _processInstaller.Account = System.ServiceProcess.ServiceAccount.User;
            _processInstaller.Username = @"DESKTOP-OQRTOO7\nikita";
            _processInstaller.Password = null;

            _serviceInstaller.DisplayName = "SaleService";
            _serviceInstaller.ServiceName = "SaleService";



            Installers.AddRange(new System.Configuration.Install.Installer[] {
            _processInstaller,
            _serviceInstaller});
        }

        #endregion
    }
}