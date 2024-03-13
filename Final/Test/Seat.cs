using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_Airline
{
  internal class Seat
  {
    int seatNum;
    bool isAvaliable;
    public Seat(int seatNum, bool isAvaliable)
    {
      this.seatNum = seatNum;
      this.isAvaliable = isAvaliable;
    }
    public bool Avaliablity
    {
      get { return isAvaliable; }
      set
      {
        isAvaliable = value;
      }
    }
  }
}