using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class NozzleModel : Notifier
    {
        public NozzleModel()
        {
            Error = false;
            Position = "";
            Mark = "";
            Size = "";
            Rating = "";
            Type = "";
            Facing = "";
            R = "";
            H = "";
            Service = "";
            Remarks = "";
        }
        public bool Error
        {
            get { return _Error; }
            set
            {
                _Error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
        private bool _Error;

        public string Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private string _Position;

        public string Mark
        {
            get { return _Mark; }
            set
            {
                _Mark = value;
                OnPropertyChanged(nameof(Mark));
            }
        }
        private string _Mark;

        public string Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
        private string _Size;

        public string Rating
        {
            get { return _Rating; }
            set
            {
                _Rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }
        private string _Rating;

        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        private string _Type;

        public string Facing
        {
            get { return _Facing; }
            set
            {
                _Facing = value;
                OnPropertyChanged(nameof(Facing));
            }
        }
        private string _Facing;

        public string R
        {
            get { return _R; }
            set
            {
                _R = value;
                OnPropertyChanged(nameof(R));
            }
        }
        private string _R;

        public string H
        {
            get { return _H; }
            set
            {
                _H = value;
                OnPropertyChanged(nameof(H));
            }
        }
        private string _H;

        public string Service
        {
            get { return _Service; }
            set
            {
                _Service = value;
                OnPropertyChanged(nameof(Service));
            }
        }
        private string _Service;

        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }
        private string _Remarks;
    }
}
