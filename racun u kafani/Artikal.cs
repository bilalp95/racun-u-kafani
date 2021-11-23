using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace racun_u_kafani
{
    public class Artikal : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _proizvodjac;

        public string this[string property]
        {
            get
            {
                var rezultat = _majstor.Validate(this);
                var greska = rezultat.Errors
                    .Where(gr => gr.PropertyName == property)
                    .FirstOrDefault();
                if (greska is not null)
                    return greska.ErrorMessage;
                return string.Empty; 
            }
            }
        public string Proizvodjac
        {
            get { return _proizvodjac; }
            set
            {
                _proizvodjac = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Proizvodjac"));
            }
        }

        private string _naziv;
        public string Naziv
        {
            get { return _naziv; }
            set
            {
                _naziv = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Naziv"));
            }
        }

        private decimal _ucena;
        public decimal Ucena
        {
            get { return _ucena; }
            set
            {
                _ucena = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ucena"));
            }
        }

        private int _marza;
        public int Marza
        {
            get { return _marza; }
            set
            {
                _marza = value;
                _cena = _ucena * ((decimal)_marza / 100 + 1);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Marza"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena"));
            }

        }

        private decimal _cena;

        public event PropertyChangedEventHandler? PropertyChanged;

        public decimal Cena
        {
            get { return _cena; }
            set
            {
                _cena = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena"));
                _marza = (int)(_cena / _ucena - 1 * 100);
            }
        }

        public string Error => throw new NotImplementedException();

        private Majstor _majstor = new();
        
      
        public class Majstor : AbstractValidator<Artikal>
        {
            public Majstor()
            {
                RuleFor(a => a.Cena).GreaterThan(0).WithMessage("Cena jok");
                RuleFor(a => a.Marza).GreaterThan(0).WithMessage("Marza jok");
                RuleFor(b => b.Naziv).MinimumLength(3).MaximumLength(10).WithMessage("Naziv Jok");
            }


            public string Error => throw new NotImplementedException();

            public event PropertyChangedEventHandler? PropertyChanged;

        }
    }
}
