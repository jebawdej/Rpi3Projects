using PhoebeCoeus.IoT.RaspberryUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PCF8591_I2C_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Timer periodicTimer;
        PCF8591 ADConverter;
        public MainPage()
        {
            this.InitializeComponent();
            InitI2CPcf8591();
        }

        private async void InitI2CPcf8591()
        {
            ADConverter = await PCF8591.Create();
            double value = ADConverter.ReadI2CAnalog_AsDouble(PCF8591_AnalogPin.A0);
            /* Now that everything is initialized, create a timer so we read data every 100mS */
            periodicTimer = new System.Threading.Timer(this.TimerCallback, null, 0, 100);
        }

        private void TimerCallback(object state)
        {
            string A0Text, A1Text, A2Text, A3Text;
            string statusText;

            /* Read and format accelerometer data */
            try
            {
                int value = ADConverter.ReadI2CAnalog(PCF8591_AnalogPin.A0);
                A0Text = String.Format("A0: {0:000}", value);
                value = ADConverter.ReadI2CAnalog(PCF8591_AnalogPin.A1);
                A1Text = String.Format("A1: {0:000}", value);
                value = ADConverter.ReadI2CAnalog(PCF8591_AnalogPin.A2);
                A2Text = String.Format("A2: {0:000}", value);
                value = ADConverter.ReadI2CAnalog(PCF8591_AnalogPin.A3);
                A3Text = String.Format("A3: {0:000}", value);
                statusText = "Status: Running";
            }
            catch (Exception ex)
            {
                A0Text = "A0: Error";
                A1Text = "A1: Error";
                A2Text = "A2: Error";
                A3Text = "A3: Error";
                statusText = "Failed to read from PCF8591: " + ex.Message;
            }

            /* UI updates must be invoked on the UI thread */
            var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Text_A0_Value.Text = A0Text;
                Text_A1_Value.Text = A1Text;
                Text_A2_Value.Text = A2Text;
                Text_A3_Value.Text = A3Text;
                Text_Status.Text = statusText;
            });
        }
    }
}
