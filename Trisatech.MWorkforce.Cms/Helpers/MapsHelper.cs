using Trisatech.MWorkforce.Cms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Helpers
{
    public class MapsHelper
    {
        public double Distance(LatLng source, LatLng dest)
        {
            Double R = 6371e3;
            double result = 0;

            double Olat1 = _DegreesToRadian(source.Latitude);
            double Olat2 = _DegreesToRadian(dest.Latitude);

            double nLng1 = _DegreesToRadian(source.Longitude);
            double nLng2 = _DegreesToRadian(dest.Longitude);

            double Qo = Olat2 - Olat1;
            double Qn = nLng2 - nLng1;

            var a = Math.Sin(Qo / 2) * Math.Sin(Qo / 2) +
                Math.Cos(Olat1) * Math.Cos(Olat2) *
                Math.Sin(Qn / 2) * Math.Sin(Qn / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            result = R * c;

            return result;

        }

        private double _DegreesToRadian(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private double _RadiansToDegree(double radians)
        {
            return radians / Math.PI * 180;
        }
    }
}
