using GalaSoft.MvvmLight;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MultiFuncBoardDemo.ViewModels
{
    public class MfbDemoViewModel : ViewModelBase
    {
        SolidColorBrush _ledTrueFill;
        SolidColorBrush _switchPressedFill;
        SolidColorBrush _buzzerSoundFill;
        SolidColorBrush _transpFill;
        public MfbDemoViewModel()
        {
            _ledTrueFill = new SolidColorBrush(Colors.LightGreen);
            _switchPressedFill = new SolidColorBrush(Colors.Red);
            _buzzerSoundFill = new SolidColorBrush(Colors.Blue);
            _transpFill = new SolidColorBrush(Colors.Transparent);
        }
        private bool _ledD1State;
        public bool LedD1State
        {
            get
            { return _ledD1State; }
            set
            {
                if(_ledD1State != value)
                {
                    _ledD1State = value;
                    LedD1StateFill = _ledD1State ? _ledTrueFill : _transpFill;
                }
            }
        }
        private bool _ledD2State;
        public bool LedD2State
        {
            get
            { return _ledD2State; }
            set
            {
                if (_ledD2State != value)
                {
                    _ledD2State = value;
                }
            }
        }
        private bool _ledD3State;
        public bool LedD3State
        {
            get
            { return _ledD3State; }
            set
            {
                if (_ledD3State != value)
                {
                    _ledD3State = value;
                }
            }
        }
        private bool _ledD4State;
        public bool LedD4State
        {
            get
            { return _ledD4State; }
            set
            {
                if (_ledD4State != value)
                {
                    _ledD4State = value;
                }
            }
        }
        private SolidColorBrush _ledD1StateFill;
        public SolidColorBrush LedD1StateFill
        {
            get
            { return _ledD1StateFill; }
            set
            {
                if (_ledD1StateFill != value)
                {
                    _ledD1StateFill = value;
                    RaisePropertyChanged("LedD1StateFill");
                }
            }
        }
        private SolidColorBrush _ledD2StateFill;
        public SolidColorBrush LedD2StateFill
        {
            get
            { return _ledD2StateFill; }
            set
            {
                if (_ledD2StateFill != value)
                {
                    _ledD2StateFill = value;
                    RaisePropertyChanged("LedD2StateFill");
                }
            }
        }
        private SolidColorBrush _ledD3StateFill;
        public SolidColorBrush LedD3StateFill
        {
            get
            { return _ledD3StateFill; }
            set
            {
                if (_ledD3StateFill != value)
                {
                    _ledD3StateFill = value;
                    RaisePropertyChanged("LedD3StateFill");
                }
            }
        }
        private SolidColorBrush _ledD4StateFill;
        public SolidColorBrush LedD4StateFill
        {
            get
            { return _ledD4StateFill; }
            set
            {
                if (_ledD4StateFill != value)
                {
                    _ledD4StateFill = value;
                    RaisePropertyChanged("LedD4StateFill");
                }
            }
        }

        private bool _switchS1Pressed;
        public bool SwitchS1Pressed
        {
            get
            { return _switchS1Pressed; }
            set
            {
                if (_switchS1Pressed != value)
                {
                    _switchS1Pressed = value;
                    SwitchS1PressedFill = _switchS1Pressed ? _ledTrueFill : _transpFill;
                }
            }
        }
        private bool _switchS2Pressed;
        public bool SwitchS2Pressed
        {
            get
            { return _switchS2Pressed; }
            set
            {
                if (_switchS2Pressed != value)
                {
                    _switchS2Pressed = value;
                    SwitchS2PressedFill = _switchS2Pressed ? _ledTrueFill : _transpFill;
                }
            }
        }
        private bool _switchS3Pressed;
        public bool SwitchS3Pressed
        {
            get
            { return _switchS3Pressed; }
            set
            {
                if (_switchS3Pressed != value)
                {
                    _switchS3Pressed = value;
                    SwitchS3PressedFill = _switchS3Pressed ? _ledTrueFill : _transpFill;
                }
            }
        }
        private SolidColorBrush _switchS1PressedFill;
        public SolidColorBrush SwitchS1PressedFill
        {
            get
            { return _switchS1PressedFill; }
            set
            {
                if (_switchS1PressedFill != value)
                {
                    _switchS1PressedFill = value;
                    RaisePropertyChanged("SwitchS1PressedFill");
                }
            }
        }
        private SolidColorBrush _switchS2PressedFill;
        public SolidColorBrush SwitchS2PressedFill
        {
            get
            { return _switchS2PressedFill; }
            set
            {
                if (_switchS2PressedFill != value)
                {
                    _switchS2PressedFill = value;
                    RaisePropertyChanged("SwitchS2PressedFill");
                }
            }
        }
        private SolidColorBrush _switchS3PressedFill;
        public SolidColorBrush SwitchS3PressedFill
        {
            get
            { return _switchS3PressedFill; }
            set
            {
                if (_switchS3PressedFill != value)
                {
                    _switchS3PressedFill = value;
                    RaisePropertyChanged("SwitchS3PressedFill");
                }
            }
        }


        private bool _buzzerSound;
        public bool BuzzerSound
        {
            get
            { return _buzzerSound; }
            set
            {
                if (_buzzerSound != value)
                {
                    _buzzerSound = value;
                    SwitchS3PressedFill = _buzzerSound ? _ledTrueFill : _transpFill;
                }
            }
        }
        private SolidColorBrush _buzzerFill;
        public SolidColorBrush BuzzerSoundFill
        {
            get
            { return _buzzerFill; }
            set
            {
                if (_buzzerFill != value)
                {
                    _buzzerFill = value;
                    RaisePropertyChanged("BuzzerSoundFill");
                }
            }
        }
    }
}