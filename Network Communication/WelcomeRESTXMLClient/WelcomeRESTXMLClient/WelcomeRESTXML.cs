// Fig. 30.7: WelcomeRESTXML.cs
// Client that consumes the WelcomeRESTXMLService.
using System;
using System.Net.Http;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WelcomeRESTXMLClient
{
   public partial class WelcomeRESTXML : Form
   {
      // object to invoke the WelcomeRESTXMLService
      private HttpClient client = new HttpClient();

      private XNamespace xmlNamespace = XNamespace.Get( 
         "http://schemas.microsoft.com/2003/10/Serialization/" );

      public WelcomeRESTXML()
      {
         InitializeComponent();
      } // end constructor

      // get user input, pass it to web service, and process response
      private async void submitButton_Click( object sender, EventArgs e )
      {
         // send request to WelcomeRESTXMLService
         string result = await client.GetStringAsync( new Uri(
            "http://localhost:55815/WelcomeRESTXMLService.svc/welcome/" +
            textBox.Text ) );

         // parse the returned XML  
         XDocument xmlResponse = XDocument.Parse( result );

         // get the <string> element's value
         MessageBox.Show( xmlResponse.Element(
            xmlNamespace + "string" ).Value, "Welcome" );
      } // end method submitButton_Click
   } // end class WelcomeRESTXML
} // end namespace WelcomeRESTXMLClient 

/**************************************************************************
 * (C) Copyright 1992-2014 by Deitel + Associates, Inc. and               *
 * Pearson Education, Inc. All Rights Reserved.                           *
 *                                                                        *
 * DISCLAIMER: The authors and publisher of this book have used their     *
 * best efforts in preparing the book. These efforts include the          *
 * development, research, and testing of the theories and programs        *
 * to determine their effectiveness. The authors and publisher make       *
 * no warranty of any kind, expressed or implied, with regard to these    *
 * programs or to the documentation contained in these books. The authors *
 * and publisher shall not be liable in any event for incidental or       *
 * consequential damages in connection with, or arising out of, the       *
 * furnishing, performance, or use of these programs.                     *
 **************************************************************************/