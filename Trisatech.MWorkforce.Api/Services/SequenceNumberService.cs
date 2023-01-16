using Trisatech.MWorkforce.Api.Interfaces;

namespace Trisatech.MWorkforce.Api.Services
{
    public class SequencerNumberService : ISequencerNumberService, IDisposable
    {
        private static int _number = 0;
        private const int MaxValue = 999999;
        public void Add()
        {
            _number++;
        }

        public int SequenceNumber
        {
            get
            {
                if (_number == MaxValue)
                    Reset();
                _number++;
                return _number;
            }
        }

        public void Dispose()
        {
            _number = 0;
        }

        public void Reset()
        {
            _number = 0;
        }
    }

}
