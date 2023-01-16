using Trisatech.MWorkforce.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Interfaces
{
    public interface IDashboardService
    {
        Task<List<LocationHistoryViewModel>> GetLocationAgent(string[] regions);
        Task<int> CountTotalTask(string[] regions, DateTime date);
        //Fungsi untuk menghitung jumlah tagihan berdasarkan waktu
        Task<decimal> CountTotalInvoice(string[] regions, DateTime date);
        //Fungsi untuk menghitung jumlah pembayaran berdasarkan waktu
        Task<decimal> CountTotalPayment(string[] regions, DateTime date);
        //Fungsi untuk menghitung jumlah task gagal berdasarkan waktu
        Task<int> CountTaskFailed(string[] regions, DateTime date);
    }
}
