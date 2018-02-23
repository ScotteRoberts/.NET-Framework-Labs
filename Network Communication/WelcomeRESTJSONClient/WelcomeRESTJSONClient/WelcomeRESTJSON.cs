// Fig. 30.11: WelcomeRESTJSON.cs
// Client that consumes WelcomeRESTJSONService.
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace WelcomeRESTJSONClient
{
   public partial class WelcomeRESTJSON : Form
   {
      // object to invoke the WelcomeRESTJSONService
      private HttpClient client = new HttpClient();

      public WelcomeRESTJSON()
      {
         InitializeComponent();
      } // end constructor

      // get user input and pass it to the web service
      private async void submitButton_Click( object sender, EventArgs e )
      {
         // send request to WelcomeRESTJSONService
         string result = await client.GetStringAsync( new Uri(
            "http://localhost:56429/WelcomeRESTJSONService.svc/welcome/" +
            textBox.Text ) );

         // deserialize response into a TextMessage object
         DataContractJsonSerializer JSONSerializer =
            new DataContractJsonSerializer( typeof( TextMessage ) );
         TextMessage message = 
            ( TextMessage ) JSONSerializer.ReadObject(
            new MemoryStream( Encoding.Unicode.GetBytes( result ) ) );

         // display Message text
         MessageBox.Show( message.Message, "Welcome" );
      } // end method submitButton_Click
   } // end class WelcomeRESTJSON

   // TextMessage class representing a JSON object
   [Serializable]
   public class TextMessage
   {
      public string Message;
   } // end class TextMessage
} // end namespace WelcomeRESTJSONClient

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