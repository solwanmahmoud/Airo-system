using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_Airline
{
  public interface ICrudOperation
  { 
    public bool Create();
    public bool Edit();
    public bool Delete();
    public void GetByID();
    public void GetAll();

  }
}
