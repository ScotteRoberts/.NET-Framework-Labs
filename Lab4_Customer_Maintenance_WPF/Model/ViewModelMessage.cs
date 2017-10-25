using GalaSoft.MvvmLight.Messaging;

namespace Lab4_Customer_Maintenance_WPF.Model
{
    // Custom message class to send customer objects.
    class AddToHomeMessage: MessageBase
    {
        public Customer CustomerContext { get; set; }
    }
}
