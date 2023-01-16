namespace Trisatech.MWorkforce.Api.Interfaces
{
    public interface ISequencerNumberService
    {
        void Add();
        void Reset();
        int SequenceNumber { get; }
    }
}
