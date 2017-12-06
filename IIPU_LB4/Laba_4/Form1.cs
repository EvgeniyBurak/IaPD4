using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_4
{
    /// <summary>
    /// Класс формы, где есть все важные методы для GUI
    /// </summary>
    public partial class Form1 : Form
    {
        // Constant for rewriting system method
        private const int WM_DEVICECHANGE = 0X219;
        private static readonly Manager _manager = new Manager();
        private List<Usb> _deviceList;
        //Our selecting table
        private readonly DataTable _table = new DataTable();

        // We follow for system messages
        protected override void WndProc(ref Message m)
        {
            // Redirect messages to our program
            base.WndProc(ref m);
            // Если конфигурация наших устройств изменилась
            if (m.Msg == WM_DEVICECHANGE)
            {
                // перезагрузка формы
                ReloadForm();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        // When we first load our form
        private void LoadForm(object sender, EventArgs e)
        {
            _deviceList = new List<Usb>();
            // We should make a template for our program
            _table.Columns.Add("Название", typeof(string));
            // Get all our devices
            ReloadForm();
            usbList.DataSource = _table;
            removeButton.Enabled = false;
            timer.Enabled = true;
        }
        // Method for reloading a program
        private void ReloadForm()
        {
            int currentPosition = 0;
            // Checking if our selected row is chosen
            if (usbList.CurrentRow != null)
            {
                currentPosition = usbList.CurrentRow.Index;
            }
            // Delete all past data
            _table.Clear();
            _deviceList = _manager.DeviseListCreate();
            foreach(Usb device in _deviceList)
            {
                _table.Rows.Add(device.DeviceName);
            }
            // If there are no index of bounds
            if (usbList.RowCount - 1 > currentPosition)
            {
                // Then we select this row
                usbList.Rows[currentPosition].Selected = true;
            }
            label1.Text = "";
        }

        // Timer for live reload every 5 seconds
        private void TickTimer(object sender, EventArgs e)
        {
            ReloadForm();
        }

        /// <summary>
        /// Событие для выбора некоторой строки в списке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSelect(object sender, EventArgs e)
        {
            // Если строка существует
            if (usbList.CurrentRow != null)
            {
                // Если нет индекса границ
                if (usbList.CurrentRow.Index >= 0 && usbList.CurrentRow.Index < _deviceList.Count)
                {
                    // Мы можем извлечь только USB, а не MTP
                    removeButton.Enabled = !_deviceList[usbList.CurrentRow.Index].IsMtpDevice;
                   
                    if (!_deviceList[usbList.CurrentRow.Index].IsMtpDevice)
                    {
                       
                        label6.Text = _deviceList[usbList.CurrentRow.Index].FreeSpace;
                        label7.Text = _deviceList[usbList.CurrentRow.Index].UsedSpace;
                        label8.Text = _deviceList[usbList.CurrentRow.Index].TotalSpace;

                    }
                }
                else
                {
                    // В других ситуациях просто блокируем
                    removeButton.Enabled = false;
                 
                }
            }
        }

        /// <summary>
        ///  Событие для нажатия кнопки удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnclickButton(object sender, EventArgs e)
        {
            // Если мы выберем, устройство которое мы хотим извлечь
            if (usbList.CurrentRow != null)
            {
                //Затем мы должны вызвать метод Eject_Device ()
                bool isEjected = _deviceList[usbList.CurrentRow.Index].EjectDevice();
                if (isEjected == false)
                {
                    MessageBox.Show("Устройство занято.");
                }
                else
                {
                   
                    label6.Text = "";
                    label7.Text = "";
                    label8.Text = "";
                }
            }
        }
    }
}
