using MultiFuncBoardDemo.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MultiFuncBoardDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MfbDemo : Page
    {
        private const int timerLedsTime = 500;
        //private const int timerLedDisplayTime = 1;
        //private int timerLedDisplayCounter;

        private const int LED_D1 = 19; //led D1
        private const int LED_D2 = 13; //led D2
        private const int LED_D3 = 6; //led D3
        private const int LED_D4 = 5; //led D4

        private const int BUZZER = 4;  //Buzzer

        private const int SWITCH_S1 = 17;  //Switch S1
        private const int SWITCH_S2 = 27;  //Switch S2
        private const int SWITCH_S3 = 22;  //Switch S3

        private const int SR_DATA = 10;  //Shift register DATA
        private const int SR_CLOCK = 9;  //Shift register CLOCK
        private const int SR_LATCH = 11; //Shift register LATCH

        private GpioPin pinD1;
        private GpioPin pinD2;
        private GpioPin pinD3;
        private GpioPin pinD4;

        private GpioPin pinBuzzer;

        private GpioPin pinDATA;
        private GpioPin pinCLOCK;
        private GpioPin pinLATCH;

        /* Segment byte maps for numbers 0 to 9 */
        private static readonly byte[] SEGMENT_MAP = new byte[] { 0xC0, 0xF9, 0xA4, 0xB0, 0x99, 0x92, 0x82, 0xF8, 0X80, 0X90 };
        /* Byte maps to select digit 1 to 4 */
        private static readonly byte[] SEGMENT_SELECT = new byte[] { 0xF1, 0xF2, 0xF4, 0xF8 };

        private GpioPin pinS1;
        private GpioPin pinS2;
        private GpioPin pinS3;

        //private GpioPinValue pinValue;
        private DispatcherTimer timerLeds;
        //private DispatcherTimer timerLedDisplay;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush greenBrush = new SolidColorBrush(Windows.UI.Colors.Green);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);
        private SolidColorBrush darkGrayBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
        private SolidColorBrush blueBrush = new SolidColorBrush(Windows.UI.Colors.Blue);

        private int StateMachineState;

        MfbDemoViewModel vm;
        public MfbDemo()
        {
            this.InitializeComponent();
            vm = (MfbDemoViewModel)DataContext;
            //vm.LedD1State = true;
            InitGPIO();
            timerLeds = new DispatcherTimer();
            timerLeds.Interval = TimeSpan.FromMilliseconds(timerLedsTime);
            timerLeds.Tick += TimerLeds_Tick;

            //timerLedDisplay = new DispatcherTimer();
            //timerLedDisplay.Interval = TimeSpan.FromMilliseconds(timerLedDisplayTime);
            //timerLedDisplay.Tick += timerLedDisplay_Tick;
            //timerLedDisplay.Start();

            if ((pinD1 != null) && (pinD2 != null) && (pinD3 != null) && (pinD4 != null))
            {
                timerLeds.Start();
            }
            //DelayText.Text = String.Format("timer interval = {0} ms", timerLedsTime);
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pinD1 = pinD1 = null;
                GpioStatus.Text = "GpioStatus: There is no GPIO controller on this device.";
                return;
            }
            //Initialize the GPIO Led outputs
            pinD1 = gpio.OpenPin(LED_D1);
            pinD2 = gpio.OpenPin(LED_D2);
            pinD3 = gpio.OpenPin(LED_D3);
            pinD4 = gpio.OpenPin(LED_D4);

            pinD1.Write(GpioPinValue.High);
            pinD2.Write(GpioPinValue.High);
            pinD3.Write(GpioPinValue.High);
            pinD4.Write(GpioPinValue.High);

            pinD1.SetDriveMode(GpioPinDriveMode.Output);
            pinD2.SetDriveMode(GpioPinDriveMode.Output);
            pinD3.SetDriveMode(GpioPinDriveMode.Output);
            pinD4.SetDriveMode(GpioPinDriveMode.Output);

            //Initialize the GPIO buzzer output
            pinBuzzer = gpio.OpenPin(BUZZER);
            pinBuzzer.Write(GpioPinValue.High);
            pinBuzzer.SetDriveMode(GpioPinDriveMode.Output);

            //Initialize the Shift Register pins
            pinDATA = gpio.OpenPin(SR_DATA);
            pinCLOCK = gpio.OpenPin(SR_CLOCK);
            pinLATCH = gpio.OpenPin(SR_LATCH);

            pinDATA.Write(GpioPinValue.High);
            pinCLOCK.Write(GpioPinValue.High);
            pinLATCH.Write(GpioPinValue.High);

            pinDATA.SetDriveMode(GpioPinDriveMode.Output);
            pinCLOCK.SetDriveMode(GpioPinDriveMode.Output);
            pinLATCH.SetDriveMode(GpioPinDriveMode.Output);

            //Initialize the GPIO Switch inputs
            pinS1 = gpio.OpenPin(SWITCH_S1);
            pinS2 = gpio.OpenPin(SWITCH_S2);
            pinS3 = gpio.OpenPin(SWITCH_S3);

            pinS1.SetDriveMode(GpioPinDriveMode.Input);
            pinS2.SetDriveMode(GpioPinDriveMode.Input);
            pinS3.SetDriveMode(GpioPinDriveMode.Input);

            pinS1.ValueChanged += Switch1_ValueChanged;
            pinS2.ValueChanged += Switch2_ValueChanged;
            pinS3.ValueChanged += Switch3_ValueChanged;


            GpioStatus.Text = "GpioStatus: GPIO pin initialized correctly.";

            StateMachineState = 0;


        }

        private void Switch1_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            this.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => {
                if (args.Edge == GpioPinEdge.RisingEdge)
                {
                    vm.SwitchS1Pressed = false;
                    pinBuzzer.Write(GpioPinValue.High);
                    vm.BuzzerSound = false;
                }
                else
                {
                    vm.SwitchS1Pressed = true;
                    pinBuzzer.Write(GpioPinValue.Low);
                    vm.BuzzerSound = true;
                }
            }).GetAwaiter().GetResult();

        }

        private void Switch2_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            this.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => {
                if (args.Edge == GpioPinEdge.RisingEdge)
                {
                    vm.SwitchS2Pressed = false;
                }
                else
                {
                    vm.SwitchS2Pressed = true;
                }
            }).GetAwaiter().GetResult();
        }
        private void Switch3_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            this.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => {
                if (args.Edge == GpioPinEdge.RisingEdge)
                {
                    vm.SwitchS3Pressed = false;
                }
                else
                {
                    vm.SwitchS3Pressed = true;
                }
            }).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Timer_Tick from DispatcherTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerLeds_Tick(object sender, object e)
        {
            switch (StateMachineState)
            {
                case 1:
                    WriteDigit(SEGMENT_MAP[2], SEGMENT_SELECT[1]);
                    vm.LedDisplayTxt = " 2  ";
                    pinD1.Write(GpioPinValue.High);
                    pinD2.Write(GpioPinValue.Low);
                    pinD3.Write(GpioPinValue.High);
                    pinD4.Write(GpioPinValue.High);
                    vm.LedD1State = false;
                    vm.LedD2State = true;
                    vm.LedD3State = false;
                    vm.LedD4State = false;
                    break;
                case 2:
                    WriteDigit(SEGMENT_MAP[3], SEGMENT_SELECT[2]);
                    vm.LedDisplayTxt = "  3 ";
                    pinD1.Write(GpioPinValue.High);
                    pinD2.Write(GpioPinValue.High);
                    pinD3.Write(GpioPinValue.Low);
                    pinD4.Write(GpioPinValue.High);
                    vm.LedD1State = false;
                    vm.LedD2State = false;
                    vm.LedD3State = true;
                    vm.LedD4State = false;
                    break;
                case 3:
                    WriteDigit(SEGMENT_MAP[4], SEGMENT_SELECT[3]);
                    vm.LedDisplayTxt = "   4";
                    pinD1.Write(GpioPinValue.High);
                    pinD2.Write(GpioPinValue.High);
                    pinD3.Write(GpioPinValue.High);
                    pinD4.Write(GpioPinValue.Low);
                    vm.LedD1State = false;
                    vm.LedD2State = false;
                    vm.LedD3State = false;
                    vm.LedD4State = true;
                    break;

                default:
                    WriteDigit(SEGMENT_MAP[1], SEGMENT_SELECT[0]);
                    vm.LedDisplayTxt = "1   ";
                    StateMachineState = 0;
                    pinD1.Write(GpioPinValue.Low);
                    pinD2.Write(GpioPinValue.High);
                    pinD3.Write(GpioPinValue.High);
                    pinD4.Write(GpioPinValue.High);
                    vm.LedD1State = true;
                    vm.LedD2State = false;
                    vm.LedD3State = false;
                    vm.LedD4State = false;
                    break;
            }

            StateMachineState++;
        }

        private void WriteDigit(byte segmentMap, byte segmentSelect)
        {
            pinLATCH.Write(GpioPinValue.Low);
            shiftOut(segmentMap, segmentSelect);
            pinLATCH.Write(GpioPinValue.High);
        }

        private void shiftOut(byte segmentMap, byte segmentSelect)
        {
            int value = (segmentMap * 256) + segmentSelect;
            BitArray ba = new BitArray(new int[] { value });

            for (int idx = 15; idx >= 0; idx--)
            {
                pinCLOCK.Write(GpioPinValue.Low);
                if (ba.Get(idx))
                    pinDATA.Write(GpioPinValue.High);
                else
                    pinDATA.Write(GpioPinValue.Low);
                pinCLOCK.Write(GpioPinValue.High);
            }

        }
    }
}
