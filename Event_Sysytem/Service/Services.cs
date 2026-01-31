namespace Event_System.Service
{
    public class Services
    {
        public static string getStatusName(int id)
        {
            string statusName = "";

            switch (id)
            {
                case 0: statusName = "Pending"; break;
                case 1: statusName = "Rejected"; break;
                case 2: statusName = "Approved"; break;




            }
            return statusName;


        }
    }
}
