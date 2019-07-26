using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Deputat
    {
        public int Id { get; set; }

        public int? TerrNumber { get; set; }

        public bool IsMajor { get; set; }

        public string AvatarPath { get; set; }

        public string NameWithInitials { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public virtual ICollection<ChatUser> ChatUsers { get; set; }
    }
}
