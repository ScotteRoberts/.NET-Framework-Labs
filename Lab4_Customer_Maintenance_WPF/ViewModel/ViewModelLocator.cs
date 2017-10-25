/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Lab4_Customer_Maintenance_WPF"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Lab4_Customer_Maintenance_WPF.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<CustomerMaintenanceViewModel>();
            SimpleIoc.Default.Register<AddCustomerViewModel>();
            SimpleIoc.Default.Register<ModifyCustomerViewModel>();
        }
        
        public CustomerMaintenanceViewModel CustomerMaintenanceViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CustomerMaintenanceViewModel>();
            }
        }

        public AddCustomerViewModel AddCustomerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddCustomerViewModel>();
            }
        }

        public ModifyCustomerViewModel ModifyCustomerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ModifyCustomerViewModel>();
            }
        }
        

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}