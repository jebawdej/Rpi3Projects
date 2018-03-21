using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace EEProm_24C256
{
    public class EEprom24C256 : IDisposable
    {
        /// <summary>
        /// The address of the device is 0x90, coded on 7 bits.
        /// To get the matching value om 8 bits, shift (>>) the bits by 1.
        /// </summary>
        private const byte addr_24C256 = (0x50); //(01010A2A1A0)
        private I2cDevice device;

        /// <summary>
        /// private constructor for internal use only.
        /// To instanciate a PCF8591 object, please use the static method PCF8591.Create();
        /// </summary>
        private EEprom24C256() { }
        /// <summary>
        /// Instanciate asyncronously a new PCF8591 object.
        /// </summary>
        /// <returns>A new PCF8591 object instance.</returns>
        public static Windows.Foundation.IAsyncOperation<EEprom24C256> Create()
        {
            return CreateAsync(I2cBusSpeed.StandardMode, I2cSharingMode.Exclusive).AsAsyncOperation();
        }

        /// <summary>
        /// Instanciate asyncronously a new PCF8591 object.
        /// </summary>
        /// <param name="BusSpeed">The I2C Bus Speed. Default value: StandardMode </param>
        /// <returns>A new PCF8591 object instance.</returns>
        public static Windows.Foundation.IAsyncOperation<EEprom24C256> Create(I2cBusSpeed BusSpeed)
        {
            return CreateAsync(BusSpeed, I2cSharingMode.Exclusive).AsAsyncOperation();
        }

        /// <summary>
        /// Instanciate asyncronously a new PCF8591 object.
        /// </summary>
        /// <param name="BusSpeed">The I2C Bus Speed. Default value: StandardMode </param>
        /// <param name="SharingMode">The I2C Sharing Mode. Default value is Exclusive. To use with caution </param>
        /// <returns>A new PCF8591 object instance.</returns>
        public static Windows.Foundation.IAsyncOperation<EEprom24C256> Create(I2cBusSpeed BusSpeed, I2cSharingMode SharingMode)
        {
            return CreateAsync(BusSpeed, SharingMode).AsAsyncOperation();
        }

        /// <summary>
        /// Instanciate asyncronously a new PCF8591 object.
        /// As System.Threading.Tasks.Task are not valid Windows Runtime type supported, this method has been set to private and is publicly exposed through the IAsyncOperation method "Create".
        /// </summary>
        /// <param name="BusSpeed">The I2C Bus Speed. Default value: StandardMode </param>
        /// <param name="SharingMode">The I2C Sharing Mode. Default value is Exclusive. To use with caution </param>
        /// <returns></returns>
        async static private Task<EEprom24C256> CreateAsync(I2cBusSpeed BusSpeed, I2cSharingMode SharingMode)
        {
            EEprom24C256 newEEprom = new EEprom24C256();
            /// advanced query syntax used to find devices on the RaspberryPi.
            string AQS = Windows.Devices.I2c.I2cDevice.GetDeviceSelector();
            var DevicesInfo = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(AQS);
            if (DevicesInfo.Count == 0) throw new Exception("No Device Information were found with query: " + AQS);
            // I2C bus settings
            var settings = new Windows.Devices.I2c.I2cConnectionSettings(addr_24C256);
            settings.BusSpeed = BusSpeed;
            settings.SharingMode = SharingMode;
            // Reteives the device from the I2C bus with the given ID.
            newEEprom.device = await Windows.Devices.I2c.I2cDevice.FromIdAsync(DevicesInfo[0].Id, settings);
            if (newEEprom.device == null) throw new Exception("No I2C Device were found with ID " + DevicesInfo[0].Id);
            return newEEprom;
        }

        public void Dispose()
        {
            this.device.Dispose();
        }

        /// <summary>
        /// Returns the byte value of the eeprom, specified by the address.
        /// </summary>
        /// <param name="Address">The address of the byte you will read</param>
        /// <returns></returns>
        public byte ReadI2CByte(Int16 address)
        {
            byte[] adr = ConvertInt32ToByteArray(address);
            byte[] b = new byte[1];
            device.WriteRead(adr, b);
            return b[0];
        }
        /// <summary>
        /// Returns the values of one page (64 bytes) of the eeprom, specified by the address. As byte array
        /// </summary>
        /// <param name="InputPin">The start address of the page you will read</param>
        /// <returns></returns>
        public byte ReadI2CPage(Int16 address)
        {
            byte[] adr = ConvertInt32ToByteArray(address);
            byte[] b = new byte[1];
            device.WriteRead(adr, b);
            return b[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">The address of the byte you will read</param>
        /// <returns></returns>
        int p;
        /// <summary>
        /// Write a byte value to the eeprom, on location specified by the address.
        /// </summary>
        /// <param name="address">Address of location to write the byte</param>
        /// <param name="value">The byte value to write to the eeprom</param>
        public void WriteI2CByte(Int16 address, byte value)
        {
            byte[] adr = ConvertInt32ToByteArray(address);
            byte[] b = new byte[3];
            b[0] = adr[0];
            b[1] = adr[1];
            b[2] = value;
            device.Write(b);
        }

        /// <summary>
        /// Write a buffer with values for one page (64 bytes) to the eeprom, specified by the address.
        /// </summary>
        /// <param name="address">Start addres of the page</param>
        /// <param name="buffer">buffer with values of one page as byte array</param>
        public void WriteI2CPage(Int16 address, byte[] buffer)
        {

            byte[] adr = ConvertInt32ToByteArray(address);
            byte[] b = new byte[buffer.Length+2];
            b[0] = adr[0];
            b[1] = adr[1];
            for(int i=0; i < buffer.Length;i++)
            {
                b[2 + i] = buffer[i];
            }
            device.Write(b);
        }

        private byte[] ConvertInt32ToByteArray(Int16 I16)
        {
            return BitConverter.GetBytes(I16);
        }

    }
}
