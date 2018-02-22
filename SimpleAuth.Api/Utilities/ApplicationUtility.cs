namespace SimpleAuth.Api.Utilities
{
    public static class ApplicationUtility
    {
        public static string GetApplicationTitle()
        {
            string title = @"

              _____ _                 _                      _   _     
             / ____(_)               | |          /\        | | | |    
            | (___  _ _ __ ___  _ __ | | ___     /  \  _   _| |_| |__  
             \___ \| | '_ ` _ \| '_ \| |/ _ \   / /\ \| | | | __| '_ \ 
             ____) | | | | | | | |_) | |  __/  / ____ \ |_| | |_| | | |
            |_____/|_|_| |_| |_| .__/|_|\___| /_/    \_\__,_|\__|_| |_|
                               | |                                     
                               |_|                                     
            ";

            return title;
        }
    }
}
