using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MediaDevices;

namespace Laba_4
{
    /// <summary>
    /// Класс для выполнения действий по перезагрузке
    /// </summary>
    class Manager
    {
        /// <summary>
        /// Метод получения устройств
        /// </summary>
        /// <returns>Список устройств</returns>
        public List<Usb> DeviseListCreate()
        {
            List<Usb> usbDevices = new List<Usb>();
            //Получение драйвера USB и MTP
            List<DriveInfo> diskDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable).ToList();
            List<MediaDevice> mtpDrives = MediaDevice.GetDevices().ToList();
            // We can't eject MTP and we dont have size of it, so we should work with it differently
            foreach (MediaDevice device in mtpDrives)
            {
                // Connect to our MTP device
                device.Connect();
                // If our device isn't usual USB
                if (device.DeviceType != DeviceType.Generic)
                {
                    // We add this device to list as MTP
                    usbDevices.Add(new Usb(device.FriendlyName, null, null, null, true));
                }
            }
            // After MTP we count and perform other USB devices
            foreach (DriveInfo drive in diskDrives)
            {
                // Add USB device to list and calculate sizes
                usbDevices.Add(new Usb(drive.Name, Convert(drive.TotalFreeSpace),
                    Convert(drive.TotalSize - drive.TotalFreeSpace),
                    Convert(drive.TotalSize), false));
            }
            return usbDevices;
        }

        /// <summary>
        /// Преобразование в MB
        /// </summary>
        /// <param name="value">Значение в байтах</param>
        /// <returns></returns>
        private string Convert(long value)
        {
            double megaBytes = (value / 1024) / 1024;
            return megaBytes.ToString() + " mb";
        }
    }
}
