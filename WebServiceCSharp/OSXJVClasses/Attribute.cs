namespace OSXJV.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class Attribute
    {
        private string name;
        private string value;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
