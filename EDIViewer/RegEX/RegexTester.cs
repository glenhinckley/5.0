
namespace PIEBALD.RegexTester
{
    public partial class RegexTester
    {
        [System.STAThreadAttribute()]
        public static int 
        Main 
        (
            string[] args
        )
        {
            int result = 0 ;
            
            try
            {
                System.Windows.Forms.Application.EnableVisualStyles() ;
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault ( false ) ;
                System.Windows.Forms.Application.Run ( new frmRegexTester() ) ;
            }
            catch ( System.Exception err )
            {
                System.Console.WriteLine ( err ) ;
            }
            
            return ( result ) ;
        }
    }
}
