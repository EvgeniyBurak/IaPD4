using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbEject;

namespace Laba_4
{
    /// <summary>
    /// Общий класс для USB-устройств
    /// </summary>
    class Usb
    {
        /// <summary>
        /// Название устройства
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// Cвободное пространство на диске
        /// </summary>
        public string FreeSpace { get; set; }
        /// <summary>
        /// Используемое пространство на диске
        /// </summary>
        public string UsedSpace { get; set; }
        /// <summary>
        /// Общее постранство на диске
        /// </summary>
        public string TotalSpace { get; set; }
        /// <summary>
        /// Проверка устройства (Mtp)
        /// </summary>
        public bool IsMtpDevice { get; set; }
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="name">Имя устройства</param>
        /// <param name="freeSize">Свободное пространство на диске</param>
        /// <param name="usedSize">Используемое пространство на диске</param>
        /// <param name="totalSize">Общее пространство на диске</param>
        /// <param name="check">Проверка устройства</param>
        public Usb(string name, string freeSize, string usedSize, string totalSize, bool check)
        {
            DeviceName = name;
            FreeSpace = freeSize;
            UsedSpace = usedSize;
            TotalSpace = totalSize;
            IsMtpDevice = check;
        }
        /// <summary>
        /// Метод извлечения устройства ( RemoveDrive )
        /// </summary>
        /// <returns>true - извлечено; false - не извлечено</returns>
     
        public bool EjectDevice()
        {
            var ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == this.DeviceName.Remove(2));
            ejectedDevice.Eject(true);
            ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == this.DeviceName.Remove(2));
           
            if (ejectedDevice == null)
                return true;
            else
                return false;
        }
    }
}
