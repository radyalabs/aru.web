using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Services
{
    public interface ISequencerNumberService
    {
        void Add();
        void Reset();
        int SequenceNumber { get; }
    }

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
